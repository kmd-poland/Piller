﻿using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Piller.Data;
using Piller.Services;
using ReactiveUI;

namespace Piller.ViewModels
{
    public class RootViewModel : MvxViewModel
    {
        public ReactiveCommand<Unit, bool> ShowUpcomingMedicationView { get; }
        public ReactiveCommand<Unit, bool> ShowMedicalCardView { get; }
        public ReactiveCommand<Unit, bool> ShowHolidayView { get; }
        public ReactiveCommand<Unit, bool> GoSettings { get; set; }
        public ReactiveCommand<Unit, Unit> ShowInitialView { get; }
        public Interaction<int, Unit> ChangeTab { get; }

        public RootViewModel() {
            ShowUpcomingMedicationView = ReactiveCommand.Create(() => this.ShowViewModel<RegistrationViewModel>());
            ShowMedicalCardView = ReactiveCommand.Create(() => this.ShowViewModel<MedicalCardViewModel>());
            ShowHolidayView = ReactiveCommand.Create(() => this.ShowViewModel<HolidayViewModel>());
            GoSettings = ReactiveCommand.Create(() => this.ShowViewModel<SettingsViewModel>());

            this.ChangeTab = new Interaction<int, Unit>();

            this.ShowInitialView = ReactiveCommand.Create(() =>
            {
				Task.Delay(324).ContinueWith(async task =>
                {
                    if ((await Mvx.Resolve<IPermanentStorageService>().List<MedicationDosage>()).Any())
                    {
                        await this.ChangeTab.Handle(0);
                    }
                    else
                    {
                        await this.ChangeTab.Handle(1);
                    }
                });
            });
        }
    }
}
