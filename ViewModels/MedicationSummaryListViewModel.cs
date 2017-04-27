using System;
using System.Reactive;
using MvvmCross.Core.ViewModels;
using ReactiveUI;
using System.Collections.Generic;
using Piller.Data;
using Piller.Services;
using MvvmCross.Platform;

namespace Piller.ViewModels
{
    public class MedicationSummaryListViewModel : MvxViewModel
    {
        private IPermanentStorageService storage = Mvx.Resolve<IPermanentStorageService>();
        private List<Data.MedicationDosage> medicationList;
        public List<Data.MedicationDosage> MedicationList
        {
            get { return medicationList; }
            set { SetProperty(ref medicationList, value); }
        }
        private Data.MedicationDosage selectedItem;
        public Data.MedicationDosage SelectedItem
        {
            get { return selectedItem; }
            set { this.SetProperty(ref selectedItem, value); }
        }
        
        public ReactiveCommand<Unit, bool> AddNew { get; }

        public ReactiveCommand<Data.MedicationDosage,Unit> Update { get; }

        public MedicationSummaryListViewModel()
        {
            AddNew = ReactiveCommand.Create(() => this.ShowViewModel<MedicationDosageViewModel>());
           Update= ReactiveCommand.Create<Data.MedicationDosage>((item) =>
           {
               this.ShowViewModel<MedicationDosageViewModel>(new MedicationDosageNavigation { MedicationDosageId = item.Id, Edit = true });
           });

        }
        public override void Start()
        {
            base.Start();
            
            Read();
        }

        private async void Read()
        {
            medicationList = new List<Data.MedicationDosage>();
            var items = await storage.List<Data.MedicationDosage>();
            if (items != null)
                foreach(Data.MedicationDosage item in items)
                    MedicationList.Add(item) ;
        }
    }
}
