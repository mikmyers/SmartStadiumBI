using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAA.IoT.Models;
// ADD THIS PART TO YOUR CODE
using System.Net;
using Newtonsoft.Json;

using MoreLinq;

namespace GAA.IoT.Web
{
    public class SQLDBHelper
    {
        public SQLDBHelper()
        {
            
        }

        /// <summary>
        /// Returns the latest information for each device over a certain time period in minutes
        /// This method looks at Realtime LAMax figures and gets the largest LAMax figure for the time period
        /// as well as the most recent LAMax data. Max(LAMax) should be used to monitor crowd cheer etc
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public List<DeviceRealTimeSoundDataModel> GetLatestRealtimeSoundData(int minutes )
        {
            var dbContext = new AzureSQLDBDataContext();

            var latestTime = DateTime.Now.ToUniversalTime().AddMinutes(-minutes);

            var data = from d in dbContext.SoundDatas
                       where d.Time >= latestTime && d.LAMax != null && d.LAMax > 0
                       group d by d.DeviceId into g
                       select new DeviceRealTimeSoundDataModel
                       {
                           DeviceId = g.Key,
                           MaximumSoundData = (from x in g orderby x.LAMax descending select x).FirstOrDefault(),
                           LatestSoundData = (from y in g orderby y.Time descending select y).FirstOrDefault()
                       };

            var list = data.ToList();
            return list.OrderBy(x => x.Device.SortIndex).ToList();
        }

        public List<DeviceRealTimeSoundDataModel> GetSoundDataByDate( DateTime date )
        {
            var dbContext = new AzureSQLDBDataContext();

            var data = from d in dbContext.SoundDatas
                       where d.Time.Date.Equals(date.Date) && d.LAMax > 0
                       group d by d.DeviceId into g
                       select new DeviceRealTimeSoundDataModel
                       {
                           DeviceId = g.Key,
                           MaximumSoundData = (from x in g orderby x.LAMax descending select x).FirstOrDefault(),
                           MinimumSoundData = (from y in g orderby y.LAMax ascending select y).FirstOrDefault()
                       };

            return data.ToList().OrderBy(x => x.Device.SortIndex).ToList();
        }

        /// <summary>
        /// Returns the data from a sliding window of 15 minutes for the time window specified in minutes
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public List<DeviceRollingAverageSoundDataModel> GetRollingAverages(int minutes)
        {
            var dbContext = new AzureSQLDBDataContext();

            var devices = DeviceFactory.Instance.Devices;

            var latestTime = DateTime.Now.ToUniversalTime().AddMinutes(-minutes);

            var data = from d in dbContext.SoundDataRollingAverageModels
                       where d.Time >= latestTime && d.AVGLEQ != null && d.AVGLEQ > 0
                       group d by d.DeviceId into g
                       select new DeviceRollingAverageSoundDataModel
                       {
                           DeviceId = g.Key,
                           SoundData = g.OrderBy( l=>l.Time).ToList()
                       };

            // order data by device sort order
            return data.ToList().OrderBy( x=>x.Device.SortIndex).ToList();
        }

        /// <summary>
        /// Returns the data from a sliding window of 15 minutes for the time window specified in minutes
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public List<DeviceRollingAverageSoundDataModel> GetRollingAveragesByDate( DateTime date )
        {
            var dbContext = new AzureSQLDBDataContext();

            var devices = DeviceFactory.Instance.Devices;

            var data = from d in dbContext.SoundDataRollingAverageModels
                       where d.Time.Value.Date.Equals(date.Date) && d.AVGLEQ != null && d.AVGLEQ > 0
                       group d by d.DeviceId into g
                       select new DeviceRollingAverageSoundDataModel
                       {
                           DeviceId = g.Key,
                           SoundData = g.OrderBy(l => l.Time).ToList()
                       };

            // order data by device sort order
            return data.ToList().OrderBy(x => x.Device.SortIndex).ToList();
        }

    }
}
