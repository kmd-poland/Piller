using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Binding.BindingContext;
using Piller.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using System.Collections.Specialized;

namespace Piller.Droid.Views
{

    class HoursListAdapter : MvxAdapter, IMvxAdapterWithChangedEvent
    {
        public HoursListAdapter(Context context) : base(context)
        {
        }

        public HoursListAdapter(Context context, IMvxAndroidBindingContext bindingContext) : base(context, bindingContext)
        {
        }

        protected HoursListAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public event EventHandler<NotifyCollectionChangedEventArgs> DataSetChanged;

        protected override IMvxListItemView CreateBindableView(object dataContext, int templateId)
        {
            var view = base.CreateBindableView(dataContext, templateId) as MvxListItemView;
            var bset = view.CreateBindingSet<MvxListItemView, TimeSpan>();

            var hour = view.FindViewById<TextView>(Resource.Id.label_medication_hour);

            bset.Bind(hour)
                .To(x => x)
                .WithConversion("TimeSpanToStringConverter");
            bset.Apply();
            return view;
        }
    }
}