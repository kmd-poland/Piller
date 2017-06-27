using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using Piller.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Piller.Services;
using MvvmCross.Platform;

namespace Piller.ViewModels
{
    public class RegistrationViewModel : MvxViewModel
    {
        private IPermanentStorageService storage = Mvx.Resolve<IPermanentStorageService>();
        private readonly INotificationService notifications = Mvx.Resolve<INotificationService>();

        private List<NotificationOccurrence> nearestList;
        public List<NotificationOccurrence> NearestList
        {
            get { return nearestList; }
            set { SetProperty(ref nearestList, value); RaisePropertyChanged(nameof(IsEmptyNearest)); }
        }
        public bool IsEmptyNearest
        {
            get { return !nearestList.Any(); }
        }
        private List<OverdueNotification> overdueList;
        public List<OverdueNotification> OverdueList
        {
            get { return overdueList; }
            set { SetProperty(ref overdueList, value); RaisePropertyChanged(nameof(IsEmptyOverdue)); }
        }
        public bool IsEmptyOverdue
        {
            get { return !overdueList.Any(); }
        }

        public async Task DeleteOverdue(OverdueNotification medication)
        {
            await storage.DeleteAsync<OverdueNotification>(medication);
            this.OverdueList.Remove(medication);
        }

        public async Task DeleteNearest(NotificationOccurrence notification)
        {
            this.notifications.CancelNotification(notification);
            await storage.DeleteAsync<NotificationOccurrence>(notification);
        }

        public async Task OverdueNearest(NotificationOccurrence notification)
        {
            var medications = await this.storage.List<MedicationDosage>();
            var medicationDosage = medications.FirstOrDefault(n => n.Id == notification.MedicationDosageId);
            this.notifications.CancelNotification(notification);
            
            await storage.DeleteAsync<NotificationOccurrence>(notification);
            //await this.notifications.OverdueNotification();
        }

        public async Task Init()
        {
            var itemsOverdue = await this.storage.List<OverdueNotification>();
            if (itemsOverdue != null)
                this.OverdueList = itemsOverdue;
            else
                this.OverdueList = new List<OverdueNotification>();

            var itemsNearest = await this.storage.List<NotificationOccurrence>();
            if (itemsNearest != null)
                this.NearestList = itemsNearest;
            else
                this.NearestList = new List<NotificationOccurrence>();
        }
    }
}
