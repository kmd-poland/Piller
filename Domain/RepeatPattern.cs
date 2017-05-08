using Piller.Data;
using System;
using System.Collections.Generic;

namespace Piller.Core.Domain
{
    public class RepeatPattern
    {
        public DaysOfWeek DayOfWeek { get; set; }
        public RepetitionInterval Interval { get; set; }
        public int RepetitionFrequency { get; set; } = 30000;
        public List<TimeSpan> Hours { get; set; }

        public RepeatPattern ()
        {
        }
    }
}
