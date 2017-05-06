using System;
using MvvmCross.iOS.Views;
using Piller.ViewModels;
using MvvmCross.Binding.BindingContext;

namespace Piller.iOS.Views
{
    public class MedicationSummaryListView : MvxTableViewController<MedicationSummaryListViewModel>
    {
        public MedicationSummaryListView ()
        {
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            this.TableView.Source = new MedicationSummaryListViewSource (this.TableView);

            var addButton = new UIKit.UIBarButtonItem (UIKit.UIBarButtonSystemItem.Add);
            this.NavigationItem.RightBarButtonItem = addButton;

            var bindingSet = this.CreateBindingSet<MedicationSummaryListView, MedicationSummaryListViewModel> ();
            bindingSet.Bind (addButton).To (vm => vm.AddNew);
            bindingSet.Apply ();
        }
    }
}
