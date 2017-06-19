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
        public TimeSpan Morning { get; set; }
        public TimeSpan Afternoon { get; set; }
        public TimeSpan Evening { get; set; }
        public SettingsData()
        {
            Morning = TimeSpan.Parse("09:00:00");
            Afternoon =TimeSpan.Parse("15:00:00");
            Evening = TimeSpan.Parse("21:00:00");
        }
    }
}
