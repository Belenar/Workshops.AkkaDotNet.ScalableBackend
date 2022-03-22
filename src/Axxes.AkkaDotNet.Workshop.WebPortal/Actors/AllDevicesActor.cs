﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Akka.Actor;
using Akka.Cluster;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.WebPortal.Actors
{
    public class AllDevicesActor : ReceiveActor
    {
        private readonly IActorRef _deviceBroadcastGroup;
        private readonly Cluster _cluster = Cluster.Get(Context.System);
        private readonly Dictionary<Guid, IActorRef> _deviceManagerLocator = new ();

        public AllDevicesActor(IActorRef deviceBroadcastGroup)
        {
            _deviceBroadcastGroup = deviceBroadcastGroup;

            Receive<ClusterEvent.MemberUp>(memberUp => memberUp.Member.Address == _cluster.SelfAddress, _ => TriggerDeviceListRefresh());
            Receive<RefreshDeviceActorList>(_ => TriggerDeviceListRefresh());

            Receive<AllDeviceIds>(HandleAllDeviceIds);
            Receive<GetAllDeviceIds>(HandleGetAllDeviceIds);
            Receive<ConnectDevice>(HandleConnectDevice);
        }

        private void TriggerDeviceListRefresh()
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

        protected override void PreStart()
        {
            Context.System.Scheduler.ScheduleTellRepeatedly(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5), Self, new RefreshDeviceActorList(), Self);
            _cluster.Subscribe(Self, typeof(ClusterEvent.MemberUp));
        }

        protected override void PostStop()
        {
            _cluster.Unsubscribe(Self);
            base.PostStop();
        }
    }
}