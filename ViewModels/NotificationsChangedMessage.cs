using System;
using MvvmCross.Plugins.Messenger;
namespace Piller.ViewModels
{
    public class NotificationsChangedMessage : MvxMessage
    {
        public NotificationsChangedMessage(object sender) : base(sender)
        {
        }
    }
}
