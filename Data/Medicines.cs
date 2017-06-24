using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using Services;
using MvvmCross.Platform;

namespace Piller.Data
{
    [Table("ProduktLeczniczyOpakowanie")]
    public class Medicines
    {
        [PrimaryKey, Indexed]
        public long KodEAN { get; set; }

        public string NazwaProduktu { get; set; }
    }
}
