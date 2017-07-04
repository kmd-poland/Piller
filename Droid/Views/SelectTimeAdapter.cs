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
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.BindingContext;

namespace Piller.Droid.Views
{
    public class SelectTimeAdapter:MvxAdapter
    {
        public SelectTimeAdapter(Context context, IMvxAndroidBindingContext bindingContext) : base(context, bindingContext)
        {

        }
        public SelectTimeAdapter(Context context):base(context)
        {
                
        }
        public SelectTimeAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected override IMvxListItemView CreateBindableView(object dataContext, int templateId)
        {
            var view = base.CreateBindableView(dataContext, templateId) as MvxListItemView;
            var item = view.FindViewById<CheckBox>(Resource.Id.checkBoxItem);

            var bset = view.CreateBindingSet<MvxListItemView, Data.TimeItem>();

            
            bset.Bind(item)
                .For(v => v.Text)
                .To(vm => vm.Label);
            bset.Bind(item)
                .For(v => v.Checked)
                .To(vm => vm.Checked);
            bset.Apply();
            
            return view;
        }
    }
}