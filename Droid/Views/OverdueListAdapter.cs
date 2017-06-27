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
        public ReactiveCommand<OverdueNotification, OverdueNotification> DeleteRequested { get; }

        public OverdueListAdapter(Context context) : base(context)
        {
            
        }

        public OverdueListAdapter(Context context, IMvxAndroidBindingContext bindingContext) : base(context, bindingContext)
        {
            this.DeleteRequested = ReactiveCommand.Create<OverdueNotification, OverdueNotification>(input => input);
        }

        public OverdueListAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected override IMvxListItemView CreateBindableView(object dataContext, int templateId)
        {
            var view = base.CreateBindableView(dataContext, templateId) as MvxListItemView;
            var bset = view.CreateBindingSet<MvxListItemView, OverdueNotification>();
            var name = view.FindViewById<TextView>(Resource.Id.label_overdue_name);
            var del_not_button = view.FindViewById<ImageView>(Resource.Id.del_not_button);

            del_not_button.Click += (sender, e) => DeleteRequested.Execute((OverdueNotification)dataContext).Subscribe();

            bset.Bind(name)
                .To(x => x.Name);


            bset.Apply();
            return view;
        }
  
    }
}
