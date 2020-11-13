using System;
using System.Collections.Generic;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Helpers
{
    class ValueNormalizationHelper
    {
        private bool _isReferenceReadingSet;
        private decimal _referenceReading;
        private int _referenceBucket;
        private DateTime _lastTimestamp;
        private decimal _lastReading;

        public IEnumerable<NormalizedMeterReading> GetNormalizedReadingsUntil(DateTime currentTime, decimal currentReading)
        {
            if (!_isReferenceReadingSet)
            {
                SetReferenceReading(currentTime, currentReading);
            }
            else
            {
                if (currentTime.BucketNumber() > _referenceBucket)
                {
                    // We should compute new Normalized Readings
                    foreach (var normalizedValue in ComputeNormalizedValues(currentTime, currentReading))
                    {
                        yield return normalizedValue;
                    }
                }
            }

            SetLastReading(currentTime, currentReading);
        }

        private void SetReferenceReading(DateTime referenceTime, decimal referenceReading)
        {
            _referenceBucket = referenceTime.BucketNumber();
            _referenceReading = referenceReading;
            _isReferenceReadingSet = true;
        }
        
        private void SetLastReading(DateTime currentTime, decimal currentReading)
        {
            _lastTimestamp = currentTime;
            _lastReading = currentReading;
        }

        private IEnumerable<NormalizedMeterReading> ComputeNormalizedValues(DateTime currentTime, decimal currentReading)
        {
            var newReferenceReading = 0M;
            var newReferenceTime = DateTime.MinValue;

            var endBucketNumber = currentTime.BucketNumber();

            for (int bucket = _referenceBucket + 1; bucket <= endBucketNumber; bucket++)
            {
                var normalizedValue = GetIntermediateValue(currentTime, currentReading, bucket.BucketDate());

                newReferenceReading = normalizedValue.MeterReading;
                newReferenceTime = normalizedValue.Timestamp;

                yield return normalizedValue;
            }

            if (newReferenceReading != 0M)
            {
                SetReferenceReading(newReferenceTime, newReferenceReading);
            }
        }

        private NormalizedMeterReading GetIntermediateValue(DateTime currentTime, decimal currentReading, DateTime bucketDate)
        {
            if (bucketDate == currentTime)
            {
                var consumption = currentReading - _referenceReading;
                return new NormalizedMeterReading(bucketDate, consumption, currentReading);
            }
            
            var millisecondsBetweenMessages = (decimal)(currentTime - _lastTimestamp).TotalMilliseconds;
            var millisecondsFromPreviousMessage = (decimal)(bucketDate - _lastTimestamp).TotalMilliseconds;

            var consumptionBetweenMessages = currentReading - _lastReading;
            var consumptionSinceLastMessage = consumptionBetweenMessages * (millisecondsFromPreviousMessage / millisecondsBetweenMessages);

            var bucketReading = _lastReading + consumptionSinceLastMessage;
            var bucketConsumption = bucketReading - _referenceReading;

            return new NormalizedMeterReading(bucketDate, bucketConsumption, bucketReading);
        }
    }
}

