using System;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.Views;
using ReactiveUI;
namespace Piller.Droid.Views
{
    public class MedicationDosageTimeListAdapter : MvxAdapterWithChangedEvent
    {

        public ReactiveCommand<TimeSpan, TimeSpan> DeleteRequested { get;  }
       
        public MedicationDosageTimeListAdapter(Android.Content.Context context) : base(context)
        {
            this.DeleteRequested = ReactiveCommand.Create<TimeSpan, TimeSpan>(input => input);
        }

     

		protected override IMvxListItemView CreateBindableView(object dataContext, int templateId)
		{
            
            var view = base.CreateBindableView(dataContext, templateId) as MvxListItemView;
			var bset = view.CreateBindingSet<MvxListItemView, TimeSpan>();

			var hour = view.FindViewById<TextView>(Resource.Id.label_medication_hour);
            var deleteButton = view.FindViewById<Button>(Resource.Id.button_delete_dosage_hour);
            bset.Bind(hour)
                .To(x => x);

            deleteButton.Click += (sender, e) => DeleteRequested.Execute((TimeSpan)dataContext).Subscribe();
			bset.Apply();

			return view;
		}
    }
}
