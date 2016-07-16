using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebP.Web.Enums;
using WebP.Web.Interfaces;

namespace WebP.Web
{
    public class FileDetails : IFileDetails
    {
        public string FileName { get; set; }

        public string FileExtension { get; set; }

        public string OriginalFileLocation { get; set; }

        public double OriginalSize { get; set; }

        public string OriginalSizeFormatted { get; set; }

        public string WebPFileLocation { get; set; }

        public double WebPSize { get; set; }

        public string WebPSizeFormatted { get; set; }

        public double Savings
        {
            get
            {
                return UseOriginal ? 0 : (OriginalSize - WebPSize);
            }
        }
        public string SavingsFormatted
        {
            get
            {
                return ConvertBytes2ReadableFormat.FormatByteSize(UseOriginal ? 0 : (OriginalSize - WebPSize) * 1024);
            }
        }
        

        public double SavingsInPercentage
        {
            get
            {
                return UseOriginal ? 0 : (Savings / OriginalSize) * 100;
            }
        }

        public ConversionStatus Status { get; set; }

        public bool UseOriginal
        {
            get
            {
                return (OriginalSize - WebPSize) < 0;
            }
        }
    }
}