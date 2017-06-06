﻿using Android.App;
using Android.OS;
using Piller.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using Piller.Resources;
using MvvmCross.Binding.BindingContext;
using Android.Views;
using Android.Graphics;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Widget;
using System;
using System.Windows.Input;
using System.Reactive;
using MvvmCross.Binding.Droid.Views;
using Android.Opengl;
using Piller.Droid.BindingConverters;
using MvvmCross.Platform.Converters;
using System.Globalization;
using MvvmCross.Plugins.PictureChooser.Droid;
using System.Reactive.Linq;
using Android.Support.Design.Widget;

namespace Piller.Droid.Views
{
	[Activity]
	public class MedicationDosageView : MvxCachingFragmentCompatActivity<MedicationDosageViewModel>
	{
		EditText nameText;
		EditText dosageText;

		Button takePicutre;
		ImageView picture;

		Button deleteBtn;
		Button daysBtn;
		Button timePicker;

        TextView daysOfWeek;

        MedicationDosageTimeLayout hoursList;

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

            takePicutre = FindViewById<Button>(Resource.Id.take_photo);
            picture = FindViewById<ImageView>(Resource.Id.photo);

            daysOfWeek = FindViewById<TextView>(Resource.Id.label_medication_days_of_week);

            deleteBtn = FindViewById<Button>(Resource.Id.deleteBtn);
            daysBtn = FindViewById<Button>(Resource.Id.daysButton);
            hoursList = FindViewById<MedicationDosageTimeLayout>(Resource.Id.notificationHours);
            timePicker = FindViewById<Button>(Resource.Id.time_picker);


            hoursList.ItemTemplateId = Resource.Layout.time_item;


            //obsluga usuwania - jedna z kilku mozliwosci
            //wcisniecie przyscisku delete spowoduje wywolanie na adapterze komendy z usuwana godzina (implementacja w MedicationDosageTimeListAdapter
            var hourAdapter = (MedicationDosageTimeListAdapter)hoursList.Adapter;//dialog tworzymy i pokazujemy z kodu
            hourAdapter.DeleteRequested.Subscribe(time => this.ViewModel.DosageHours.Remove(time));

            //aby ui sie odswiezyl, lista godzin powinna być jakimś typem NotifyCollectionChanged (np. ReactiveList)
            //w samym UI można użyć MvxLinearLayout, który działa podobnie do listy,ale nie spowoduje scrolla w scrollu
            //wtedy właściwość Times bindujemy to tego komponentu
            timePicker.Click += (o, e) =>
            {
                TimePickerDialog timePickerFragment = new TimePickerDialog(
                       this,
                    (s, args) =>
                    {
                        // handler jest wołany dwukrotnie: raz bo wybrana została godzina, drugi bo picker został zamknięty - ten musimy zignorować
                        if (((TimePicker)s).IsShown)
                            this.ViewModel.DosageHours.Add(new TimeSpan(args.HourOfDay, args.Minute, 0));
                    },
                       12,
                       00,
                       true
                   );
                timePickerFragment.Show();
            };

            FirstBottomSheet bottomDialog = new FirstBottomSheet(this);
            SecondBottomSheet secondDialog = new SecondBottomSheet(this);
            DeleteDialog deleteDialog = new DeleteDialog(this);

            View sheetView = LayoutInflater.Inflate(Resource.Layout.bottom_dialog, null);
            View secondVIew = LayoutInflater.Inflate(Resource.Layout.bottom_dialog_2, null);
            View deleteView = LayoutInflater.Inflate(Resource.Layout.delete_dialog, null);

