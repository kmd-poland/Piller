using System;
using MvvmCross.Core.ViewModels;
using RxUI = ReactiveUI;
using System.Reactive;
using Piller.Data;
using Piller.Services;
using MvvmCross.Platform;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;


namespace Piller.ViewModels
{
    public class MedicationDosageViewModel : MvxViewModel
    {
        private IPermanentStorageService storage = Mvx.Resolve<IPermanentStorageService> ();

        string medicationName;

        public string MedicationName {
            get { return medicationName; }
            set { this.RaiseAndSetIfChanged (ref medicationName, value); }
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
        private RxUI.ReactiveList<TimeSpan> times;
        public RxUI.ReactiveList<TimeSpan> Times
        {
            get { return this.times; }
            set { SetProperty(ref times, value); }
        }
        public RxUI.ReactiveCommand<Unit, bool> Save { get; private set; }
        public RxUI.ReactiveCommand<Data.MedicationDosage,Unit> Delete { get; set; }
        public RxUI.ReactiveCommand SelectAllDays { get; set; }

        public MedicationDosageViewModel ()
        {
            this.Times = new RxUI.ReactiveList<TimeSpan>();

            this.Save = RxUI.ReactiveCommand.CreateFromTask<Unit, bool> (async _ => {

                if (isEdit)
                    await this.storage.UpdateAsync<MedicationDosage>(new Data.MedicationDosage {Id=editedItem.Id, Name = this.medicationName, Dosage = this.MedicationDosage,Monday=this.Monday,Tuesday=this.Tuesday,Wednesday=this.Wednesday,Thursday=this.Thurdsday,Friday=this.Friday,Saturday=this.Saturday,Sunday=this.Sunday });
                else
                    await this.storage.SaveAsync<Data.MedicationDosage>(new Data.MedicationDosage { Name = this.medicationName, Dosage = this.MedicationDosage, Monday = this.Monday, Tuesday = this.Tuesday, Wednesday = this.Wednesday, Thursday = this.Thurdsday, Friday = this.Friday, Saturday = this.Saturday, Sunday = this.Sunday });
  
                //tu powinnismy zwracac rezultat (czy sie udalo czy nie). Reakcje robimy w Subscribe
                //  this.Close (this);
               // this.ShowViewModel<MedicationSummaryListViewModel>();
                return true;
            });
            this.Delete = RxUI.ReactiveCommand.CreateFromTask<Data.MedicationDosage>(async _ =>
              {
                  await this.storage.DeleteAsync<MedicationDosage>(editedItem);
                  this.ShowViewModel<MedicationSummaryListViewModel>();

                  // this.Close(this);
              });

            this.SelectAllDays = RxUI.ReactiveCommand.Create(() => { Monday = true; Tuesday = true; Wednesday = true; Thurdsday = true; Friday = true; Saturday = true; Sunday = true; });


            //save sie udal, albo nie - tu dosatniemy rezultat komendy. Jak sie udal, to zamykamy ViewModel
            this.Save
                .Subscribe(result =>
                {
                    if (result)
                        this.Close(this);
                });
                
            this.Save.ThrownExceptions.Subscribe (ex => {
                  Debug.WriteLine($"ex {ex}");
                // show nice message to the user
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
        }
    }
}