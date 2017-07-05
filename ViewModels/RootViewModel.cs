using System;
using System.Reactive;
using System.Reactive.Linq;
using MvvmCross.Core.ViewModels;
using ReactiveUI;

namespace Piller.ViewModels
{
    public class RootViewModel : MvxViewModel
    {
        public ReactiveCommand<Unit, bool> ShowUpcomingMedicationView { get; }
        public ReactiveCommand<Unit, bool> ShowMedicalCardView { get; }
        public ReactiveCommand<Unit, bool> ShowHolidayView { get; }
        public ReactiveCommand<Unit, bool> GoSettings { get; set; }

        public RootViewModel() {
            ShowUpcomingMedicationView = ReactiveCommand.Create(() => this.ShowViewModel<RegistrationViewModel>());
            ShowMedicalCardView = ReactiveCommand.Create(() => this.ShowViewModel<MedicalCardViewModel>());
            ShowHolidayView = ReactiveCommand.Create(() => this.ShowViewModel<HolidayViewModel>());
            GoSettings = ReactiveCommand.Create(() => this.ShowViewModel<SettingsViewModel>());
        }
    }
}
