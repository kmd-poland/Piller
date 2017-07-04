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

namespace Piller.Droid.Views
{
    class FirstBottomSheet : MvxBottomSheetDialogFragment<BottomDialogViewModel>
    {

        LinearLayout acceptButton;
        LinearLayout canceltButton;
        MvxListView hoursListView;

        public ReactiveCommand<Unit, IList<Data.TimeItem>> Accept { get; private set; }
        public new ReactiveCommand<Unit, bool> Cancel { get; } = ReactiveCommand.Create(() => { return true; });
        public IEnumerable<Data.TimeItem> list;

        public FirstBottomSheet():base()
        {
           
            Accept = ReactiveCommand.Create(() =>
            {
                IList<Data.TimeItem> checkedHours = new List<Data.TimeItem>();
                foreach (var item in list)
                {

                    if (item.Checked)
                        checkedHours.Add(item);
                }
                return checkedHours;
            });
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.bottom_dialog, null);

            hoursListView = view.FindViewById<MvxListView>(Resource.Id.hoursList);
            hoursListView.ItemsSource = list;
            hoursListView.ItemTemplateId = Resource.Layout.selectTimeItem;
            hoursListView.Adapter = new SelectTimeAdapter(this.Context, (IMvxAndroidBindingContext)this.BindingContext);



            acceptButton = view.FindViewById<LinearLayout>(Resource.Id.okButton);
            acceptButton.Click += (o, e) => Accept.Execute().Subscribe<IList<Data.TimeItem>>();

            canceltButton = view.FindViewById<LinearLayout>(Resource.Id.cancelButton);
            canceltButton.Click += (o, e) => Cancel.Execute().Subscribe();
            return view;
        }

    

        internal void Show(Android.Support.V4.App.FragmentManager fragmentManager,IEnumerable<Data.TimeItem> hoursList)
        {
            list = hoursList;
         //   foreach (var hour in checkedItems)
              //  list.First(h => h.Name == hour.Name).Checked = true;
            //   morningLabel[1]= $"({morningHour:hh\\:mm})";
            // this.morning.Text = string.Join(" ",morningLabel);
            //eveningLabel[1] = $"({eveningHour:hh\\:mm})";
            //this.evening.Text = string.Join(" ", eveningLabel);
            this.Show(fragmentManager,"bottom");
        }
    }
   
}
