using System;
namespace Piller.Data
{
    public class RepeatPattern
    {
		public DaysOfWeek DayOfWeek { get; set; }
		public RepetitionInterval Interval { get; set; }
		public int RepetitionFrequency { get; set; }

		public RepeatPattern()
		{
		}
    }

	public enum RepetitionInterval
	{
		None,
		Daily,
		Weekly,
		Monthly,
		Yearly,
		Decennially,
		Centennially,
		Millenially
	}
}
