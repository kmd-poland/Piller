using System;
namespace Piller.ViewModels
{
    public class MedicationDosageNavigation
    {
<<<<<<< HEAD
        public int MedicationDosageId { get; set; }
        public bool Edit { get; set; }
=======

        public const int NewRecord = -1;
        //set -1 for new record
        public int MedicationDosageId { get; set; } = NewRecord;
>>>>>>> origin/develop
    }
}
