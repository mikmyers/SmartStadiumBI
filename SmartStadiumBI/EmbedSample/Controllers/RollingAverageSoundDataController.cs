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
    public class RollingAverageSoundDataController : ApiController
    {

        [SwaggerOperation("GetRollingAverageSoundData")]
        public IEnumerable<DeviceRollingAverageSoundDataModel> Get(int minutes)
        {
            return MvcApplication.SqlDBHelper.GetRollingAverages(minutes);
        }
    }

    public class RollingAverageDateSoundDataController : ApiController
    {

        [SwaggerOperation("GetRollingAverageSoundDataByDate")]
        public IEnumerable<DeviceRollingAverageSoundDataModel> Get( DateTime date )
        {
            return MvcApplication.SqlDBHelper.GetRollingAveragesByDate( date );
        }
    }
}
