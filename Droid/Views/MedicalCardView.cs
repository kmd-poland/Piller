
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Support.V4;
using Piller.ViewModels;

namespace Piller.Droid.Views
{
	[MvxFragment(typeof(RootViewModel), Resource.Id.content_frame, true)]
	[Register("piller.droid.views.MedicalCardView")]
    public class MedicalCardView : MvxFragment<MedicalCardViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle bundle)
        {
			var ignore = base.OnCreateView(inflater, container, bundle);

            return this.BindingInflate(Resource.Layout.MedicalCard, null);
        }
    }
}
