using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
        public TimeSpan Hour { get; set; }
        [JsonIgnore]
        public string Label
        {
            get
            {
                return $"{Name} ({Hour:hh\\:mm})";
            }
        } 
        [JsonIgnore]
        public bool Checked { get; set; }
        public TimeItem(string name)
        {
            this.Name = name;
        }

    }
}
