using System;
using System.Threading.Tasks;
using Piller.Data;

namespace Piller.Services
{
    public interface INotificationService
    {
        Task ScheduleNotification(MedicationDosage medicationDosage);
		Task CancelNotification(int id);
    }
}
