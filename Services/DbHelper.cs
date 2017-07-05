using System;
using System.Threading.Tasks;
using MvvmCross.Platform;
using Piller.Data;
using Piller.MixIns.DaysOfWeekMixIns;

namespace Piller.Services
{
    public static class DbHelper
    {
        private static IPermanentStorageService storage = Mvx.Resolve<IPermanentStorageService>();

		public static async Task AddNotificationOccurrences(MedicationDosage medDosage)
		{
			if (medDosage.Days.AllSelected())
			{
				// schedule for every occurrence of hour for every 24 hours
				foreach (var hour in medDosage.HoursEncoded.Split(';'))
				{
					var occurrence = new NotificationOccurrence()
					{
						Name = medDosage.Name,
						Dosage = medDosage.Dosage,
						MedicationDosageId = medDosage.Id.Value,
						ThumbnailImage = medDosage.ThumbnailName,
						OccurrenceDateTime = NextOccurrenceFromHour(TimeSpan.Parse(hour))
					};

					await storage.SaveAsync(occurrence);
				}
			}
			else
			{
				// schedule in a weekly manner for each day of week
				foreach (var hour in medDosage.HoursEncoded.Split(';'))
				{
					foreach (var day in medDosage.Days.GetSelected())
					{
						var occurrence = new NotificationOccurrence()
						{
							Name = medDosage.Name,
							Dosage = medDosage.Dosage,
							MedicationDosageId = medDosage.Id.Value,
							ThumbnailImage = medDosage.ThumbnailName,
							OccurrenceDateTime = NextOccurrenceFromHour(TimeSpan.Parse(hour))
						};

						await storage.SaveAsync(occurrence);
					}
				}
			}
		}

		private static DateTime NextOccurrenceFromHour(TimeSpan hour)
		{
			var occurrenceDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour.Hours, hour.Minutes, 0);
			if (DateTime.Now.Hour > hour.Hours)
				return occurrenceDate.AddDays(1);
			return occurrenceDate;
		}
    }
}