            bottomDialog.SetContentView(sheetView);
            secondDialog.SetContentView(secondVIew);
            deleteDialog.SetContentView(deleteView,new ViewGroup.LayoutParams( ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
            secondDialog.Create();

            bottomDialog.FirstOption.Subscribe(x =>
            {
                ViewModel.SelectAllDays.Execute().Subscribe();
                bottomDialog.Hide();
            });
            bottomDialog.SecondOption.Subscribe(x =>
            {
                this.ViewModel.Monday = true;
                this.ViewModel.Tuesday = true;
                this.ViewModel.Wednesday = true;
                this.ViewModel.Thurdsday = true;
                this.ViewModel.Friday = true;
                this.ViewModel.Saturday = false;
                this.ViewModel.Sunday = false;
                bottomDialog.Dismiss();
            });
            bottomDialog.SetCustom.Subscribe(x =>
            {
                secondDialog.Show();
            });

            secondDialog.Accept.Subscribe(x  =>
            {
                this.ViewModel.Monday = x[0];
                this.ViewModel.Tuesday = x[1];
                this.ViewModel.Wednesday = x[2];
                this.ViewModel.Thurdsday = x[3];
                this.ViewModel.Friday = x[4];
                this.ViewModel.Saturday = x[5];
                this.ViewModel.Sunday = x[6];
                bottomDialog.Hide();
                secondDialog.Hide();
            });
            secondDialog.Cancel.Subscribe(x =>
            {
                secondDialog.Dismiss();
            });

            daysBtn.Click += (o, e) =>bottomDialog.Show();
            deleteDialog.Create();
            deleteBtn.Click += (o, e) => deleteDialog.Show();
            deleteDialog.Accept.Subscribe(x =>
            {
                if(((ICommand)ViewModel.Delete).CanExecute(null))
                    ViewModel.Delete.Execute().Subscribe();
            });
            deleteDialog.Cancel.Subscribe(x => deleteDialog.Dismiss());
            
            SetBinding();
		}



		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			this.MenuInflater.Inflate(Resource.Menu.dosagemenu, menu);
			var saveItem = menu.FindItem(Resource.Id.action_save);

            this.ViewModel.Save.CanExecute.Subscribe(canExecute => saveItem.SetEnabled(canExecute));

			return base.OnCreateOptionsMenu(menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
            //sprawdzamy, czy przycisk ma id zdefiniowane dla Save, i czy Save mozna wywolac (to na przyszlosc, gdy bedzie walidacja)
            // jak tak - odpalamy komendę. To dziwne Subscribe na końcu do wymóg ReactiveUI7
            if (item.ItemId == Resource.Id.action_save && ((ICommand)this.ViewModel.Save).CanExecute(null))
            {
                this.ViewModel.Save.Execute().Catch<bool, Exception>(ex =>
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                    return Observable.Empty<bool>();
                }).Subscribe(_ => {
                    System.Diagnostics.Debug.WriteLine($"Save invoked {_}");
                });

                return true;
            }

            return base.OnOptionsItemSelected(item);
		}

		private MvxFluentBindingDescriptionSet<MedicationDosageView, MedicationDosageViewModel> bindingSet;
		private void SetBinding()   
		{
			bindingSet = this.CreateBindingSet<MedicationDosageView, MedicationDosageViewModel>();

			//sposob na bezposrednie sluchanie observable. W momencie, gdy CanExecute sie zmieni wykona sie kod z Subscribe
			this.ViewModel.Delete.CanExecute.Subscribe(canExecute => deleteBtn.Visibility = canExecute ? ViewStates.Visible : ViewStates.Gone);

			bindingSet.Bind(this.SupportActionBar)
					  .To(x => x.MedicationName)
					  .For(v => v.Title)
					  .WithConversion(new InlineValueConverter<string, string>(medicationName =>
					  {
						  if (string.IsNullOrEmpty(medicationName))
							  return this.ViewModel.Id.HasValue ? "" : AppResources.MedicationDosageViewModel_Title;
						  return medicationName;
					  }));

			bindingSet.Bind(nameText)
					  .To(x => x.MedicationName);

            bindingSet.Bind(picture)
                      .To(x => x.Bytes)
                      .For("Bitmap")
                      .WithConversion(new MvxInMemoryImageValueConverter());

			bindingSet.Bind(picture)
                      .To(x => x.Bytes)
                      .For(v => v.Visibility)
			          .WithConversion(new InlineValueConverter<byte[], ViewStates>((byte[] arg) => arg == null ? ViewStates.Gone : ViewStates.Visible)) ;

            bindingSet.Bind(takePicutre)
				.To(vm => vm.TakePhotoCommand);

			bindingSet.Bind(dosageText)
				.To(vm => vm.MedicationDosage);
			//bindingSet.Bind(deleteBtn)
			//	.To(vm => vm.Delete);
/*
            bindingSet.Bind(daysOfWeek)
                .To(x => x.Days)
                .WithConversion(new DaysOfWeekConverter());
            bindingSet.Bind(daysOfWeek)
                .To(x => x.Days)
                .For(v => v.Visibility)
                .WithConversion(new InlineValueConverter<DaysOfWeek, ViewStates>(dosageHours => dosageHours == DaysOfWeek.None ? ViewStates.Gone : ViewStates.Visible));
                */
            bindingSet.Bind(daysBtn)
            .To(vm => vm.Repeat);

            bindingSet.Bind(hoursList)
				.For(x => x.ItemsSource)
					  .To(vm => vm.DosageHours);
			bindingSet.Apply();


            //Bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
		}

	}
}