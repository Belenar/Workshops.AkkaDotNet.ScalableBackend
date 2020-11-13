using System;
using System.Collections.Generic;
using System.Linq;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.State
{
    class NormalizedReadingPersistenceState
    {
        public List<ReadingPersistenceStateItem> Items { get; } = new List<ReadingPersistenceStateItem>();

        public void Add(NormalizedMeterReading reading)
        {
            Items.Add(new ReadingPersistenceStateItem { Reading = reading, Saved = false });
        }

        public NormalizedMeterReading[] GetUnsavedItems()
        {
            return Items.Where(i => !i.Saved).Select(i => i.Reading).ToArray();
        }

        public void SetSavedUntil(DateTime until)
        {
            foreach (var item in Items.Where(i => i.Reading.Timestamp <= until))
            {
                item.Saved = true;
            }
        }

        public NormalizedMeterReading[] GetLastReadings(int numberOfReadings)
        {
            var numberOfReturnedReadings = Math.Min(numberOfReadings, Items.Count);

            if (numberOfReturnedReadings == 0)
                return Array.Empty<NormalizedMeterReading>();

            return Items
                .Select(i => i.Reading)
                .OrderByDescending(r => r.Timestamp)
                .Take(numberOfReturnedReadings)
                .OrderBy(r => r.Timestamp)
                .ToArray();
        }

        public void Truncate()
        {
            if (Items.Any())
            {
                var bottomDate = Items.Last().Reading.Timestamp.AddHours(-12);
                Items.RemoveAll(i => i.Reading.Timestamp < bottomDate && i.Saved);
            }
        }

        public class ReadingPersistenceStateItem
        {
            public NormalizedMeterReading Reading { get; set; }
            public bool Saved { get; set; }
        }
    }
}
