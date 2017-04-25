using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piller.ViewModels
{
    public class DataMedicationDosage
    {
        public string MedicationName { get; set; }
        public string MedicationDosage { get; set; }
        public string Time { get; set; }
        public int ID { get; set; }
        public DataMedicationDosage(string medicationName,int hour,int minute,int id,string medicationDosage)
        {
            MedicationName = medicationName;
            MedicationDosage = medicationDosage;
            Time = $"{hour}:{minute}";
            ID = id;
        }
    }
}
