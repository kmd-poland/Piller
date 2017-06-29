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
        private List<NotificationOccurrence> overdueList;
        public List<NotificationOccurrence> OverdueList
        {
            get { return overdueList; }
            set { SetProperty(ref overdueList, value); RaisePropertyChanged(nameof(IsEmptyOverdue)); }
        }
        public bool IsEmptyOverdue
        {
            get { return !overdueList.Any(); }
        }

        private List<NotificationOccurrence> laterList;
        public List<NotificationOccurrence> LaterList
        {
            get { return laterList; }
            set { SetProperty(ref laterList, value); RaisePropertyChanged(nameof(IsEmptyLater)); }
        }
        public bool IsEmptyLater
        {
            get { return !laterList.Any(); }
        }

        public async Task DeleteOverdue(NotificationOccurrence notification)
        {
            this.notifications.CancelNotification(notification);
            await storage.DeleteAsync<NotificationOccurrence>(notification);
            this.OverdueList.Remove(notification);
        }

        public async Task DeleteNearest(NotificationOccurrence notification)
        {
            this.notifications.CancelNotification(notification);
            await storage.DeleteAsync<NotificationOccurrence>(notification);
            this.NearestList.Remove(notification);
        }

        /*public async Task OverdueNearest(NotificationOccurrence notification)
        {
            var medications = await this.storage.List<MedicationDosage>();
            var medicationDosage = medications.FirstOrDefault(n => n.Id == notification.MedicationDosageId);
            this.notifications.CancelNotification(notification);
            
            await storage.DeleteAsync<NotificationOccurrence>(notification);
            //await this.notifications.OverdueNotification();
        }*/

        public async Task Init()
        {
            var notifications = await this.storage.List<NotificationOccurrence>();

            DateTime now = DateTime.Now;
            DateTime start = now.AddHours(-2);
            DateTime end = now.AddHours(2);
            var overdueNotifications = notifications.Where(n => n.OccurrenceDateTime < start);
            var nearestNotifications = notifications.Where(n => n.OccurrenceDateTime > start && n.OccurrenceDateTime < end);
            var laterNotifications = notifications.Where(n => n.OccurrenceDateTime > end);

            this.OverdueList = overdueNotifications.ToList<NotificationOccurrence>();
            this.NearestList = nearestNotifications.ToList<NotificationOccurrence>();
            this.LaterList = laterNotifications.ToList<NotificationOccurrence>();

            /*var itemsNearest = await this.storage.List<NotificationOccurrence>();
            if (itemsNearest != null)
                this.NearestList = itemsNearest;
            else
                this.NearestList = new List<NotificationOccurrence>();*/
        }
    }
}
