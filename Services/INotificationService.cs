using System;
using Piller.Core.Domain;
using System.Threading.Tasks;

namespace Piller.Core.Services
{
    public interface INotificationService
    {
        Task ScheduleNotification (CoreNotification notification);
        void CancelNotification (int id);
    }
}
