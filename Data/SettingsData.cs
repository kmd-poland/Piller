using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ReactiveUI;
using System.Runtime.Serialization;

namespace Piller.Data
{
	public class SettingsData
	{
		public static string Key { get; } = "hours_settings";
		public IEnumerable<TimeItem> HoursList { get; set; }
		public string RingUri { get; set; } = "content://settings/system/ringtone";
		public int SnoozeMinutes { get; set; } = 50;
		public int WindowHours { get; set; } = 2;

    }
    public class TimeItem : ReactiveObject
    {
        private bool @checked;

        [DataMember]
        public string Name { get; set; }

		[DataMember]
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
        public bool Checked { 
            get { return @checked; } 
            set { this.RaiseAndSetIfChanged(ref @checked, value);  }
        
        }



        public TimeItem(string name)
        {
            this.Name = name;
        }

    }
}
