using System;
using System.Threading.Tasks;
using Piller.Data;
using Android.Content;
using Android.App;

namespace Piller.Services
{
    public interface INotificationService
    {
        Task ScheduleNotification(MedicationDosage medicationDosage);
		Task CancelNotification(int id);
        void CancelNotification(NotificationOccurrence notification);
        Task OverdueNotification(Notification not, MedicationDosage medicationDosage, DateTime nextOccurrenceDate, Intent notificationIntent);
    }
}
