using System;
using Foundation;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace Piller.iOS.Views
{
    public class MedicationSummaryListViewSource : MvxTableViewSource
    {
        public MedicationSummaryListViewSource (UITableView tableView) : base(tableView)
        {
        }

        protected override UITableViewCell GetOrCreateCellFor (UITableView tableView, NSIndexPath indexPath, object item)
        {
            throw new NotImplementedException ();
        }
    }
}
