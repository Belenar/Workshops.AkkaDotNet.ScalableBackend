using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Axxes.AkkaDotNet.Workshop.WebPortal.Models;
using Axxes.AkkaDotNet.Workshop.WebPortal.System;
using Microsoft.AspNetCore.Mvc;

namespace Axxes.AkkaDotNet.Workshop.WebPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementsController : ControllerBase
    {
        private readonly IActorSystemService _actorSystem;

        public MeasurementsController(IActorSystemService actorSystem)
        {
            _actorSystem = actorSystem;
        }
        /// <summary>
        /// Gets 24 hours worth of data, starting from the supplied timestamp.
        /// </summary>
        /// <param name="deviceId">The id of the device to be queried.</param>
        /// <param name="fromDateTimeUtc">The start time of the 24 hour period.</param>
        /// <returns>24 hours of measurements in 5 minute buckets.</returns>
        [HttpGet]
        [Route("{deviceId}/{fromDateTimeUtc}")]
        public async Task<IEnumerable<Measurement>> Get([FromRoute] Guid deviceId, [FromRoute] DateTime fromDateTimeUtc)
        {
            var toDateTimeUtc = fromDateTimeUtc.AddDays(1);
            return await _actorSystem.GetMeasurements(deviceId, fromDateTimeUtc, toDateTimeUtc);
        }
    }
}
