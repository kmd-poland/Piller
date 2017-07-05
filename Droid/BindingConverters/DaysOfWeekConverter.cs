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
        protected override string Convert(DaysOfWeek value, Type targetType, object parameter, CultureInfo culture)
        {
			if(value.HasFlag(DaysOfWeek.All))
                return Resources.AppResources.EveryDayLabel;

			var days = new[]
			{
				value.HasFlag(DaysOfWeek.Monday),
				value.HasFlag(DaysOfWeek.Tuesday),
				value.HasFlag(DaysOfWeek.Wednesday),
				value.HasFlag(DaysOfWeek.Thursday),
				value.HasFlag(DaysOfWeek.Friday),
				value.HasFlag(DaysOfWeek.Saturday),
				value.HasFlag(DaysOfWeek.Sunday),
			};


			var abbreviatedNames = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
			var selectedDays = abbreviatedNames
					.Skip(1).Concat(abbreviatedNames.Take(1)).ToArray()
					.Zip(days, (day, selected) => selected ? day.TrimEnd('.') : null)
					.Where(x => x != null);

			return $"({String.Join(", ", selectedDays)})";
        }

        protected override DaysOfWeek ConvertBack(string value, Type targetType, object parameter, CultureInfo culture)
        {
            return DaysOfWeek.None; // todo implement ?
        }
    }
}
