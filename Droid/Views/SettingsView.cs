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
using Android.Media;

namespace Piller.Droid.Views
{
    [Activity]
    public class SettingsView : MvxAppCompatActivity<SettingsViewModel>
    {
        MedicationDosageTimeLayout HoursList;
        TextView addHour, soundLabel;
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

            addHour = FindViewById<TextView>(Resource.Id.addHourBtn);
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

			soundLabel = FindViewById<TextView>(Resource.Id.soundLabel);
            soundLabel.Click += (o, e) =>
            {
                Intent intent = new Intent(RingtoneManager.ActionRingtonePicker);
				intent.PutExtra(RingtoneManager.ExtraRingtoneTitle, true);
                intent.PutExtra(RingtoneManager.ExtraRingtoneShowSilent, false);
                intent.PutExtra(RingtoneManager.ExtraRingtoneShowDefault, true);

				Android.Net.Uri uri;
				if (!String.IsNullOrEmpty(this.ViewModel.RingUri))
					uri = Android.Net.Uri.Parse(this.ViewModel.RingUri);
				else
					uri = RingtoneManager.GetDefaultUri(RingtoneType.Alarm);
				
                intent.PutExtra(RingtoneManager.ExtraRingtoneExistingUri, uri);

				StartActivityForResult(Intent.CreateChooser(intent, "Wybierz dzwonek"), 0);

                //Android.Net.Uri ring = (Android.Net.Uri)intent.GetParcelableExtra(RingtoneManager.ExtraRingtonePickedUri);
            };



            SetBinding();
        }

        private void SetBinding()
        {
            var bindingSet = this.CreateBindingSet<SettingsView, SettingsViewModel>();          
            bindingSet.Bind(HoursList)
                .For(v => v.ItemsSource)
                .To(vm => vm.HoursList);
			
            bindingSet.Bind(addHour)
                .For(nameof(View.Click))
                .To(vm => vm.AddHour);

			bindingSet.Bind(soundLabel)
					  .For(v => v.Text)
					  .To(vm => vm.RingUri)
			          .WithConversion(new InlineValueConverter<string, string>((arg) => 
						{
							var uri = Android.Net.Uri.Parse(arg);
							Ringtone ringtone = RingtoneManager.GetRingtone(this, uri);
							return RingtoneManager.IsDefault(uri) ? "Default" : ringtone.GetTitle(this);
						}));

            bindingSet.Apply();
        }

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			if (resultCode == Result.Ok)
			{
				Android.Net.Uri ring = (Android.Net.Uri)data.GetParcelableExtra(RingtoneManager.ExtraRingtonePickedUri);
            	this.ViewModel.SetRingUri.Execute(ring.ToString()).Subscribe();
			}
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