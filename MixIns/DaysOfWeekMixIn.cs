using System;
using System.Collections.Generic;
using System.Globalization;
using Piller.Data;

namespace Piller.MixIns.DaysOfWeekMixIns
{
	public static class DaysOfWeekMixIn
	{
		public static bool AllSelected(this DaysOfWeek This)
		{
			return (This.HasFlag(DaysOfWeek.Monday)
					&& This.HasFlag(DaysOfWeek.Tuesday)
					&& This.HasFlag(DaysOfWeek.Wednesday)
					&& This.HasFlag(DaysOfWeek.Thursday)
					&& This.HasFlag(DaysOfWeek.Friday)
					&& This.HasFlag(DaysOfWeek.Saturday)
					&& This.HasFlag(DaysOfWeek.Sunday));
		}

		public static IEnumerable<DaysOfWeek> GetSelected(this DaysOfWeek This)
		{
			if (This.HasFlag(DaysOfWeek.Monday))
				yield return DaysOfWeek.Monday;
			if (This.HasFlag(DaysOfWeek.Tuesday))
				yield return DaysOfWeek.Tuesday;
			if (This.HasFlag(DaysOfWeek.Wednesday))
				yield return DaysOfWeek.Wednesday;
			if (This.HasFlag(DaysOfWeek.Thursday))
				yield return DaysOfWeek.Thursday;
			if (This.HasFlag(DaysOfWeek.Friday))
				yield return DaysOfWeek.Friday;
			if (This.HasFlag(DaysOfWeek.Saturday))
				yield return DaysOfWeek.Saturday;
			if (This.HasFlag(DaysOfWeek.Sunday))
				yield return DaysOfWeek.Sunday;
		}

		public static string DayOfWeekName(this DaysOfWeek This)
		{
			if (This == DaysOfWeek.Monday)
				return DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Monday);
			if (This == DaysOfWeek.Tuesday)
				return DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Tuesday);
			if (This == DaysOfWeek.Wednesday)
				return DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Wednesday);
			if (This == DaysOfWeek.Thursday)
				return DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Thursday);
			if (This == DaysOfWeek.Friday)
				return DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Friday);
			if (This == DaysOfWeek.Saturday)
				return DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Saturday);
			if (This == DaysOfWeek.Sunday)
				return DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Sunday);

			return null;
		}

		public static bool EqualsDaysOfWeek(this DayOfWeek This, DaysOfWeek day)
		{
			return Enum.GetName(typeof(DayOfWeek), This).ToLower() == Enum.GetName(typeof(DaysOfWeek), day).ToLower();
		}

		public static DaysOfWeek ToDaysOfWeek(this DayOfWeek dayOfWeek)
		{
			var dayOfWeekName = Enum.GetName(typeof(DayOfWeek), dayOfWeek);
			var result = (DaysOfWeek)Enum.Parse(typeof(DaysOfWeek), dayOfWeekName);
			return result;
		}

		public static int GetOrdinal(this DaysOfWeek This)
		{
			if (This == DaysOfWeek.Monday)
				return 0;
			if (This == DaysOfWeek.Tuesday)
				return 1;
			if (This == DaysOfWeek.Wednesday)
				return 2;
			if (This == DaysOfWeek.Thursday)
				return 3;
			if (This == DaysOfWeek.Friday)
				return 4;
			if (This == DaysOfWeek.Saturday)
				return 5;
			if (This == DaysOfWeek.Sunday)
				return 6;

			throw new ArgumentOutOfRangeException();
		}
	}
}
