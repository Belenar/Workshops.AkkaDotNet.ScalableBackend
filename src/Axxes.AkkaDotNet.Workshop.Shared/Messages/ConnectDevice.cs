using System;

namespace Axxes.AkkaDotNet.Workshop.Shared.Messages
{
    public class ConnectDevice
    {
        public Guid Id { get; }


        public ConnectDevice(Guid id)
        {
            Id = id;
        }
    }
}