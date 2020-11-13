using System;
using System.Collections.Immutable;

namespace Axxes.AkkaDotNet.Workshop.Shared.Messages
{
    public class AllDeviceIds
    {
        public ImmutableArray<Guid> DeviceIds { get; }

        public AllDeviceIds(ImmutableArray<Guid> deviceIds)
        {
            DeviceIds = deviceIds;
        }
    }
}