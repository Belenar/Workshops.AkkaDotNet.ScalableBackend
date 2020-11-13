using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Akka.Actor;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.WebPortal.Actors
{
    public class AllDevicesActor : ReceiveActor
    {
        private readonly IActorRef _deviceBroadcastGroup;
        private Dictionary<Guid, IActorRef> _deviceManagerLocator = new Dictionary<Guid, IActorRef>();

        public AllDevicesActor(IActorRef deviceBroadcastGroup)
        {
            _deviceBroadcastGroup = deviceBroadcastGroup;
            Context.System.Scheduler.ScheduleTellRepeatedly(TimeSpan.FromSeconds(30), TimeSpan.FromMinutes(5), Self, new RefreshDeviceActorList(), Self);

            Receive<RefreshDeviceActorList>(HandleRefreshDeviceActorList);
            Receive<AllDeviceIds>(HandleAllDeviceIds);
            Receive<GetAllDeviceIds>(HandleGetAllDeviceIds);
            Receive<ConnectDevice>(HandleConnectDevice);
        }

        private void HandleRefreshDeviceActorList(RefreshDeviceActorList msg)
        {
            _deviceBroadcastGroup.Tell(new GetAllDeviceIds());
        }

        private void HandleAllDeviceIds(AllDeviceIds msg)
        {
            foreach (var msgDeviceId in msg.DeviceIds)
            {
                _deviceManagerLocator[msgDeviceId] = Sender;
            }
        }

        private void HandleGetAllDeviceIds(GetAllDeviceIds msg)
        {
            Sender.Tell(new AllDeviceIds(ImmutableArray.Create(_deviceManagerLocator.Keys.ToArray())));
        }

        private void HandleConnectDevice(ConnectDevice msg)
        {
            _deviceManagerLocator[msg.Id].Forward(msg);
        }

        public static Props CreateProps(IActorRef deviceBroadcastGroup)
        {
            return Props.Create<AllDevicesActor>(deviceBroadcastGroup);
        }
    }
}