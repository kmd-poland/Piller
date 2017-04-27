using System;
using SQLite;
using System.Collections.Generic;

namespace Piller.Data
{
    [Table("MEDICATION_DOSAGE")]
    public class MedicationDosage
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Dosage { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }


    }
}
