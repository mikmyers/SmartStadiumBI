using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAA.IoT.Models
{
    public class ReadingBase
    {
        public DateTime Time { get; set; }

        public DateTime TimeLocal
        {
            get
            {
                return TimeZone.CurrentTimeZone.ToUniversalTime( Time).AddHours(1);
            }
        }
        public string TimeLabel
        {
            get
            {
                return Time.ToString("HH:mm:ss");
            }
        }


        public string TimeLabelShort
        {
            get
            {
                return Time.ToString("HH:mm");
            }
        }


        public string DateTimeLabel
        {
            get
            {
                return Time.ToString("dd MMM yyyy HH:mm:ss");
            }
        }

        private string _uniqueId;

        public string UniqueId
        {
            get
            {
                if (string.IsNullOrEmpty(_uniqueId))
                    _uniqueId = Guid.NewGuid().ToString();
                return _uniqueId;
            }
        }

        public string DeviceId { get; set; }

        public string DeviceName { get; set; }
    }
}
