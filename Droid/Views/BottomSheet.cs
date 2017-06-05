using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Piller.Droid.Views
{
    class BottomSheet: BottomSheetDialogFragment
    {
        static BottomSheetDialogFragment newInstance()
        {
            return new BottomSheetDialogFragment();
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.dialog_days,container,false);
           // return base.OnCreateView(inflater, container, savedInstanceState);
           
        }
    }
}