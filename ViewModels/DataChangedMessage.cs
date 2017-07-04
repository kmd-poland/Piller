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
		public string RingUri { get; }

		public SettingsChangeMessage(object sender, TimeSpan morning, TimeSpan evening, string ringUri) : base(sender)
        {
            Morning = morning;
            Evening = evening;
            RingUri = ringUri;

        }
    }

}
