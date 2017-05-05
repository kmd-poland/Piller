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
}
