using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebP.Web.Enums;

namespace WebP.Web.Interfaces
{
    interface IFileDetails
    {
        /// <summary>
        /// File Name
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// File Extension
        /// </summary>
        string FileExtension { get; set; }

        /// <summary>
        /// Original File Location
        /// </summary>
        string OriginalFileLocation { get; set; }

        /// <summary>
        /// Original Size
        /// </summary>
        double OriginalSize { get; set; }

        /// <summary>
        /// Original Size Formatted
        /// </summary>
        string OriginalSizeFormatted { get; set; }

        /// <summary>
        /// WebP File Location
        /// </summary>
        string WebPFileLocation { get; set; }

        /// <summary>
        /// WebP Size
        /// </summary>
        double WebPSize { get; set; }

        /// <summary>
        /// WebP Size Formatted
        /// </summary>
        string WebPSizeFormatted { get; set; }

        /// <summary>
        /// Savings
        /// </summary>
        double Savings { get; }

        /// <summary>
        /// Savings Formatted
        /// </summary>
        string SavingsFormatted { get; }

        /// <summary>
        /// Savings In Percentage
        /// </summary>
        double SavingsInPercentage { get; }

        /// <summary>
        /// Status
        /// </summary>
        ConversionStatus Status { get; set; }

        /// <summary>
        /// Use Original File (original file is smaller).
        /// </summary>
        bool UseOriginal { get; }
    }
}
