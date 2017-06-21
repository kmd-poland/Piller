using System;
using MvvmCross.Plugins.Messenger;
namespace Piller.ViewModels
{
    public class DataChangedMessage : MvxMessage
    {
        public DataChangedMessage(object sender) : base(sender)
        {
        }
    }
    public class SettingsChangeMessage : MvxMessage
    {
        public TimeSpan Morning { get; }
        public TimeSpan Evening { get; }
        public SettingsChangeMessage(object sender, TimeSpan morning, TimeSpan evening) : base(sender)
        {
            Morning = morning;
            Evening = evening;
        }
    }

}
