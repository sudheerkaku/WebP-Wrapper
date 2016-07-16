using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebP.Library;
using WebP.Web.Enums;

namespace WebP.Web.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// To trigger WebP Encode.
        /// </summary>
        /// <param name="L">Location</param>
        /// <param name="T">OptimizationType</param>
        /// <param name="M">OptimizationMode</param>
        /// <returns></returns>
        public ActionResult Index(string L, string T, string M)
        {
            List<FileDetails> fileDetailsList = new List<FileDetails>();
            string imageDestination = ConfigurationManager.AppSettings["ImageDestinationLocalFolder"]; //Default Location to Local Image folder.

            if (!string.IsNullOrWhiteSpace(L)) //Change Image location based on input (L).
                imageDestination = L.ToUpper().Equals("SERVER") ? ConfigurationManager.AppSettings["ImageDestinationServerFolder"] : ConfigurationManager.AppSettings["ImageDestinationLocalFolder"];

            //Optimization Type & Mode - default values (Advanced & LossLess).
            OptimizationType optimizationType = string.IsNullOrWhiteSpace(T) ? OptimizationType.Advanced : (OptimizationType)Enum.Parse(typeof(OptimizationType), T, true);
            OptimizationMode optimizationMode = string.IsNullOrWhiteSpace(M) ? OptimizationMode.LossLess : (OptimizationMode)Enum.Parse(typeof(OptimizationMode), M, true);

            DirectoryInfo directoryInfo = new DirectoryInfo(imageDestination);
            foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.*", SearchOption.AllDirectories))
            {
                FileDetails fileDetails = new FileDetails();
                fileDetails.FileName = fileInfo.Name;
                fileDetails.FileExtension = fileInfo.Extension;
                fileDetails.OriginalFileLocation = fileInfo.DirectoryName;
                fileDetails.OriginalSize = ConvertBytes2ReadableFormat.ByteToBits(fileInfo);
                fileDetails.OriginalSizeFormatted = ConvertBytes2ReadableFormat.FormatByteSize(fileInfo);
                fileDetails.Status = ConversionStatus.Started;

                string folderStructure = fileInfo.DirectoryName.Replace(imageDestination, string.Empty);
                EncodeImage(ref fileDetails, folderStructure, OptimizationMode.LossLess, OptimizationType.Advanced);
                
                fileDetailsList.Add(fileDetails);
            }

            return View(fileDetailsList);
        }

        /// <summary>
        /// Encode Image
        /// </summary>
        /// <param name="fileDetails"></param>
        /// <param name="folderStructure"></param>
        /// <param name="optimizationMode"></param>
        /// <param name="optimizationType"></param>
        public void EncodeImage(ref FileDetails fileDetails, string folderStructure, OptimizationMode optimizationMode, OptimizationType optimizationType)
        {
            try
            {
                byte[] rawWebP;
                
                //get the image using complete path
                Bitmap bmp = new Bitmap(string.Format("{0}\\{1}", fileDetails.OriginalFileLocation, fileDetails.FileName));

                //Test encode near lossless mode in memory with quality 40 and speed 9
                // quality 100: No-loss (bit-stream same as -lossless).
                // quality 80: Very very high PSNR (around 54dB) and gets an additional 5-10% size reduction over WebP-lossless image.
                // quality 60: Very high PSNR (around 48dB) and gets an additional 20%-25% size reduction over WebP-lossless image.
                // quality 40: High PSNR (around 42dB) and gets an additional 30-35% size reduction over WebP-lossless image.
                // quality 20 (and below): Moderate PSNR (around 36dB) and gets an additional 40-50% size reduction over WebP-lossless image.
                
                fileDetails.WebPFileLocation = string.Concat(ConfigurationManager.AppSettings["WebPDestinationLocalFolder"], folderStructure);

                //string withOptimizationDetails = fileDetails.FileName.Replace(fileDetails.FileExtension, string.Format("-{0}-{1}.webp", optimizationMode.ToString(), optimizationType.ToString()));
                string WebPFileName = Path.Combine(fileDetails.WebPFileLocation, fileDetails.FileName.Replace(fileDetails.FileExtension, ".webp"));
                
                using (clsWebP webp = new clsWebP())
                {
                    if (optimizationMode.Equals(OptimizationMode.Lossy))
                    {
                        switch (optimizationType)
                        {
                            case OptimizationType.Advanced:
                                rawWebP = webp.EncodeLossy(bmp, 75, 9);
                                break;
                            case OptimizationType.Near:
                            default:
                                rawWebP = webp.EncodeLossy(bmp, 75);
                                break;
                        }
                    }
                    else //OptimizationMode.LossLess
                    {
                        switch (optimizationType)
                        {
                            case OptimizationType.Advanced:
                                rawWebP = webp.EncodeLossless(bmp, 9);
                                break;
                            case OptimizationType.Near:
                                rawWebP = webp.EncodeNearLossless(bmp, 40, 9);
                                break;
                            default:
                                rawWebP = webp.EncodeLossless(bmp);
                                break;
                        }
                    }
                    fileDetails.Status = ConversionStatus.FinishedEncoding;
                    fileDetails.WebPSize = ConvertBytes2ReadableFormat.ByteToBits(rawWebP.Length);
                    fileDetails.WebPSizeFormatted = ConvertBytes2ReadableFormat.FormatByteSize(rawWebP.Length);
                    if (fileDetails.OriginalSize > fileDetails.WebPSize)
                    {
                        FileInfo file = new FileInfo(WebPFileName);
                        file.Directory.Create(); // If the directory already exists, this method does nothing.
                        System.IO.File.WriteAllBytes(file.FullName, rawWebP);
                        fileDetails.Status = ConversionStatus.Success;
                    }
                    else
                        fileDetails.Status = ConversionStatus.UseOriginal;
                }
            }
            catch (Exception ex)
            {
                if (fileDetails.Status.Equals(ConversionStatus.Started))
                    fileDetails.Status = ConversionStatus.FailedToEncode;
                else if (fileDetails.Status.Equals(ConversionStatus.FinishedEncoding))
                    fileDetails.Status = ConversionStatus.FailedToCreateFile;
            }
        }
    }
}