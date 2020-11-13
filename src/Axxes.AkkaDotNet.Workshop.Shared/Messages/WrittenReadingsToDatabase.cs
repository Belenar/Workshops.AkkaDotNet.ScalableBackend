using System;

namespace Axxes.AkkaDotNet.Workshop.Shared.Messages
{
    public class WrittenReadingsToDatabase
    {
        public DateTime WrittenToDate { get; }

        public WrittenReadingsToDatabase(DateTime writtenToDate)
        {
            WrittenToDate = writtenToDate;
        }
    }
}