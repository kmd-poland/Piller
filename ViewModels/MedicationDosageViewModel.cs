using System;
using MvvmCross.Core.ViewModels;
using RxUI = ReactiveUI;
using System.Reactive;
using Piller.Data;
using Piller.Services;
using MvvmCross.Platform;
<<<<<<< HEAD
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using Android.Widget;
=======
using Acr.UserDialogs;
using Piller.Resources;
using ReactiveUI;
using MvvmCross.Plugins.Messenger;
>>>>>>> origin/develop

namespace Piller.ViewModels
{
    public class MedicationDosageViewModel : MvxViewModel
    {
        private IPermanentStorageService storage = Mvx.Resolve<IPermanentStorageService>();

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
            set { this.SetProperty(ref monday, value); }
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
        public bool Thurdsday
        {
            get { return thursday; }
            set { this.SetProperty(ref thursday, value); }
        }

        private bool friday;
        public bool Friday
        {
            get { return friday; }
            set { this.SetProperty(ref friday, value); }
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
            set { this.SetProperty(ref sunday, value); }
        }


        private RxUI.ReactiveList<TimeSpan> dosageHours;
        public RxUI.ReactiveList<TimeSpan> DosageHours
        {
            get { return this.dosageHours; }
            set { SetProperty(ref dosageHours, value); }
        }


        string medicationDosage;
        public string MedicationDosage {
            get { return medicationDosage; }
            set { this.RaiseAndSetIfChanged(ref medicationDosage, value); }
            }

        private int hour;
        public int Hour {
            get{ return hour;}
                set{ this.RaiseAndSetIfChanged(ref hour, value); }
            }

        private int minute;
        public int Minute
        {
            get { return minute; }
            set { this.RaiseAndSetIfChanged(ref minute, value); }
        }
        public string Time { get { return $"{hour}:{minute}"; } }
        private bool monday;
        public bool Monday
        {
            get { return monday; }
            set { this.RaiseAndSetIfChanged(ref monday, value); }
        }
        private bool tuesday;
        public bool Tuesday
        {
            get { return tuesday; }
            set { this.RaiseAndSetIfChanged(ref tuesday, value); }
        }
        private bool wednesday;
        public bool Wednesday
        {
            get { return wednesday; }
            set { this.RaiseAndSetIfChanged(ref wednesday, value); }
        }
        private bool thursday;
        public bool Thurdsday
        {
            get { return thursday; }
            set { this.RaiseAndSetIfChanged(ref thursday, value); }
        }
        private bool friday;
        public bool Friday
        {
            get { return friday; }
            set { this.RaiseAndSetIfChanged(ref friday, value); }
        }
        private bool saturday;
        public bool Saturday
        {
            get { return saturday; }
            set { this.RaiseAndSetIfChanged(ref saturday, value); }
        }
        private bool sunday;
        public bool Sunday
        {
            get { return sunday; }
            set { this.RaiseAndSetIfChanged(ref sunday, value); }
        }

        private bool isEdit;
        public bool IsEdit
        {
            get { return this.isEdit; }
            set { this.RaiseAndSetIfChanged(ref isEdit, value); }
        }
        private RxUI.ReactiveList<TimeSpan> notificationHours;
        public RxUI.ReactiveList<TimeSpan> NotificationHours
        {
            get { return this.notificationHours; }
            set { SetProperty(ref notificationHours, value); }
        }
        public RxUI.ReactiveCommand<Unit, bool> Save { get; private set; }
<<<<<<< HEAD
        public RxUI.ReactiveCommand<Data.MedicationDosage,bool> Delete { get; set; }
=======
        public RxUI.ReactiveCommand<MedicationDosage, bool> Delete { get; set; }
>>>>>>> origin/develop
        public RxUI.ReactiveCommand SelectAllDays { get; set; }

