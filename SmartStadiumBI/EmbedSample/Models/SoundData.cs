using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAA.IoT.Web.Models
{
    public partial class SoundData
    {

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
    }
}
