using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Platform.Converters;

namespace Piller.Droid.BindingConverters
{
    public class DosageHoursConverter : MvxValueConverter<IEnumerable<TimeSpan>, string>
    {
        public DosageHoursConverter()
        {
        }

        protected override string Convert(IEnumerable<TimeSpan> value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var ddd = value.Select(v => v.ToString(@"h\:mm"));
            return string.Join(", ", ddd.ToArray());
        }

        protected override IEnumerable<TimeSpan> ConvertBack(string value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new List<TimeSpan>(); // todo implement
        }
    }
}
