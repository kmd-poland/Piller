using Android.App;
using Android.OS;
using Piller.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using Piller.Resources;
using MvvmCross.Binding.BindingContext;
using Android.Views;

using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Widget;
using Android.Support.V7.Widget;
using System;
namespace Piller.Droid.Views
{
    [Activity]
    public class MedicationDosageView : MvxAppCompatActivity<MedicationDosageViewModel>
    {
        EditText nameText;
        EditText dosageText;
        Button saveBtn;
        Button deleteBtn;
        Button daysBtn;
        TextView time_label;
        CheckBox monday;
        CheckBox tuesday;
        CheckBox wednesday;
        CheckBox thursday;
        CheckBox friday;
        CheckBox saturday;
        CheckBox sunday;
        const int TIME_DIALOG_ID = 0;
        Button timePicker;
        private int hour;
        private int minute;
        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MedicationDosageView);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
           
            //Toolbar will now take on default actionbar characteristics
            SetSupportActionBar(toolbar);

            SupportActionBar.Title = AppResources.MedicationDosageViewModel_Title;
            nameText = FindViewById<EditText>(Resource.Id.NameEditText);
            dosageText = FindViewById<EditText>(Resource.Id.DosageEditText);

            monday = FindViewById<CheckBox>(Resource.Id.mondayCheckBox);
            tuesday = FindViewById<CheckBox>(Resource.Id.tuesdayCheckBox);
            wednesday = FindViewById<CheckBox>(Resource.Id.wednesdayCheckBox);
            thursday = FindViewById<CheckBox>(Resource.Id.thursdayCheckBox);
            friday = FindViewById<CheckBox>(Resource.Id.fridayCheckBox);
            saturday = FindViewById<CheckBox>(Resource.Id.saturdayCheckBox);
            sunday = FindViewById<CheckBox>(Resource.Id.sundayCheckBox);

            saveBtn = FindViewById<Button>(Resource.Id.saveBtn);
            deleteBtn = FindViewById<Button>(Resource.Id.deleteBtn);
            daysBtn = FindViewById<Button>(Resource.Id.everyDayBtn);

            timePicker = FindViewById<Button>(Resource.Id.time_picker);
            time_label = FindViewById<TextView>(Resource.Id.timeDisplay);
            timePicker.Click += (o, e) => ShowDialog(TIME_DIALOG_ID);
            hour = DateTime.Now.Hour;
            minute = DateTime.Now.Minute;
            UpdateDisplay();
           SetBinding();
            

        }
        private void UpdateDisplay()
        {
            string time = string.Format("{0}:{1}", hour, minute.ToString().PadLeft(2, '0'));
            time_label.Text = $"{hour}:{minute}";
        }
        private void TimePickerCallback(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            hour = e.HourOfDay;
            minute = e.Minute;
            UpdateDisplay();
            //Co dalej???
        }
        protected override Dialog OnCreateDialog(int id)
        {
            if (id == TIME_DIALOG_ID)
                return new TimePickerDialog(this, TimePickerCallback, hour, minute, false);

            return null;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.dosagemenu, menu);
            var saveItem = menu.FindItem(Resource.Id.action_save);
            /**
             * nie działa
             bindignSet.Bind(saveItem)
                        .To(vm=>vm.Sav);
             * */
            return base.OnCreateOptionsMenu(menu);
        }
        
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            /**
             * ????????????????????????????????????
             *Jak tutaj wywołać akcję z modelu widoku?
             * */
            Toast.MakeText(this, "Klik", ToastLength.Long).Show();
            return base.OnOptionsItemSelected(item);
        }
        private MvxFluentBindingDescriptionSet<MedicationDosageView, MedicationDosageViewModel> bindingSet;
        private void SetBinding()
        {
            bindingSet = this.CreateBindingSet<MedicationDosageView, MedicationDosageViewModel>();
            bindingSet.Bind(deleteBtn)
               .For(x=>x.Visibility)
               .To(vm => vm.IsEdit)
               .WithConversion("Visibility");

            bindingSet.Bind(nameText)
                      .To(x => x.MedicationName);
            bindingSet.Bind(saveBtn)
                .For(nameof(View.Click))
                .To(vm => vm.Save);
            bindingSet.Bind(dosageText)
                .To(vm => vm.MedicationDosage);
            bindingSet.Bind(deleteBtn)
                .To(vm => vm.Delete);

            bindingSet.Bind(monday)
                .For(x=>x.Checked)
                .Mode(MvvmCross.Binding.MvxBindingMode.TwoWay)
                .To(vm => vm.Monday);
            bindingSet.Bind(tuesday)
               .For(x => x.Checked)
               .Mode(MvvmCross.Binding.MvxBindingMode.TwoWay)
               .To(vm => vm.Tuesday);
            bindingSet.Bind(wednesday)
               .For(x => x.Checked)
               .Mode(MvvmCross.Binding.MvxBindingMode.TwoWay)
               .To(vm => vm.Wednesday);
            bindingSet.Bind(thursday)
               .For(x => x.Checked)
               .Mode(MvvmCross.Binding.MvxBindingMode.TwoWay)
               .To(vm => vm.Thurdsday);
            bindingSet.Bind(friday)
               .For(x => x.Checked)
               .Mode(MvvmCross.Binding.MvxBindingMode.TwoWay)
               .To(vm => vm.Friday);
            bindingSet.Bind(saturday)
               .For(x => x.Checked)
               .Mode(MvvmCross.Binding.MvxBindingMode.TwoWay)
               .To(vm => vm.Saturday);
            bindingSet.Bind(sunday)
               .For(x => x.Checked)
               .Mode(MvvmCross.Binding.MvxBindingMode.TwoWay)
               .To(vm => vm.Sunday);

            bindingSet.Bind(daysBtn)
                .To(vm => vm.SelectAllDays);
            bindingSet.Apply();


            
        }

    }
}
