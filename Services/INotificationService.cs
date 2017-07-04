using System;
using System.Threading.Tasks;
using Piller.Data;

namespace Piller.Services
{
    public interface INotificationService
    {
        Task ScheduleNotifications(MedicationDosage medicationDosage);
        Task ScheduleNotification(NotificationOccurrence notificationOccurrence, MedicationDosage medicationDosage = null);
        Task OverdueNotification(NotificationOccurrence notificationOccurrence, MedicationDosage medicationDosage);

		Task CancelAllNotificationsForMedication(int medicationDosageId);
        Task CancelNotification(NotificationOccurrence notification);
        Task CancelAllNotificationsForMedication(MedicationDosage medicationDosage);
        Task CancelAndRemove(int id);
    }
}
