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
    public class OverdueListAdapter : MvxAdapter
    {
        public ReactiveCommand<NotificationOccurrence, NotificationOccurrence> DeleteRequested { get; }

        public OverdueListAdapter(Context context) : base(context)
        {
            
        }

        public OverdueListAdapter(Context context, IMvxAndroidBindingContext bindingContext) : base(context, bindingContext)
        {
            this.DeleteRequested = ReactiveCommand.Create<NotificationOccurrence, NotificationOccurrence>(input => input);
        }

        public OverdueListAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected override IMvxListItemView CreateBindableView(object dataContext, int templateId)
        {
            var view = base.CreateBindableView(dataContext, templateId) as MvxListItemView;
            var bset = view.CreateBindingSet<MvxListItemView, NotificationOccurrence>();
            var name = view.FindViewById<TextView>(Resource.Id.label_overdue_name);
            var dosage = view.FindViewById<TextView>(Resource.Id.label_overdue_dosage);
            var time = view.FindViewById<TextView>(Resource.Id.label_overdue_time);
            var del_not_button = view.FindViewById<ImageView>(Resource.Id.del_not_button);

            del_not_button.Click += (sender, e) => DeleteRequested.Execute((NotificationOccurrence)dataContext).Subscribe();

            bset.Bind(name)
                .To(x => x.Name);

            bset.Bind(dosage)
                .To(x => x.Dosage);

            bset.Bind(time)
                .To(x => x.OccurrenceDateTime);

            bset.Apply();
            return view;
        }
  
    }
}
