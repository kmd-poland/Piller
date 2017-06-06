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
using ReactiveUI;
using System.Reactive;

namespace Piller.Droid.Views
{
    class DeleteDialog: Android.Support.Design.Widget.BottomSheetDialog
    {
        LinearLayout acceptButton;
        LinearLayout canceltButton;
        public new ReactiveCommand<Unit, bool> Accept { get; private set; }= ReactiveCommand.Create(() => { return true; });
        public new ReactiveCommand<Unit, bool> Cancel { get; } = ReactiveCommand.Create(() => { return true; });
        public DeleteDialog(Context context) : base(context)
        {
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            acceptButton = FindViewById<LinearLayout>(Resource.Id.okButton);
            acceptButton.Click += (o, e) => Accept.Execute().Subscribe();

            canceltButton = FindViewById<LinearLayout>(Resource.Id.cancelButton);
            canceltButton.Click += (o, e) => Cancel.Execute().Subscribe();
        }

    }
}