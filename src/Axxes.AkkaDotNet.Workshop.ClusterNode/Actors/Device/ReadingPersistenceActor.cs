using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.Persistence;
using Axxes.AkkaDotNet.Workshop.ClusterNode.State;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors.Device
{
    class ReadingPersistenceActor : ReceivePersistentActor
    {
        private readonly Guid _deviceId;
        private NormalizedReadingPersistenceState _state = new NormalizedReadingPersistenceState();
        private readonly IActorRef _dbWriterActor;

        public ReadingPersistenceActor(Guid deviceId)
        {
            _deviceId = deviceId;

            Command<NormalizedMeterReading>(HandleNormalizeMeterReadingCommand);
            Command<RequestLastNormalizedReadings>(HandleRequestLastNormalizedReading);
            Command<TakeHourlySnapshot>(_ => CreateHourlySnapshot());
            Command<WrittenReadingsToDatabase>(HandleWrittenReadingsToDatabaseCommand);

            Recover<SnapshotOffer>(HandleSnapshotOffer);
            Recover<NormalizedMeterReading>(HandleNormalizeMeterReading);
            Recover<WrittenReadingsToDatabase>(HandleWrittenReadingsToDatabase);

            _dbWriterActor = Context.ActorOf(ReadingDbWriterActor.CreateProps(_deviceId));

            ScheduleSnapshots();
        }

        #region Snapshots

        /// <summary>
        /// To spread all the snapshot activity over the hour, we schedule these messages at a random
        /// time in the first hour, and every hour after that. It will trigger the following:
        /// - save of a snapshot
        /// - trigger the save of historic values
        /// - truncate the current state back to 12 hours
        /// </summary>
        private void ScheduleSnapshots()
        {
            var seconds = new Random().Next(3600);
            var initialDelay = new TimeSpan(0, 0, 0, seconds);
            var interval = new TimeSpan(0, 1, 0, 0);
            Context.System.Scheduler.ScheduleTellRepeatedly(initialDelay, interval, Context.Self, new TakeHourlySnapshot(), Context.Self);
        }

        /// <summary>
        /// Restores the last snapshot
        /// </summary>
        private void HandleSnapshotOffer(SnapshotOffer offer)
        {
            if (offer.Snapshot is NormalizedReadingPersistenceState state)
                _state = state;
        }

        private void CreateHourlySnapshot()
        {
            SaveSnapshot(_state);
            _dbWriterActor.Tell(new WriteReadingsToDatabase(ImmutableArray.Create(_state.GetUnsavedItems())));
        }

        #endregion

        #region NormalizedMeterReading

        private void HandleNormalizeMeterReadingCommand(NormalizedMeterReading message)
        {
            Persist(message, msg => HandleNormalizeMeterReading(message));
        }

        private void HandleNormalizeMeterReading(NormalizedMeterReading message)
        {
            _state.Add(message);
        }

        #endregion

        #region WrittenReadingsToDatabase

        private void HandleWrittenReadingsToDatabaseCommand(WrittenReadingsToDatabase message)
        {
            Persist(message, msg => HandleWrittenReadingsToDatabase(message));
        }

        private void HandleWrittenReadingsToDatabase(WrittenReadingsToDatabase message)
        {
            _state.SetSavedUntil(message.WrittenToDate);
            _state.Truncate();
        }

        #endregion

        #region RequestLastNormalizedReadings

        private void HandleRequestLastNormalizedReading(RequestLastNormalizedReadings message)
        {
            var lastReadings = _state.GetLastReadings(message.NumberOfReadings);
            var response = new ReturnLastNormalizedReadings(ImmutableArray.Create(lastReadings));
            Sender.Tell(response);
        }

        #endregion

        public static Props CreateProps(Guid deviceId)
        {
            return Props.Create<ReadingPersistenceActor>(deviceId);
        }

        public override string PersistenceId => $"value-persistence-{_deviceId}";
    }
}
