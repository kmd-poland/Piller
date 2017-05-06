using System;
using MvvmCross.Core.ViewModels;
using RxUI = ReactiveUI;
using System.Reactive;
using Piller.Data;
using Piller.Services;
using MvvmCross.Platform;

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

        public RxUI.ReactiveCommand<Unit, bool> Save { get; private set; }

        public MedicationDosageViewModel ()
        {
            this.Save = RxUI.ReactiveCommand.CreateFromTask<Unit, bool> (async _ => {
                await this.storage.SaveAsync<MedicationDosage> (new MedicationDosage { Name = this.medicationName });
                this.Close (this);

                return true;
            });

            this.Save.ThrownExceptions.Subscribe (ex => {
                // show nice message to the user
            });
        }
    }
}