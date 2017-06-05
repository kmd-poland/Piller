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
    class FirstBottomSheet:BottomSheetDialog
    {
        RadioButton customOption;
        RadioButton firstOption;
        View firstView;
        public FirstBottomSheet(Context context) : base(context)
        {
        }
        public new ReactiveCommand<Unit, bool> FirstOption { get; } = ReactiveCommand.Create(() => { return true; });
        public new ReactiveCommand<Unit, bool> SetCustom { get; } = ReactiveCommand.Create(() => { return true; });
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            firstView = LayoutInflater.Inflate(Resource.Layout.bottom_dialog, null);
            customOption = FindViewById<RadioButton>(Resource.Id.option3);
            firstOption = FindViewById<RadioButton>(Resource.Id.option1);

            customOption.Click += (o, e) => SetCustom.Execute().Subscribe();
            firstOption.Click += (o, e) => FirstOption.Execute().Subscribe();
        }

    }
}