using System;
using MvvmCross.Binding.Droid.Views;
namespace Piller.Droid.Views
{
    public class MedicationDosageTimeLayout : MvxLinearLayout
    {
        public MedicationDosageTimeLayout(Android.Content.Context context, Android.Util.IAttributeSet attrs) : base(context, attrs, new MedicationDosageTimeListAdapter(context))
        {
        }
    }
}
