using Cheesebaron.MvxPlugins.Settings.Interfaces;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Piller.Data;
using ReactiveUI;
using System.Reactive;
using Newtonsoft.Json;
using System;
using System.Reactive.Linq;
using MvvmCross.Plugins.Messenger;
using System.Threading.Tasks;
using Piller.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Acr.UserDialogs;
using System.Linq;

namespace Piller.ViewModels
{
    public class SettingsViewModel : MvxViewModel
    {
        private TimeSpan morningHour;
        public TimeSpan MorningHour
        {
            get { return morningHour; }
            set { SetProperty(ref morningHour, value); }
        }
        private TimeSpan afternoonHour;
        public TimeSpan AfternoonHour
        {
            get { return afternoonHour; }
            set { SetProperty(ref afternoonHour, value); }
        }
        private TimeSpan eveningHour;
        public TimeSpan EveningHour
        {
            get { return eveningHour; }
            set { SetProperty(ref eveningHour, value); }
        }

		private string ringUri;
		public string RingUri
		{
			get { return ringUri; }
			set { SetProperty(ref ringUri, value); }
		}

		private int snoozeMinutes;
		public int SnoozeMinutes
		{
			get { return snoozeMinutes; }
			set { SetProperty(ref snoozeMinutes, value); }
		}

		private int windowHours;
		public int WindowHours
		{
			get { return windowHours; }
			set { SetProperty(ref windowHours, value); }
		}

        private ObservableCollection<TimeItem> hoursList;
        public ObservableCollection<TimeItem> HoursList
        {
            get { return hoursList; }
            set { SetProperty(ref hoursList, value); }
        }
        public ReactiveCommand<Unit,Unit> AddHour { get; }
		public ReactiveCommand<String, Unit> SetRingUri { get; }
		public ReactiveCommand<int, Unit> SetSnooze { get; }
		public ReactiveCommand<int, Unit> SetWindow { get; }

        public ReactiveCommand<Unit, bool> Save { get; }
        private SettingsData settingsData;
        private ISettings settings = Mvx.Resolve<ISettings>();
        private readonly string key = SettingsData.Key;
        private const int maxItems=6;

        public SettingsViewModel()
        {
            readSettings();

            if (settingsData.HoursList == null)
            {
                HoursList = new ObservableCollection<TimeItem>()
                {
                    new TimeItem(Resources.AppResources.MorningLabel) {Hour = TimeSpan.Parse("08:00:00") },
                    new TimeItem(Resources.AppResources.EveningLabel){Hour=TimeSpan.Parse("20:00:00")}
                };
            }
            else
                HoursList = new ObservableCollection<TimeItem>(settingsData.HoursList);
           
            var canAdd = this.WhenAnyValue(vm => vm.HoursList.Count, c => c < maxItems);
            AddHour = ReactiveCommand.Create(() =>
            {
                var result = UserDialogs.Instance.Prompt(new PromptConfig()
                    .SetInputMode(InputType.Name)
                    .SetTitle(Resources.AppResources.AddHourMessage)
                    .SetPlaceholder(Resources.AppResources.NewHourPlaceHolder)
                    .SetOkText(Resources.AppResources.SaveText)
                    .SetCancelText(Resources.AppResources.CancelText)     
                    .SetAction(o => {
                    if(o.Ok)
                        {
                            string newName=string.IsNullOrWhiteSpace(o.Text) ? Resources.AppResources.NewHourPlaceHolder : o.Text; 
                            HoursList.Add(new TimeItem(newName));
                        }
                    }));
            }, 
            canAdd);

            RingUri = settingsData.RingUri;
            SetRingUri = ReactiveCommand.Create<String>(uri => RingUri = uri);

			this.WindowHours = settingsData.WindowHours;
			this.SnoozeMinutes = settingsData.SnoozeMinutes;

            this.SetSnooze = ReactiveCommand.Create<int>(value => this.SnoozeMinutes = value);
			this.SetWindow = ReactiveCommand.Create<int>(value => this.WindowHours = value);

            Save = ReactiveCommand.Create(() =>
            {
				var data = JsonConvert.SerializeObject(new SettingsData() {HoursList=this.HoursList, RingUri = this.ringUri, WindowHours = this.windowHours, SnoozeMinutes = this.snoozeMinutes });
                settings.AddOrUpdateValue<string>(key, data);
                return true;
            });
            Save.Subscribe(async x =>
            {
                if (x)
                {
                    await reloadDataBase();
					Mvx.Resolve<IMvxMessenger>().Publish(new SettingsChangeMessage(this,MorningHour,EveningHour,ringUri));
                    Close(this);
                }
            });
        }
        public async Task reloadDataBase()
        {
            IPermanentStorageService storage = Mvx.Resolve<IPermanentStorageService>();
            INotificationService notifications = Mvx.Resolve<INotificationService>();
            var items = await storage.List<MedicationDosage>();
            if (items == null)
                return;
            var medicationList = new List<MedicationDosage>(items);
            foreach(var item in medicationList)
            {
                var dosageHours = new List<TimeSpan>();
                var hours = item.Hours.Split(new string[] { ", " }, StringSplitOptions.None);

                dosageHours = HoursList
                    .Where(h => hours.Contains(h.Name))
                    .Select(i => i.Hour)
                    .ToList();

                if(dosageHours.Count>0)
                {
                    item.DosageHours = dosageHours;
					item.RingUri = ringUri;

                    await storage.SaveAsync<MedicationDosage>(item);
                    await notifications.ScheduleNotifications(item);
                }
            }
        }



        private void readSettings()
        {
            var data = settings.GetValue<string>(key);
            if (String.IsNullOrEmpty(data))
            {
                settingsData = new SettingsData();
                return;
            }
            try
            {
                settingsData = JsonConvert.DeserializeObject<SettingsData>(data);
            }
            catch (JsonException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                settingsData = new SettingsData();
            }

        }
    }
}