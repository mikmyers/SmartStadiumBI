using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class Extensions
    {
        public static string GetText( this Exception ex, bool includeStack )
        {
            StringBuilder b = new StringBuilder();
            while( ex != null)
                {
                b.Append(ex.Message);
                if (includeStack)
                    b.Append(ex.StackTrace);
                ex = ex.InnerException;
            }
            return b.ToString();
        }

        public static int ToEpoch(this DateTime date)
        {
            if (date == null) return int.MinValue;
            DateTime epoch = new DateTime(1970, 1, 1);
            TimeSpan epochTimeSpan = date - epoch;
            return (int)epochTimeSpan.TotalSeconds;
        }
    }
}
