using Akka.TestKit.Xunit2;
using Akka.Actor;
using Axxes.AkkaDotNet.Workshop.ClusterNode.Actors;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.Tests
{
    public class DeviceManagerTest : TestKit
    {
        [Fact]
        public void WhenConnectIsSent_ThenChildIsCreated()
        {
            var subject = Sys.ActorOf<DeviceManagerActor>();

            var connectMsg = new ConnectDevice(Guid.NewGuid());

            //inject the probe by passing it to the test subject
            //like a real resource would be passing in production
            subject.Tell(connectMsg, TestActor);

            ExpectMsg<DeviceConnected>(msg =>
                msg.DeviceId == connectMsg.DeviceId &&
                msg.DeviceActor != null &&
                msg.DeviceActor != ActorRefs.Nobody
            );
        }
    }
}