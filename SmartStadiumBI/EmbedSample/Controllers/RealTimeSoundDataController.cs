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
    public class RealTimeSoundDataController : ApiController
    {
        [SwaggerOperation("GetLatestSoundData", OperationId = "GetLatestSoundData")]
        public IEnumerable<DeviceRealTimeSoundDataModel> Get( int minutes )
        {
            return MvcApplication.SqlDBHelper.GetLatestRealtimeSoundData(minutes );
        }
    }

    public class RealTimeDateSoundDataController : ApiController
    {
        [SwaggerOperation("GetSoundDataForDate", OperationId = "GetSoundDataForDate")]
        public IEnumerable<DeviceRealTimeSoundDataModel> Get(DateTime date)
        {
            return MvcApplication.SqlDBHelper.GetSoundDataByDate( date );
        }
    }
}