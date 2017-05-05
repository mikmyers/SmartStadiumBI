using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace GAA.IoT.Models
{
    public class SoundDataModel : TableEntity
    {
        public SoundDataModel()
        {
          //  this.PartitionKey = this.DeviceId;
          //  this.RowKey = this.Time;
        }


        public DateTime Time { get; set; }

        /// <summary>
        /// Show time in Irish time. Time coming from IoT Hub is always Universal
        /// </summary>
        public DateTime TimeLocal
        {
            get
            {
                return TimeZoneInfo.ConvertTimeFromUtc(Time, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));

            }
        }
        public string TimeLabel
        {
            get
            {
                return TimeLocal.ToString("HH:mm:ss");
            }
        }


        public string TimeLabelShort
        {
            get
            {
                return TimeLocal.ToString("HH:mm");
            }
        }


        public string DateTimeLabel
        {
            get
            {
                return TimeLocal.ToString("dd MMM yyyy HH:mm:ss");
            }
        }

        public string DeviceId { get; set; }

        public string DeviceName { get; set; }

        public double LA10 { get; set; }
        public double LA90 { get; set; }
        public double LAMax { get; set; }

        public double LAEQ { get; set; }

        public string Description { get; set; }

        public string Stand { get; set; }

    }

    public class AggregateSoundDataModel : ReadingBase
    {
        /// <summary>
        /// This is a linear average which shouldn't be used. Use AVGPressure instead
        /// </summary>
        public double AVGleq { get; set; }

        public double MaxLAMax { get; set; }

        /// <summary>
        /// This is the average pressure which is avg(10^LEQ/10)
        /// Stream analytics should calculate this when analysing the strem
        /// /// </summary>
        public double AVGPressureSquared { get; set; }

        public double AVGPressure
        {
            get
            {
                return Math.Log10(AVGPressureSquared) * 10;
            }
        }

    }

    public class DeviceRealTimeSoundDataModel
    {
        public string DeviceId { get; set; }

        public DeviceModel Device
        {
            get
            {
                return DeviceFactory.Instance.Devices.Where(x => x.DeviceId == this.DeviceId).FirstOrDefault();
            }
        }

        public Models.SoundData LatestSoundData { get; set; }

        public Models.SoundData MaximumSoundData { get; set; }
        public Models.SoundData MinimumSoundData { get; set; }
    }

    public class DeviceRollingAverageSoundDataModel
    {
        public string DeviceId { get; set; }

        public DeviceModel Device
        {
            get
            {
                return DeviceFactory.Instance.Devices.Where(x => x.DeviceId == this.DeviceId).FirstOrDefault();
            }
        }

        public List<Models.SoundDataRollingAverageModel> SoundData { get; set; }

    }
}
