using MvvmCross.Platform.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Piller.ViewModels
{
    public class TimeSpanToStringConverter:MvxValueConverter<TimeSpan,string>
    {
        protected override string Convert(TimeSpan value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"{value.Hours}:{value.Minutes}";
        }
    }
}
