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

using Android.Support.Design.Widget;
using ReactiveUI;
using System.Reactive;
using Piller.ViewModels;
using System.Reactive.Linq;

namespace Piller.Droid.Views
{
    class OrdiniationSchemeDialog : BottomSheetDialog
    {
        View bottomSheet;
        ToggleButton monday;
        ToggleButton tuesday;
        ToggleButton wednesday;
        ToggleButton thursday;
        ToggleButton friday;
        ToggleButton saturday;
        ToggleButton sunday;
        Button everyday;

        MedicationDosageViewModel viewModel;

        public OrdiniationSchemeDialog(Context context, MedicationDosageViewModel viewModel) : base(context)
        {
            View secondVIew = LayoutInflater.Inflate(Resource.Layout.bottom_dialog_2, null);
            this.SetContentView(secondVIew);
            this.viewModel = viewModel;

            //this.Create();
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            monday = FindViewById<ToggleButton>(Resource.Id.mondayCheckBox);
            tuesday = FindViewById<ToggleButton>(Resource.Id.tuesdayCheckBox);
            wednesday = FindViewById<ToggleButton>(Resource.Id.wednesdayCheckBox);
            thursday = FindViewById<ToggleButton>(Resource.Id.thursdayCheckBox);
            friday = FindViewById<ToggleButton>(Resource.Id.fridayCheckBox);
            saturday = FindViewById<ToggleButton>(Resource.Id.saturdayCheckBox);
            sunday = FindViewById<ToggleButton>(Resource.Id.sundayCheckBox);
            everyday = FindViewById<Button>(Resource.Id.everyday);

            viewModel.WhenAnyValue(x => x.Monday).ObserveOn(RxApp.MainThreadScheduler).Subscribe(val => monday.Checked = val);
            monday.Events().CheckedChange.Subscribe(args => viewModel.Monday = args.IsChecked);

            viewModel.WhenAnyValue(x => x.Tuesday).ObserveOn(RxApp.MainThreadScheduler).Subscribe(val => tuesday.Checked = val);
            tuesday.Events().CheckedChange.Subscribe(args => viewModel.Tuesday = args.IsChecked);

            viewModel.WhenAnyValue(x => x.Wednesday).ObserveOn(RxApp.MainThreadScheduler).Subscribe(val => wednesday.Checked = val);
            wednesday.Events().CheckedChange.Subscribe(args => viewModel.Wednesday = args.IsChecked);

            viewModel.WhenAnyValue(x => x.Thursday).ObserveOn(RxApp.MainThreadScheduler).Subscribe(val => thursday.Checked = val);
            thursday.Events().CheckedChange.Subscribe(args => viewModel.Thursday = args.IsChecked);

            viewModel.WhenAnyValue(x => x.Friday).ObserveOn(RxApp.MainThreadScheduler).Subscribe(val => friday.Checked = val);
            friday.Events().CheckedChange.Subscribe(args => viewModel.Friday = args.IsChecked);

            viewModel.WhenAnyValue(x => x.Saturday).ObserveOn(RxApp.MainThreadScheduler).Subscribe(val => saturday.Checked = val);
            saturday.Events().CheckedChange.Subscribe(args => viewModel.Saturday = args.IsChecked);

            viewModel.WhenAnyValue(x => x.Sunday).ObserveOn(RxApp.MainThreadScheduler).Subscribe(val => sunday.Checked = val);
            sunday.Events().CheckedChange.Subscribe(args => viewModel.Sunday = args.IsChecked);

            everyday.Events().Click.Select(args=>Unit.Default).InvokeCommand(viewModel.SelectAllDays);

        }

      
    }
}