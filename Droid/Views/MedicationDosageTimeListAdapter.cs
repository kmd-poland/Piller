using System;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.Views;
using ReactiveUI;
using Piller.Data;
using Android.Content;
using MvvmCross.Binding.Droid.BindingContext;

namespace Piller.Droid.Views
{
    public class MedicationDosageTimeListAdapter : MvxAdapterWithChangedEvent
    {
        private IMvxAndroidBindingContext bindingContext;

        public ReactiveCommand<TimeItem, TimeItem> CLickItem { get;  }
       
        public MedicationDosageTimeListAdapter(Context context) : base(context)
        {
            this.CLickItem = ReactiveCommand.Create<TimeItem, TimeItem>(input => input);
        }
        /*
        public MedicationDosageTimeListAdapter(Context context, IMvxAndroidBindingContext bindingContext) : base(context,bindingContext)
        {
            this.bindingContext = bindingContext;
            this.CLickItem = ReactiveCommand.Create<TimeItem, TimeItem>(input => input);
        }
        */
        protected override IMvxListItemView CreateBindableView(object dataContext, int templateId)
		{
            
            var view = base.CreateBindableView(dataContext, templateId) as MvxListItemView;
			var bset = view.CreateBindingSet<MvxListItemView, TimeItem>();

			var hour = view.FindViewById<TextView>(Resource.Id.hourLabel);
            var name = view.FindViewById<TextView>(Resource.Id.hourName);
          
            bset.Bind(hour)
                .To(x => x.Hour)
                 .WithConversion(new InlineValueConverter<TimeSpan, string>(t => $"{t:hh\\:mm}"));
            bset.Bind(name)
                .To(x => x.Name);
            view.Click += (o, e) => CLickItem.Execute((TimeItem)dataContext).Subscribe();
			bset.Apply();

			return view;
		}
    }
}
