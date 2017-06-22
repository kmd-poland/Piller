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

        public RootViewModel() {
            ShowUpcomingMedicationView = ReactiveCommand.Create(() => this.ShowViewModel<MedicationSummaryListViewModel>());
            ShowMedicalCardView = ReactiveCommand.Create(() => this.ShowViewModel<MedicalCardViewModel>());
            ShowHolidayView = ReactiveCommand.Create(() => this.ShowViewModel<HolidayViewModel>());
        }
    }
}
