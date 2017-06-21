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

namespace Piller.Droid.Views
{
    class FirstBottomSheet : BottomSheetDialog
    {
        CheckBox morning;
        CheckBox evening;
        LinearLayout acceptButton;
        LinearLayout canceltButton;
        View firstView;
        string[] morningLabel = new string[2];
        string[] eveningLabel = new string[2];
        public FirstBottomSheet(Context context) : base(context)
        {
            firstView = LayoutInflater.Inflate(Resource.Layout.bottom_dialog, null);
            SetContentView(firstView);
            this.Create();
        }


        public ReactiveCommand<Unit, HoursPattern> Accept { get; private set; }
        public new ReactiveCommand<Unit, bool> Cancel { get; } = ReactiveCommand<Unit, bool>.Create(() => { return true; });
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            morning = FindViewById<CheckBox>(Resource.Id.morning);
            evening = FindViewById<CheckBox>(Resource.Id.evening);

            morningLabel[0] = morning.Text;
            eveningLabel[0] = evening.Text;


            Accept = ReactiveCommand.Create(() =>
            {
                return new HoursPattern() { Morning = morning.Checked, Evening = evening.Checked };
            }
         );
            acceptButton = FindViewById<LinearLayout>(Resource.Id.okButton);
            acceptButton.Click += (o, e) => Accept.Execute().Subscribe<HoursPattern>();

            canceltButton = FindViewById<LinearLayout>(Resource.Id.cancelButton);
            canceltButton.Click += (o, e) => Cancel.Execute().Subscribe();
        }

        internal void Show(TimeSpan morningHour, TimeSpan eveningHour)
        {
            morningLabel[1]= $"({morningHour:hh\\:mm})";
            this.morning.Text = string.Join(" ",morningLabel);
            eveningLabel[1] = $"({eveningHour:hh\\:mm})";
            this.evening.Text = string.Join(" ", eveningLabel);
            this.Show();
        }
    }
}