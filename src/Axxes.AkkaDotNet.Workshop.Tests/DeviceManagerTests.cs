using System;
using System.Linq;
using Akka.TestKit.Xunit2;
using Axxes.AkkaDotNet.Workshop.ClusterNode.Actors;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;
using Xunit;

namespace Axxes.AkkaDotNet.Workshop.Tests
{
    public class DeviceManagerTests : TestKit
    {
        [Fact]
        public void ConnectDeviceCreatesChildActor()
        {
            var deviceId = Guid.NewGuid();

            var deviceManagerProps = DeviceManagerActor.CreateProps();
            var manager = ActorOfAsTestActorRef<DeviceManagerActor>(deviceManagerProps, TestActor);

            manager.Tell(new ConnectDevice(deviceId));

            ExpectMsg<DeviceConnected>(msg => msg.DeviceActor.Path.Elements.Last() == $"device-{deviceId}");
        }
    }
}