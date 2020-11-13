using System;
using System.Linq;
using Xunit;
using Akka.TestKit.Xunit2;
using Axxes.AkkaDotNet.Workshop.ClusterNode.Actors;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.Tests
{
    public class DeviceManagerActorTests : TestKit
    {
        [Fact]
        public void ConnectDeviceCreatesChildActor()
        {
            var deviceId = Guid.NewGuid();

            var managerProps = DeviceManagerActor.CreateProps();

            var deviceManager = ActorOfAsTestActorRef<DeviceManagerActor>(managerProps, TestActor);

            deviceManager.Tell(new ConnectDevice(deviceId));

            ExpectMsg<DeviceConnected>(msg => msg.DeviceRef.Path.Elements.Last() == $"device-{deviceId}");
        }
    }
}
