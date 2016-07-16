using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebP.Web.Enums
{
    public enum OptimizationMode
    {
        LossLess = 1,
        Lossy = 2
    }

    public enum OptimizationType
    {
        Advanced = 1,
        Near = 2,
        Simple = 3
    }

    public enum ConversionStatus
    {
        Started = 1,
        FinishedEncoding = 2,
        FailedToEncode = 3,
        FailedToCreateFile = 4,
        Success = 5,
        UseOriginal = 6
    }

    public enum FileSizeUnit : byte
    {
        B,
        KB,
        MB,
        GB,
        TB,
        PB,
        EB,
        ZB,
        YB
    }
}