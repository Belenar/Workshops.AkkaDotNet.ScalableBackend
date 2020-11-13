using System;
using System.Collections.Generic;
using Akka.Actor;
using Axxes.AkkaDotNet.Workshop.ClusterNode.Actors.Device;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors
{
    public class DeviceManagerActor : ReceiveActor
    {
        private readonly Dictionary<Guid, IActorRef> _deviceActors = new Dictionary<Guid, IActorRef>();

        public DeviceManagerActor()
        {
            Receive<ConnectDevice>(HandleConnectDevice);
        }

        private void HandleConnectDevice(ConnectDevice request)
        {
            if (!_deviceActors.ContainsKey(request.Id))
            {
                CreateDeviceActor(request.Id);
            }
            var response = new DeviceConnected(_deviceActors[request.Id]);
            Sender.Tell(response);
        }

        private void CreateDeviceActor(Guid deviceId)
        {
            var props = DeviceActor.CreateProps(deviceId);
            var name = $"device-{deviceId}";
            var deviceActorRef = Context.ActorOf(props, name);

            _deviceActors[deviceId] = deviceActorRef;
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy((exception) =>
            {
                if (exception is NotImplementedException)
                    return Directive.Resume;
                return Directive.Restart;
            });
        }

        public static Props CreateProps()
        {
            return Props.Create<DeviceManagerActor>();
        }
    }
}
