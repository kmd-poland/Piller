using System;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.Views;
using ReactiveUI;
using Piller.Data;
using Android.Content;
using MvvmCross.Binding.Droid.BindingContext;
using Android.Views;

namespace Piller.Droid.Views
{
    public class MedicationDosageTimeListAdapter : MvxAdapterWithChangedEvent
    {
        private IMvxAndroidBindingContext bindingContext;

        public ReactiveCommand<TimeItem, TimeItem> CLickItem { get;  }
        public ReactiveCommand<TimeItem, TimeItem> DeleteRequested { get; }

        public MedicationDosageTimeListAdapter(Context context) : base(context)
        {
            this.CLickItem = ReactiveCommand.Create<TimeItem, TimeItem>(input => input);
            this.DeleteRequested = ReactiveCommand.Create<TimeItem, TimeItem>(input => input);
        }
        
        
        protected override IMvxListItemView CreateBindableView(object dataContext, int templateId)
		{
            
            var view = base.CreateBindableView(dataContext, templateId) as MvxListItemView;
			var bset = view.CreateBindingSet<MvxListItemView, TimeItem>();

			var hour = view.FindViewById<TextView>(Resource.Id.hourLabel);
            var name = view.FindViewById<TextView>(Resource.Id.hourName);

            var deleteButton = view.FindViewById<ImageView>(Resource.Id.button_delete_dosage_hour);
            deleteButton.Click += (sender, e) => DeleteRequested.Execute((TimeItem)dataContext).Subscribe();

            bset.Bind(hour)
                .To(x => x.Hour)
                 .WithConversion(new InlineValueConverter<TimeSpan, string>(t => $"{t:hh\\:mm}"));
            bset.Bind(name)
                .To(x => x.Name);
            bset.Bind(deleteButton)
                .For(v=>v.Visibility)
                .To(x=>x.Name)
                .WithConversion(new InlineValueConverter<string, ViewStates>(n => n.Equals(Resources.AppResources.MorningLabel) || n.Equals(Resources.AppResources.EveningLabel) ? ViewStates.Gone : ViewStates.Visible));
			bset.Apply();

            view.Click += (o, e) => CLickItem.Execute((TimeItem)dataContext).Subscribe();

			return view;
		}
    }
}
