using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Axxes.AkkaDotNet.Workshop.WebPortal.Models;

namespace Axxes.AkkaDotNet.Workshop.WebPortal.System
{
    public interface IActorSystemService
    {
        IEnumerable<Measurement> GetMeasurements(Guid deviceId, in DateTime fromDateTimeUtc, in DateTime toDateTimeUtc);
        Task<IEnumerable<Device>> GetAllDevices();
    }
}