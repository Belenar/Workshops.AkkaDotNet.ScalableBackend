namespace Axxes.AkkaDotNet.Workshop.Shared.Messages
{
    public class RequestLastNormalizedReadings
    {
        public int NumberOfReadings { get; }

        public RequestLastNormalizedReadings(int numberOfReadings)
        {
            NumberOfReadings = numberOfReadings;
        }
    }
}
