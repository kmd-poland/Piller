using System;
using MvvmCross.Core.ViewModels;
using RxUI = ReactiveUI;
using System.Reactive;
using Piller.Data;
using Piller.Services;
using MvvmCross.Platform;
using Acr.UserDialogs;
using Piller.Resources;
using ReactiveUI;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Plugins.PictureChooser;
using System.IO;
using Services;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace Piller.ViewModels
{
    public class MedicationDosageViewModel : MvxViewModel
    {
		IMvxPictureChooserTask PictureChooser = Mvx.Resolve<IMvxPictureChooserTask>();
		private IPermanentStorageService storage = Mvx.Resolve<IPermanentStorageService>();
		private readonly ImageLoaderService imageLoader = Mvx.Resolve<ImageLoaderService>();
        private readonly INotificationService notifications = Mvx.Resolve<INotificationService>();
        private ISettings settings = Mvx.Resolve<ISettings>();
		private IMedicineDatabaseService medicinesDatabase = Mvx.Resolve<IMedicineDatabaseService>();

        public ReactiveCommand<Unit, Stream> TakePhotoCommand { get; set; }

		private byte[] _bytes;
		public byte[] Bytes
		{
			get { return _bytes; }
			set { _bytes = value; RaisePropertyChanged(() => Bytes); }
		}

		private async Task OnPicture(Stream pictureStream)
		{
            
			var memoryStream = new MemoryStream();
            await pictureStream.CopyToAsync(memoryStream);
			Bytes = memoryStream.ToArray();
		}

        //identyfikator rekordu, uzywany w trybie edycji
        private int? id;
        public int? Id { 
            get { return this.id; }
            set { this.SetProperty(ref this.id, value); }
        }

        string medicationName;
        public string MedicationName
        {
			get { return medicationName; }
            set { this.SetProperty(ref medicationName, value); }
        }

		string startDate;
		public string StartDate
		{
			get { return startDate; }
			set { this.SetProperty(ref startDate, value); }
		}

		string endDate;
		public string EndDate
		{
			get { return endDate; }
			set	{ this.SetProperty(ref endDate, value); }
		}

        public async void SetMedicinesName(string kodEAN)
        {
            Data.Medicines medicine =  await this.medicinesDatabase.GetAsync(kodEAN);

            if(medicine != null)
            {
                MedicationName = $"{medicine.NazwaProduktu} ({medicine.Moc})" ;
            }
            else
            {
                MedicationName = "";
                UserDialogs.Instance.Toast("Nie znaleziono w bazie leków");
            }
        }
       

        long ean;
        public long EAN
        {
            get { return ean; }
            set {
                this.SetProperty(ref ean, value);
            }
        }

        string medicationDosage;
        public string MedicationDosage
        {
            get { return medicationDosage; }
            set { this.SetProperty(ref medicationDosage, value); }
        }


        private bool monday;
        public bool Monday
        {
            get { return monday; }
            set { this.SetProperty(ref monday, value);  }
        }

        private bool tuesday;
        public bool Tuesday
        {
            get { return tuesday; }
            set { this.SetProperty(ref tuesday, value); }
        }

        private bool wednesday;
        public bool Wednesday
        {
            get { return wednesday; }
            set { this.SetProperty(ref wednesday, value); }
        }

        private bool thursday;
        public bool Thursday
        {
            get { return thursday; }
            set { this.SetProperty(ref thursday, value); }
        }

        private bool friday;
        public bool Friday
        {
            get { return friday; }
            set { this.SetProperty(ref friday, value);  }
        }

        private bool saturday;
        public bool Saturday
        {
            get { return saturday; }
            set { this.SetProperty(ref saturday, value); }
        }

        private bool sunday;
        public bool Sunday
        {
            get { return sunday; }
            set { this.SetProperty(ref sunday, value); RaisePropertyChanged(nameof(Everyday)); RaisePropertyChanged(nameof(DaysLabel)); }
        }
        private bool everyday;
        public bool Everyday
        {
            get { return everyday || (Monday && Tuesday && Wednesday && Thursday && Friday && Saturday && Sunday); }
            set {
                isNew = false;
                if(value)
                    SelectAllDays.Execute().Subscribe();
                SetProperty(ref everyday, value);
            }
        }
        private bool custom;
        public bool Custom
        {
            get { return custom || (!Everyday&&!isNew); }
            set
            {
                isNew = false;
                SetProperty(ref custom, value);
            }
        }

		public string RingUri { get; private set; }

        private bool isNew;
        public List<TimeItem> CheckedHours { get; private set; } = new List<TimeItem>();

        private RxUI.ReactiveList<TimeSpan> dosageHours;
        public ReactiveList<TimeSpan> DosageHours
        {
            get { return this.dosageHours; }
            set { SetProperty(ref dosageHours, value); }
        }
        private string hoursLabel;
        public string HoursLabel
        {
            get {return hoursLabel; }
            private set { SetProperty(ref hoursLabel, value); }
        }

        public string DaysLabel
        {
            get
            {
                if (Everyday)
                    return AppResources.EveryDayLabel;
                return AppResources.CustomDayLabel;
            }
        }

        private ReactiveList<TimeItem> timeItems;
        public ReactiveList<TimeItem> TimeItems
        {
            get { return timeItems; }
            set { SetProperty(ref timeItems, value); }
        }

        private void selectAllDays()
        {
            Monday = true;
            Tuesday = true;
            Wednesday = true;
            Thursday = true;
            Friday = true;
            Saturday = true;
            Sunday = true;
           
        }

        public ReactiveCommand<Unit, bool> Save { get; private set; }
        public ReactiveCommand<MedicationDosage, bool> Delete { get; set; }    
        public ReactiveCommand<Unit, Unit> SelectAllDays { get; set; }
        public ReactiveCommand<Unit, bool> GoSettings { get; }
        public ReactiveCommand<IList<TimeItem>, Unit> SetRepeatTime { get; }
        public ReactiveCommand<Unit, bool> ShowDialog { get; }

        public MedicationDosageViewModel()
        {
        ShowDialog = ReactiveCommand.Create(() => this.ShowViewModel<BottomDialogViewModel>());
            this.DosageHours = new ReactiveList<TimeSpan>();
            var canSave = this.WhenAny(
				vm => vm.MedicationName,
				vm => vm.MedicationDosage,
				vm => vm.Monday,
				vm => vm.Tuesday,
				vm => vm.Wednesday,
				vm => vm.Thursday,
				vm => vm.Friday,
				vm => vm.Saturday,
				vm => vm.Sunday,
				vm => vm.DosageHours.Count,
				(n, d, m, t, w, th, f, sa, su, h) =>

				!String.IsNullOrWhiteSpace(n.Value) &&
				!String.IsNullOrWhiteSpace(d.Value) &&
				(m.Value | t.Value | w.Value | th.Value | f.Value | sa.Value | su.Value) &&
				h.Value > 0);
            
			this.TakePhotoCommand = ReactiveCommand.CreateFromTask(() => PictureChooser.TakePicture(1920, 75));
			this.TakePhotoCommand
                .Where(x=>x!=null)
                .Select(x =>this.OnPicture(x))
                .Subscribe();
			

            this.Save = RxUI.ReactiveCommand.CreateFromTask<Unit, bool>(async _ =>
			{

				var dataRecord = new MedicationDosage
				{
					Id = this.Id,
					Name = this.MedicationName,
					From = this.StartDate,
					To = this.EndDate,
					Dosage = this.MedicationDosage,

					Days =
						(this.Monday ? DaysOfWeek.Monday : DaysOfWeek.None)
						| (this.Tuesday ? DaysOfWeek.Tuesday : DaysOfWeek.None)
						| (this.Wednesday ? DaysOfWeek.Wednesday : DaysOfWeek.None)
						| (this.Thursday ? DaysOfWeek.Thursday : DaysOfWeek.None)
						| (this.Friday ? DaysOfWeek.Friday : DaysOfWeek.None)
						| (this.Saturday ? DaysOfWeek.Saturday : DaysOfWeek.None)
						| (this.Sunday ? DaysOfWeek.Sunday : DaysOfWeek.None),
					DosageHours = this.DosageHours,
					Hours = this.HoursLabel,
					RingUri = this.RingUri
                    
                };

				if (!string.IsNullOrEmpty(this.StartDate) && !string.IsNullOrEmpty(this.EndDate))
				{
					DateTime start = DateTime.Parse(this.StartDate);
					DateTime end = DateTime.Parse(this.EndDate);
					if (start > end)
					{
						UserDialogs.Instance.Toast("Ustaw prawidłowo zakres dat.");
						return false;
					}

				}

                if (this.Bytes != null)
                {
                    dataRecord.ImageName = $"image_{medicationName}";
                    dataRecord.ThumbnailName = $"thumbnail_{medicationName}";
                    imageLoader.SaveImage(this.Bytes, dataRecord.ImageName);
                    imageLoader.SaveImage(this.Bytes, dataRecord.ThumbnailName, 120);
                }

				await this.storage.SaveAsync<MedicationDosage>(dataRecord);
                //var notification = new CoreNotification(dataRecord.Id.Value, dataRecord.Name, "Rano i wieczorem", new RepeatPattern() { DayOfWeek = dataRecord.Days, Interval = RepetitionInterval.None, RepetitionFrequency = 1 });
                await this.notifications.ScheduleNotification(dataRecord);

                return true;
            }, canSave);

            var canDelete = this.WhenAny(x => x.Id, id => id.Value.HasValue);
            this.Delete = RxUI.ReactiveCommand.CreateFromTask<Data.MedicationDosage, bool>(async _ =>
               {
                   if (this.Id.HasValue)
                   {
                       await this.storage.DeleteByKeyAsync<MedicationDosage>(this.Id.Value);
                       return true;
                   }
                   return false;
             }, canDelete);

            this.SelectAllDays = ReactiveCommand.Create(() => 
            {
                selectAllDays();
            });

            //save sie udal, albo nie - tu dosatniemy rezultat komendy. Jak sie udal, to zamykamy ViewModel
            this.Save
                .Subscribe(result =>
                {
                    if (result)
                    {
                        Mvx.Resolve<IMvxMessenger>().Publish(new DataChangedMessage(this));
                        this.Close(this);
                    }
                });

            this.Save.ThrownExceptions.Subscribe(ex =>
            {
                UserDialogs.Instance.ShowError(AppResources.MedicationDosageView_SaveError);
                // show nice message to the user

            });

            this.Delete
                .Subscribe(result =>
                {
                    if (result)
                    {
                        Mvx.Resolve<IMvxMessenger>().Publish(new DataChangedMessage(this));
                        this.Close(this);
                    }
                });
            GoSettings = ReactiveCommand.Create(() => this.ShowViewModel<SettingsViewModel>());
            SetRepeatTime = ReactiveCommand.Create<IList<TimeItem>>(p =>
            {
                CheckedHours = p as List<TimeItem>;
                setHours();

            });          
        }
    
        private void loadSettings()
        {
            SettingsData data = JsonConvert.DeserializeObject<SettingsData>(settings.GetValue<string>(SettingsData.Key));
            if (data == null)
            {
                data = new SettingsData();
                data.HoursList = new List<TimeItem>()
                {
                    new TimeItem(Resources.AppResources.MorningLabel) {Hour = TimeSpan.Parse("08:00:00") },
                    new TimeItem(Resources.AppResources.EveningLabel){Hour=TimeSpan.Parse("20:00:00")}
            	};

                settings.AddOrUpdateValue<string>(SettingsData.Key, JsonConvert.SerializeObject(data));
            }

            TimeItems = new ReactiveList<TimeItem>(data.HoursList);
            string[] hoursNames;
            if (HoursLabel == null)
                hoursNames = new string[1] { TimeItems[0].Name };
            else
            {
				hoursNames = HoursLabel.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            }

            CheckedHours = TimeItems
                .Where(h => hoursNames.Contains(h.Name))
                .Select(h => { h.Checked = true; return h; })
                .ToList();
			
            setHours();

			RingUri = data.RingUri;
        }

        private void setHours()
        {
			DosageHours.Clear();
			HoursLabel = String.Join(", ", this.CheckedHours.Select(i => i.Name));

			foreach(var item in CheckedHours)
			{
			    DosageHours.Add(item.Hour);
			}
        }


        public async void Init(MedicationDosageNavigation nav)
        {
            if (nav.MedicationDosageId != MedicationDosageNavigation.NewRecord)
            {
                isNew = false;
                MedicationDosage item = await storage.GetAsync<Data.MedicationDosage>(nav.MedicationDosageId);
                Id = item.Id;
                MedicationName = item.Name;
				StartDate = item.From;
				EndDate = item.To;
                MedicationDosage = item.Dosage;
                Monday = item.Days.HasFlag(DaysOfWeek.Monday);
                Tuesday = item.Days.HasFlag(DaysOfWeek.Tuesday);
                Wednesday = item.Days.HasFlag(DaysOfWeek.Wednesday);
                Thursday = item.Days.HasFlag(DaysOfWeek.Thursday);
                Friday = item.Days.HasFlag(DaysOfWeek.Friday);
                Saturday = item.Days.HasFlag(DaysOfWeek.Saturday);
                Sunday = item.Days.HasFlag(DaysOfWeek.Sunday);
				DosageHours = new ReactiveList<TimeSpan>(item.DosageHours);
                HoursLabel = item.Hours;
				RingUri = item.RingUri;

                if (!string.IsNullOrEmpty(item.ImageName))
				    Bytes = imageLoader.LoadImage(item.ImageName);
            }
            else
            {
                isNew = true;
                selectAllDays();
            }
            loadSettings();
        }
    }
}
