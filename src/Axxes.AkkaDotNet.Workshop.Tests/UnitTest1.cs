using System;
using Akka.Actor;
using Akka.TestKit.Xunit;
using Axxes.AkkaDotNet.Workshop.ClusterNode.Actors;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;
using Xunit;

namespace Axxes.AkkaDotNet.Workshop.Tests
{
    public class DeviceManagerTests : TestKit
    {
        [Fact]
        public void Test1()
        {
            var subject = Sys.ActorOf<DeviceManagerActor>();

            subject.Tell(new ConnectDevice(Guid.NewGuid()));

            ExpectMsg<DeviceConnected>(TimeSpan.FromSeconds(1));
        }
    }
}