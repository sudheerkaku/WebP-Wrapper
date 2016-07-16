using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebP.Web.Enums;

namespace WebP.Web
{
    public class ConvertBytes2ReadableFormat
    {
        /// <summary>
        /// Converts a numeric value into a string that represents the number expressed as a size value in bytes,
        /// kilobytes, megabytes, or gigabytes...
        /// </summary>
        /// <param name="fileSize">The numeric value to be converted.</param>
        /// <returns>The converted string.</returns>
        public static string FormatByteSize(double fileSize)
        {
            FileSizeUnit unit = FileSizeUnit.B;
            while (fileSize >= 1024 && unit < FileSizeUnit.YB)
            {
                fileSize = fileSize / 1024;
                unit++;
            }

            return string.Format("{0:0.##} {1}", fileSize, unit);
        }

        /// <summary>
        /// Converts a numeric value into a string that represents the number expressed as a size value in bytes,
        /// kilobytes, megabytes, or gigabytes...
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns>The converted string.</returns>
        public static string FormatByteSize(FileInfo fileInfo)
        {
            return FormatByteSize(fileInfo.Length);
        }

        /// <summary>
        /// Converts a numeric value into a string that represents the number expressed as a size value in bytes,
        /// kilobytes, megabytes, or gigabytes...
        /// </summary>
        /// <param name="fileSize">The numeric value to be converted.</param>
        /// <returns>The converted string.</returns>
        public static double ByteToBits(double fileSize)
        {
            FileSizeUnit unit = FileSizeUnit.B;
            while (fileSize >= 1024 && unit < FileSizeUnit.YB)
            {
                fileSize = fileSize / 1024;
                unit++;
            }
            return fileSize;
        }

        /// <summary>
        /// Converts a numeric value into a string that represents the number expressed as a size value in bytes,
        /// kilobytes, megabytes, or gigabytes...
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns>The converted string.</returns>
        public static double ByteToBits(FileInfo fileInfo)
        {
            return ByteToBits(fileInfo.Length);
        }
    }
}