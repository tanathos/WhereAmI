// Guids.cs
// MUST match guids.h
using System;

namespace Recoding.WhereAmI
{
    static class GuidList
    {
        public const string guidWhereAmIPkgString = "bc5921ff-f09d-44f7-a81f-898e0db311ce";
        public const string guidWhereAmICmdSetString = "c2dd10f8-0ec5-413c-8fcc-d99586715a69";

        public static readonly Guid guidWhereAmICmdSet = new Guid(guidWhereAmICmdSetString);
    };
}