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
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.BindingContext;
using Piller.Data;

namespace Piller.Droid.Views
{
    public class SelectTimeAdapter : ArrayAdapter<TimeItem>
    {
       
        public SelectTimeAdapter(Context context) : base(context, Resource.Layout.selectTimeItem)
        {
        }

        public SelectTimeAdapter(Context context, IList<TimeItem> objects) : base(context, Resource.Layout.selectTimeItem, objects)
        {
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var timeItem = GetItem(position);
            var view = LayoutInflater.From(this.Context).Inflate(Resource.Layout.selectTimeItem, null);
          
            var item = view.FindViewById<CheckBox>(Resource.Id.checkBoxItem);
            item.Checked = timeItem.Checked;
            item.Text = timeItem.Label;
			item.CheckedChange += (sender, e) => timeItem.Checked = e.IsChecked;
    
            return view;
		
		}
   
    }
}