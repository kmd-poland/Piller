using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using Piller.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Piller.Services;
using MvvmCross.Platform;
using System.Collections.ObjectModel;
using Piller.MixIns.DaysOfWeekMixIns;
using MvvmCross.Plugins.Messenger;

namespace Piller.ViewModels
{
    public class RegistrationViewModel : MvxViewModel
    {
        private IPermanentStorageService storage = Mvx.Resolve<IPermanentStorageService>();
        private readonly INotificationService notifications = Mvx.Resolve<INotificationService>();
        MvxSubscriptionToken notificationsChangedSubscriptionToken;

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
            await this.notifications.CancelNotification(notification);
            //await this.notifications.ScheduleNotification(notification);
        }

        public async Task DeleteNearest(NotificationOccurrence notification)
        {
            await this.notifications.CancelNotification(notification);
            //await this.notifications.ScheduleNotification(notification);
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
            var allNotifications = await this.storage.List<NotificationOccurrence>();

            notificationsChangedSubscriptionToken = Mvx.Resolve<IMvxMessenger>().Subscribe<NotificationsChangedMessage>(async mesg => await Init());

            DateTime now = DateTime.Now;
            DateTime start = now.AddHours(-2);
            DateTime end = now.AddHours(2);
            var overdueNotifications = allNotifications.Where(n => n.OccurrenceDateTime < start);
            var nearestNotifications = allNotifications.Where(n => n.OccurrenceDateTime >= start && n.OccurrenceDateTime <= end);
            var laterNotifications = allNotifications.Where(n => n.OccurrenceDateTime > end);

            var sortedOverdueList = overdueNotifications.OrderBy(e => e.OccurrenceDateTime);
            this.OverdueList = sortedOverdueList.ToList();
            var sortedNearestList = nearestNotifications.OrderBy(e => e.OccurrenceDateTime);
            this.NearestList = sortedNearestList.ToList();
            var sortedLaterList = laterNotifications.OrderBy(e => e.OccurrenceDateTime);
            this.LaterList = sortedLaterList.ToList();

            /*var itemsNearest = await this.storage.List<NotificationOccurrence>();
            if (itemsNearest != null)
                this.NearestList = itemsNearest;
            else
                this.NearestList = new List<NotificationOccurrence>();*/
        }
    }
}
