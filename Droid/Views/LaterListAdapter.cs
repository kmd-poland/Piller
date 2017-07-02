using System;
using Android.Runtime;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.Views;
using Piller.Data;
using MvvmCross.Binding.Droid.BindingContext;
using Android.Content;
using Piller.Droid.BindingConverters;
using Android.Views;
using System.Collections.Generic;
using System.Linq;
using Android.Graphics;
using MvvmCross.Plugins.File;
using Services;
using MvvmCross.Platform;
using ReactiveUI;

namespace Piller.Droid.Views
{
    public class LaterListAdapter : MvxAdapter
    {
        public LaterListAdapter(Context context) : base(context)
        {
        }

        public LaterListAdapter(Context context, IMvxAndroidBindingContext bindingContext) : base(context, bindingContext)
        {
            
        }

        public LaterListAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected override IMvxListItemView CreateBindableView(object dataContext, int templateId)
        {
            var view = base.CreateBindableView(dataContext, templateId) as MvxListItemView;
            var name = view.FindViewById<TextView>(Resource.Id.label_later_name);
            var dosage = view.FindViewById<TextView>(Resource.Id.label_later_dosage);
            var time = view.FindViewById<TextView>(Resource.Id.label_later_time);
            var bset = view.CreateBindingSet<MvxListItemView, NotificationOccurrence>();


            bset.Bind(name)
                .To(x => x.Name);

            bset.Bind(dosage)
                .To(x => x.Dosage);

            bset.Bind(time)
                .To(x => x.OccurrenceDateTime);
                .WithConversion(new LaterHoursConverter());

            bset.Apply();
            return view;
        }
    }
}
