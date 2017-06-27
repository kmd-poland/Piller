using System;
using SQLite;

namespace Piller.Data
{
    [Table("OVERDUE_NOTIFICATION")]
    public class OverdueNotification
    {
        [PrimaryKey, AutoIncrement]
        public int? Id { get; set; }

        public string Name { get; set; }

        public int MedicationDosageId { get; set; }

        public long OccurrenceDateMillis { get; set; }

        public OverdueNotification()
        {

        }

        public OverdueNotification(int medicationDosageId,  string Name, long occurrenceDateMillis)
        {
            this.MedicationDosageId = medicationDosageId;
            this.Name = Name;
            this.OccurrenceDateMillis = occurrenceDateMillis;
        }
    }
}
