using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Axxes.AkkaDotNet.Workshop.WebPortal.Models;

namespace Axxes.AkkaDotNet.Workshop.WebPortal.System
{
    public interface IActorSystemService
    {
        Task<IEnumerable<Measurement>> GetMeasurements(Guid deviceId, DateTime fromDateTimeUtc, DateTime toDateTimeUtc);
        Task<IEnumerable<Device>> GetAllDevices();
    }
}