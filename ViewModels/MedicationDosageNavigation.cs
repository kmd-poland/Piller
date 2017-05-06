using System;
namespace Piller.ViewModels
{
    public class MedicationDosageNavigation
    {

        public const int NewRecord = -1;
        //set -1 for new record
        public int MedicationDosageId { get; set; } = NewRecord;
    }
}
