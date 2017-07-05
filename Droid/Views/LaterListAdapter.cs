using System;
using Android.Runtime;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.Views;
using Piller.Data;
using MvvmCross.Binding.Droid.BindingContext;
using Android.Content;
using Piller.Droid.BindingConverters;
using Android.Views;
using System.Collections.Generic;
using System.Linq;
using Android.Graphics;
using MvvmCross.Plugins.File;
using Services;
using MvvmCross.Platform;
using ReactiveUI;
using Humanizer;

namespace Piller.Droid.Views
{

    public class LaterListLayout : MvxLinearLayout
    {
		public LaterListLayout(Context context, Android.Util.IAttributeSet attrs) : base(context, attrs, new LaterListAdapter(context))
        {

		}

	}
    public class LaterListAdapter : MvxAdapterWithChangedEvent
    {

		private readonly ImageLoaderService imageLoader = Mvx.Resolve<ImageLoaderService>();

        public LaterListAdapter(Context context) : base(context)
        {
        }

       

        protected override IMvxListItemView CreateBindableView(object dataContext, int templateId)
        {
            var view = base.CreateBindableView(dataContext, templateId) as MvxListItemView;
            var name = view.FindViewById<TextView>(Resource.Id.label_later_name);
            var dosage = view.FindViewById<TextView>(Resource.Id.label_later_dosage);
            var time = view.FindViewById<TextView>(Resource.Id.label_later_time);
            var bset = view.CreateBindingSet<MvxListItemView, NotificationOccurrence>();
			var thumbnail = view.FindViewById<ImageView>(Resource.Id.list_thumbnail);

            bset.Bind(name)
                .To(x => x.Name);

            bset.Bind(dosage)
                .To(x => x.Dosage);

            bset.Bind(time)
                .To(x => x.OccurrenceDateTime)
                .WithConversion(new InlineValueConverter<DateTime, string>(dt => dt.Humanize(false)));

			bset.Bind(thumbnail)
			   .To(x => x.ThumbnailImage)
			   .For("Bitmap")
			   .WithConversion(new InlineValueConverter<string, Bitmap>(file =>
			   {
				   if (file != null)
				   {
					   byte[] array = imageLoader.LoadImage(file);
					   return BitmapFactory.DecodeByteArray(array, 0, array.Length);
				   }
				   else
				   {

					   return BitmapFactory.DecodeResource(this.Context.Resources, Resource.Drawable.pillThumb);
				   }
			   }));

            bset.Apply();
            return view;
        }
    }
}
