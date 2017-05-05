using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MvvmCross.Platform.Converters;
using Piller.Data;

namespace Piller.Droid.BindingConverters
{
    public class DaysOfWeekConverter : MvxValueConverter<DaysOfWeek, string>
    {
		private readonly string[] days = new string[] { "", "pn", "wt", "śr", "czw", "pt", "so", "nd" };

		public DaysOfWeekConverter()
        {
        }

        protected override string Convert(DaysOfWeek value, Type targetType, object parameter, CultureInfo culture)
        {
            var daysNumbers = new DaysOfWeek[] {
                value & DaysOfWeek.Monday,
                value & DaysOfWeek.Tuesday,
				value & DaysOfWeek.Wednesday,
				value & DaysOfWeek.Thursday,
				value & DaysOfWeek.Friday,
                value & DaysOfWeek.Saturday,
                value & DaysOfWeek.Sunday,
                value & DaysOfWeek.None
            };

            var result = new List<string>();
            foreach (var dayNumber in daysNumbers.Where(x => x != DaysOfWeek.None))
            {
                var index = Array.IndexOf(Enum.GetValues(typeof(DaysOfWeek)), dayNumber);
                result.Add(days[index]);
            }

            return $"({string.Join(", ", result.ToArray())})";
        }

        protected override DaysOfWeek ConvertBack(string value, Type targetType, object parameter, CultureInfo culture)
        {
            return DaysOfWeek.None; // todo implement ?
        }
    }
}
