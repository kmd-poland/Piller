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
using Piller.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Binding.Droid.BindingContext;
using Piller.Data;

namespace Piller.Droid.Views
{
    [Activity]
    public class SettingsView : MvxAppCompatActivity<SettingsViewModel>
    {
        RelativeLayout morning;
        RelativeLayout afternoon;
        RelativeLayout evening;
        TextView morningHour;
        TextView afternoonHour;
        TextView eveningHour;
        MedicationDosageTimeLayout HoursList;
        Button addHour;
        TimeItem newItem;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Settings);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
            SupportActionBar.Title = "Ustawienia";

            HoursList = FindViewById<MedicationDosageTimeLayout>(Resource.Id.hoursList);
            HoursList.ItemTemplateId = Resource.Layout.time_item;

            addHour = FindViewById<Button>(Resource.Id.addHourBtn);
            var hoursAdapter = (MedicationDosageTimeListAdapter) HoursList.Adapter;
            hoursAdapter.CLickItem.Subscribe(item =>
            {
                TimePickerDialog timePicker = new TimePickerDialog(
                    this,
                    (s, args) =>
                    {
                        if (((TimePicker)s).IsShown)
                        {
                            newItem = new TimeItem(item.Name);
                            newItem.Hour= new TimeSpan(args.HourOfDay, args.Minute, 0);
                            var id = this.ViewModel.HoursList.IndexOf(item);
                            if(id>=0)
                            {
                                this.ViewModel.HoursList.RemoveAt(id);
                                this.ViewModel.HoursList.Insert(id,newItem);
                            }
                        }
                    },
                     12,
                     00,
                     true);
                timePicker.Show();
            });

            hoursAdapter.DeleteRequested.Subscribe(time => this.ViewModel.HoursList.Remove(time));
            SetBinding();
        }

        private void SetBinding()
        {
            var bindingSet = this.CreateBindingSet<SettingsView, SettingsViewModel>();
            bindingSet.Bind(morningHour)
                .For(v => v.Text)
                .To(vm => vm.MorningHour)
                .WithConversion(new InlineValueConverter<TimeSpan, string>(t => $"{t:hh\\:mm}"));
            bindingSet.Bind(afternoonHour)
               .To(vm => vm.AfternoonHour)
                .WithConversion(new InlineValueConverter<TimeSpan, string>(t => $"{t:hh\\:mm}"));
            bindingSet.Bind(eveningHour)
               .To(vm => vm.EveningHour)
                .WithConversion(new InlineValueConverter<TimeSpan, string>(t => $"{t:hh\\:mm}"));
            bindingSet.Bind(HoursList)
                .For(v => v.ItemsSource)
                .To(vm => vm.HoursList);
            bindingSet.Bind(addHour)
                .For(nameof(View.Click))
                .To(vm => vm.AddHour);
            bindingSet.Apply();

        }

        public override bool OnSupportNavigateUp()
        {
            OnBackPressed();
            return true;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.settings_menu,menu);
            return base.OnCreateOptionsMenu(menu);
        }
        
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int goHomeId = 16908332;
            if (item.ItemId == Resource.Id.action_save)
                ViewModel.Save.Execute().Subscribe();
            else if (item.ItemId == goHomeId)              
                OnBackPressed();
            return true;
        }
        


    }
}