using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MvvmCross.Platform.Converters;
using Piller.Data;

namespace Piller.Droid.BindingConverters
{
    public class LaterHoursConverter : MvxValueConverter<DateTime, string>
    {
        public LaterHoursConverter()
        {
        }

        protected override string Convert(DateTime value, Type targetType, object parameter, CultureInfo culture)
        {
            var now = DateTime.Now;
            var result = (value - now).Hours.ToString();

            return $"{"za " + result + " h"}";
        }

    }
}