        public MedicationDosageViewModel()
        {
<<<<<<< HEAD
            this.NotificationHours = new RxUI.ReactiveList<TimeSpan>();

            this.Save = RxUI.ReactiveCommand.CreateFromTask<Unit, bool> (async _ => {
=======
            this.DosageHours = new RxUI.ReactiveList<TimeSpan>();

            this.Save = RxUI.ReactiveCommand.CreateFromTask<Unit, bool>(async _ =>
            {

                var dataRecord = new MedicationDosage
                {
                    Id = this.Id,
                    Name = this.MedicationName,
                    Dosage = this.MedicationDosage,
                    Days =
                        (this.Monday ? DaysOfWeek.Monday : DaysOfWeek.None)
                        | (this.Tuesday ? DaysOfWeek.Tuesday : DaysOfWeek.None)
                        | (this.Wednesday ? DaysOfWeek.Wednesday : DaysOfWeek.None)
                        | (this.Thurdsday ? DaysOfWeek.Thursday : DaysOfWeek.None)
                        | (this.Friday ? DaysOfWeek.Friday : DaysOfWeek.None)
                        | (this.Saturday ? DaysOfWeek.Saturday : DaysOfWeek.None)
                        | (this.Sunday ? DaysOfWeek.Sunday : DaysOfWeek.None),
                    DosageHours = this.DosageHours
                };

                await this.storage.SaveAsync<MedicationDosage>(dataRecord);
>>>>>>> origin/develop

                if (isEdit)
                    await this.storage.SaveAsync<MedicationDosage>(new Data.MedicationDosage {Id=editedItem.Id, Name = this.medicationName, Dosage = this.MedicationDosage,Monday=this.Monday,Tuesday=this.Tuesday,Wednesday=this.Wednesday,Thursday=this.Thurdsday,Friday=this.Friday,Saturday=this.Saturday,Sunday=this.Sunday});
                else
                    await this.storage.SaveAsync<Data.MedicationDosage>(new Data.MedicationDosage { Name = this.medicationName, Dosage = this.MedicationDosage, Monday = this.Monday, Tuesday = this.Tuesday, Wednesday = this.Wednesday, Thursday = this.Thurdsday, Friday = this.Friday, Saturday = this.Saturday, Sunday = this.Sunday });
  
                //tu powinnismy zwracac rezultat (czy sie udalo czy nie). Reakcje robimy w Subscribe
                return true;
            });
            this.Delete = RxUI.ReactiveCommand.CreateFromTask<Data.MedicationDosage,bool>(async _ =>
              {
                  await this.storage.DeleteAsync<MedicationDosage>(editedItem);
                  return true;              
              });

            this.SelectAllDays = RxUI.ReactiveCommand.Create(() => { Monday = true; Tuesday = true; Wednesday = true; Thurdsday = true; Friday = true; Saturday = true; Sunday = true; });


<<<<<<< HEAD
=======
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

            this.SelectAllDays = RxUI.ReactiveCommand.Create(() => { Monday = true; Tuesday = true; Wednesday = true; Thurdsday = true; Friday = true; Saturday = true; Sunday = true; });


>>>>>>> origin/develop
            //save sie udal, albo nie - tu dosatniemy rezultat komendy. Jak sie udal, to zamykamy ViewModel
            this.Save
                .Subscribe(result =>
                {
                    if (result)
<<<<<<< HEAD
                        this.Close(this);
                });
                
            this.Save.ThrownExceptions.Subscribe (ex => {
                  Debug.WriteLine($"ex {ex}");
                // show nice message to the user
               
            });

=======
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



>>>>>>> origin/develop
            this.Delete
                .Subscribe(result =>
                {
                    if (result)
<<<<<<< HEAD
                        this.Close(this);
                });
        }
        private MedicationDosage editedItem;

        public async void Init(MedicationDosageNavigation nav)
        {
            if (!nav.Edit) {
                IsEdit = false;
                return;
            }
        
            Data.MedicationDosage item =await storage.GetAsync<Data.MedicationDosage>(nav.MedicationDosageId);
            editedItem = item;
            MedicationName = item.Name;
            MedicationDosage = item.Dosage;
            Monday = item.Monday;
            Tuesday = item.Tuesday;
            Wednesday = item.Wednesday;
            Thurdsday = item.Tuesday;
            Friday = item.Friday;
            Saturday = item.Saturday;
            Sunday = item.Sunday;

            IsEdit = true;
=======
                    {
                        Mvx.Resolve<IMvxMessenger>().Publish(new DataChangedMessage(this));
                        this.Close(this);
                    }
                });
        }


        public async void Init(MedicationDosageNavigation nav)
        {
            if (nav.MedicationDosageId != MedicationDosageNavigation.NewRecord)
            {
                Data.MedicationDosage item = await storage.GetAsync<Data.MedicationDosage>(nav.MedicationDosageId);
                Id = item.Id;
                MedicationName = item.Name;
                MedicationDosage = item.Dosage;
                Monday = item.Days.HasFlag(DaysOfWeek.Monday);
                Tuesday = item.Days.HasFlag(DaysOfWeek.Tuesday);
                Wednesday = item.Days.HasFlag(DaysOfWeek.Wednesday);
                Thurdsday = item.Days.HasFlag(DaysOfWeek.Thursday);
                Friday = item.Days.HasFlag(DaysOfWeek.Friday);
                Saturday = item.Days.HasFlag(DaysOfWeek.Saturday);
                Sunday = item.Days.HasFlag(DaysOfWeek.Sunday);
                DosageHours = new RxUI.ReactiveList<TimeSpan>(item.DosageHours);
            }
      
>>>>>>> origin/develop
        }
    }
}
