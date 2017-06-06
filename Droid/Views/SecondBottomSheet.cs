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

namespace Piller.Droid.Views
{
    class SecondBottomSheet : BottomSheetDialog
    {
        View bottomSheet;
        CheckBox monday;
        CheckBox tuesday;
        CheckBox wednesday;
        CheckBox thursday;
        CheckBox friday;
        CheckBox saturday;
        CheckBox sunday;
        LinearLayout acceptButton;
        LinearLayout canceltButton;

        public ReactiveCommand<Unit, bool[]>Accept { get; private set; }
        public new ReactiveCommand<Unit, bool> Cancel { get; } = ReactiveCommand<Unit, bool>.Create(() => { return true; });

        public SecondBottomSheet(Context context) : base(context)
        {
            
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetCancelable(false);


            monday = FindViewById<CheckBox>(Resource.Id.mondayCheckBox);
            tuesday = FindViewById<CheckBox>(Resource.Id.tuesdayCheckBox);
            wednesday = FindViewById<CheckBox>(Resource.Id.wednesdayCheckBox);
            thursday = FindViewById<CheckBox>(Resource.Id.thursdayCheckBox);
            friday = FindViewById<CheckBox>(Resource.Id.fridayCheckBox);
            saturday = FindViewById<CheckBox>(Resource.Id.saturdayCheckBox);
            sunday = FindViewById<CheckBox>(Resource.Id.sundayCheckBox);
            Accept = ReactiveCommand.Create<Unit, bool[]>(x =>
            {
                return GetSelectedDays();
            }
           );
            acceptButton = FindViewById<LinearLayout>(Resource.Id.okButton);
            acceptButton.Click += (o, e) => Accept.Execute().Subscribe();

            canceltButton = FindViewById<LinearLayout>(Resource.Id.cancelButton);
            canceltButton.Click += (o,e)=>Cancel.Execute().Subscribe();
        }

        private bool[] GetSelectedDays()
        {
            return new bool[] { monday.Checked, tuesday.Checked, wednesday.Checked, thursday.Checked, friday.Checked, saturday.Checked, sunday.Checked };
        }

    }
}