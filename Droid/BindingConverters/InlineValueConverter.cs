using System;
using MvvmCross.Platform.Converters;

namespace Piller.Droid.BindingConverters
{
    public class InlineValueConverter<TIn, TOut> : MvxValueConverter<TIn, TOut>
    {
        private Func<TIn, TOut> converter;

        public InlineValueConverter(Func<TIn, TOut> convertFunction)
        {
            this.converter = convertFunction;
        }

        protected override TOut Convert(TIn value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.converter(value);
        }
    }
}
