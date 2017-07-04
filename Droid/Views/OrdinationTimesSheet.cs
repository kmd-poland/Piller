using System;
using System.Collections.Generic;
using Android.OS;
using Android.Views;
using Android.Widget;
using ReactiveUI;
using System.Reactive;
using Piller.ViewModels;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.Design;
using System.Linq;
using Android.Support.Design.Widget;
using Android.Content;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Views;

namespace Piller.Droid.Views
{
    class OrdinationTimesSheet : BottomSheetDialog
    {

        LinearLayout acceptButton;
        LinearLayout canceltButton;
        ListView hoursListView;
        MedicationDosageViewModel viewModel;
     

        public OrdinationTimesSheet(Context context, MedicationDosageViewModel viewModel) : base(context)
        {
            this.viewModel = viewModel;

			var view = LayoutInflater.Inflate(Resource.Layout.bottom_dialog, null);
            this.SetContentView(view);
    
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            hoursListView = FindViewById<ListView>(Resource.Id.hoursList);

            var adapter = new SelectTimeAdapter(this.Context, viewModel.TimeItems);
            hoursListView.Adapter = adapter;
	
        }
    
    }
   
}