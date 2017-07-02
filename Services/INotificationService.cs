using System;
using System.Threading.Tasks;
using Piller.Data;

namespace Piller.Services
{
    public interface INotificationService
    {
        Task ScheduleNotifications(MedicationDosage medicationDosage);
		Task CancelNotification(int id);
        Task CancelNotification(NotificationOccurrence notification);
        Task CancelNotifications(MedicationDosage medicationDosage);
        Task OverdueNotification(NotificationOccurrence notificationOccurrence, MedicationDosage medicationDosage);
        Task CancelAndRemove(int id);
    }
}
