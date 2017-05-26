using System;
using SQLite;

namespace Piller.Data
{
    [Table("NOTIFICATION_OCCURRENCE")]
    public class NotificationOccurrence
    {
		[PrimaryKey, AutoIncrement]
		public int? Id { get; set; }

        public int MedicationDosageId { get; set; }

        public long OccurrenceDateMillis { get; set; }

        public DateTime OccurrenceDateTime { get; set; }

        public NotificationOccurrence()
        {

        }

        public NotificationOccurrence(int medicationDosageId, DateTime occurrenceDate, long occurrenceDateMillis)
        {
            this.MedicationDosageId = medicationDosageId;
            this.OccurrenceDateTime = occurrenceDate;
            this.OccurrenceDateMillis = occurrenceDateMillis;
        }
    }
}
