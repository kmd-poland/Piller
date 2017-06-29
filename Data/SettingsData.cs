using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piller.Data
{
    public class SettingsData
    {
        public static string Key { get; } = "hours_settings";
        public IEnumerable<TimeItem> HoursList { get; set; }
    }
    public class TimeItem
    {
        public string Name { get; set; }
        private TimeSpan hour;
        public TimeSpan Hour
        {
            get { return hour; }
            set { hour = value; }
        }
        public TimeItem(string name)
        {
            this.Name = name;
        }

    }
}
