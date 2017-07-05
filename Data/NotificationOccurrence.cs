using System;
using SQLite;

namespace Piller.Data
{
    [Table("NOTIFICATION_OCCURRENCE")]
    public class NotificationOccurrence
    {
		[PrimaryKey, AutoIncrement]
		public int? Id { get; set; }

        public string Name { get; set; }

        public string Dosage { get; set; }

        public int MedicationDosageId { get; set; }

        public long OccurrenceDateMillis { get; set; }

        public DateTime OccurrenceDateTime { get; set; }


        public string ThumbnailImage { get; set; }
        public NotificationOccurrence()
        {

        }

        public NotificationOccurrence(string name, string dosage, int medicationDosageId, DateTime occurrenceDate, long occurrenceDateMillis)
        {
            this.Name = name;
            this.Dosage = dosage;
            this.MedicationDosageId = medicationDosageId;
            this.OccurrenceDateTime = occurrenceDate;
            this.OccurrenceDateMillis = occurrenceDateMillis;
        }
    }
}
