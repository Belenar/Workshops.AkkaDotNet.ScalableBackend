using System.Collections.Generic;
using System.Threading.Tasks;
using Axxes.AkkaDotNet.Workshop.WebPortal.Models;
using Axxes.AkkaDotNet.Workshop.WebPortal.System;
using Microsoft.AspNetCore.Mvc;

namespace Axxes.AkkaDotNet.Workshop.WebPortal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly IActorSystemService _actorSystem;

        public DevicesController(IActorSystemService actorSystem)
        {
            _actorSystem = actorSystem;
        }

        /// <summary>
        /// Fetches all available devices.
        /// </summary>
        /// <returns>A list of all available devices.</returns>
        [HttpGet]
        public async Task<IEnumerable<Device>> Get()
        {
            return await _actorSystem.GetAllDevices();
        }
    }
}
