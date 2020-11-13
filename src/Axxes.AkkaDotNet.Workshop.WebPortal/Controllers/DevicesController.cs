using System.Collections.Generic;
using Axxes.AkkaDotNet.Workshop.WebPortal.Models;
using Axxes.AkkaDotNet.Workshop.WebPortal.System;
using Microsoft.AspNetCore.Mvc;

namespace Axxes.AkkaDotNet.Workshop.WebPortal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DevicesController : ControllerBase
    {
        private static IEnumerable<Device> _devices;

        private IEnumerable<Device> Devices => _devices ??= _actorSystem.GetAllDevices();

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
        public IEnumerable<Device> Get()
        {
            return Devices;
        }
    }
}
