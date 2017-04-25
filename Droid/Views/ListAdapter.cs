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


namespace Piller.Droid.Views
{

    class ListAdapter:MvxAdapter
    {
        public ListAdapter(Android.Content.Context context) : base(context)
        {
        }

        public ListAdapter(Android.Content.Context context, MvvmCross.Binding.Droid.BindingContext.IMvxAndroidBindingContext bindingContext) : base(context, bindingContext)
        {
        }

        public ListAdapter(IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected override IMvxListItemView CreateBindableView(object dataContext, int templateId)
        {
            var view = base.CreateBindableView(dataContext, templateId) as MvxListItemView;
            var bset = view.CreateBindingSet<MvxListItemView, Piller.Data.MedicationDosage>();

            var name = view.FindViewById<TextView>(Resource.Id.label_medication_name);

            bset.Bind(name)
                .To(x => x.Name);
            bset.Apply();
            return view;
        }
    }
}