using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.Cluster;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.WebPortal.Actors
{
    public class AllDevicesActor : ReceiveActor
    {
        private readonly IActorRef _broadcastRouter;
        private Dictionary<Guid, IActorRef> _managerForDevices = new();
        private Cluster _cluster;

        public AllDevicesActor(IActorRef broadcastRouter)
        {
            _broadcastRouter = broadcastRouter;
            Receive<AllDeviceIds>(HandleAllDeviceIds);
            Receive<GetAllDeviceIds>(HandleGetAllDeviceIds);
            Receive<ClusterEvent.MemberUp>(
                message => message.Member.Address == _cluster.SelfAddress,
                HandleMemberUp);
        }

        private void HandleMemberUp(ClusterEvent.MemberUp message)
        {
            _broadcastRouter.Tell(new GetAllDeviceIds());
        }

        protected override void PreStart()
        {
            _cluster = Cluster.Get(Context.System);
            _cluster.Subscribe(Self, typeof(ClusterEvent.MemberUp));
        }

        private void HandleAllDeviceIds(AllDeviceIds message)
        {
            foreach (var deviceId in message.DeviceIds)
            {
                _managerForDevices[deviceId] = Sender;
            }
        }

        private void HandleGetAllDeviceIds(GetAllDeviceIds obj)
        {
            Sender.Tell(new AllDeviceIds(_managerForDevices.Keys.ToImmutableArray()));
        }

        public static Props CreateProps(IActorRef broadcastRouter)
        {
            return Props.Create<AllDevicesActor>(broadcastRouter);
        }
    }
}
