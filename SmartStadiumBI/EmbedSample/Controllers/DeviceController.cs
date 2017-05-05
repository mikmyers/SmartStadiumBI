using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GAA.IoT.Models;

using Swashbuckle.Swagger.Annotations;

namespace GAA.IoT.Web.Controllers
{
    public class DeviceController : ApiController
    {
        [SwaggerOperation("GetAllDevices")]
        /// <summary>
        /// Gets a list of all devices
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DeviceModel> Get()
        {
            return DeviceFactory.Instance.Devices;
        }

    }

}
