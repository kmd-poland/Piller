using Acr.UserDialogs.Fragments;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.Design;
using ReactiveUI;
using System.Reactive;
using System;
using Piller.ViewModels;

namespace Piller.Droid.Views
{
    public class BottomSheet : MvxBottomSheetDialogFragment
    {
        RadioButton everyday;
        RadioButton custom;
        private bool selectedEveryday;
        public ReactiveCommand<bool, bool> ChoseEveryday { get; }
        public ReactiveCommand<Unit,bool> ChoseCustom { get; }
        public BottomSheet() : base()
        {
            ChoseEveryday = ReactiveCommand.Create<bool, bool>(o => { return o; });
            ChoseCustom = ReactiveCommand.Create(() => { return true; });
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.dialog_days, null);
            everyday = view.FindViewById<RadioButton>(Resource.Id.everyday);
            custom = view.FindViewById<RadioButton>(Resource.Id.custom);

            everyday.Checked = this.selectedEveryday;
            custom.Checked = !this.selectedEveryday;

             everyday.Click += (o, e) => ChoseEveryday.Execute(true).Subscribe();
            custom.Click += (o, e) => ChoseCustom.Execute().Subscribe();

            return view;
        }

        public void Show(FragmentManager manager, bool selectedEveryday)
        {
            this.selectedEveryday = selectedEveryday;

            // custom.Checked = !selectedEveryday;
            this.Show(manager, "daysbottom");
        }
    }
}