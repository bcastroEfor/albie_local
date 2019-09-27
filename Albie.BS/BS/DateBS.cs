using Albie.BS.Interfaces;
using Albie.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Albie.BS
{
    public class DateBS : IDateBS
    {

        public static string dateToStringFormat
        {
            get
            {
                return "dd-MM-yyyy";
            }
        }

        
        public IEnumerable<LabelAndValue<int>> GetWeeks(int year = -1)
        {
            if (year == -1) year = DateTime.UtcNow.Year;

            var maxWeek = GetWeeksInYear(year);

            if (CheckingFirstDateOfWeekIsInYear(year) == false)
            {
                maxWeek++;
            }

            int[] weeks = Enumerable.Range(0, maxWeek).ToArray();
            var lista = weeks.Select(w => new LabelAndValue<int>(
                FirstDateOfWeekISO8601(year, w).ToString(dateToStringFormat), w));
            GetDaysInMonth();
            return lista;
        }

        public IEnumerable<LabelAndValue<DateTime>> GetDaysInMonth(int year = -1, int month = -1)
        {
            if (year == -1) year = DateTime.UtcNow.Year;
            if (month == -1) month = DateTime.UtcNow.Month;
            List<DateTime> dates = Enumerable.Range(1, DateTime.DaysInMonth(year, month))  // Days: 1, 2 ... 31 etc.
                             .Select(day => new DateTime(year, month, day)) // Map each day to a date
                             .ToList(); // Load dates into a list
            return dates.Select(o => new LabelAndValue<DateTime>(o.ToString("dd-MMM dddd"), o, o));
        }

        public IEnumerable<Weeks> GetWeeksInMonth(int year = -1, int month = -1)
        {
            if (year == -1) year = DateTime.UtcNow.Year;
            if (month == -1) month = DateTime.UtcNow.Month;
            var calendar = CultureInfo.CurrentCulture.Calendar;
            var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            IEnumerable<Weeks> weekPeriods = Enumerable.Range(1, calendar.GetDaysInMonth(year, month))
                      .Select(d =>
                      {
                          var date = new DateTime(year, month, d);
                          var weekNumInYear = calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, firstDayOfWeek);
                          return new { date, weekNumInYear };
                      })
                      .GroupBy(x => x.weekNumInYear)
                      .Select(x => new Weeks
                      {
                          From = x.First().date,
                          To = x.Last().date
                      })
                      .ToList();
            return weekPeriods;
        }

        public Dictionary<int, string> GetMonthsInYear()
        {
            return Enumerable.Range(1, 12).Select(i => new
            {
                Index = i,
                MonthName = new CultureInfo("es-ES").DateTimeFormat.GetMonthName(i)
            }).ToDictionary(x => x.Index, x => x.MonthName);
        }

        public static int GetWeeksInYear(int year, CalendarWeekRule rule = CalendarWeekRule.FirstFourDayWeek, DayOfWeek firstDayOfWeek = DayOfWeek.Monday)
        {
            return GetWeekbyDate(new DateTime(year, 12, 31), rule, firstDayOfWeek);
        }

        public static int GetWeekbyDate(DateTime date, CalendarWeekRule rule = CalendarWeekRule.FirstFourDayWeek, DayOfWeek firstDayOfWeek = DayOfWeek.Monday)
        {
            return DateTimeFormatInfo.CurrentInfo.Calendar.GetWeekOfYear(date, rule, firstDayOfWeek);
        }

        public static bool CheckingFirstDateOfWeekIsInYear(int year, CalendarWeekRule rule = CalendarWeekRule.FirstFourDayWeek, DayOfWeek firstDayOfWeek = DayOfWeek.Monday)
        {
            DateTime firstWeekDay = FirstDateOfWeekISO8601NotCheking(year, 0, rule, firstDayOfWeek);
            return (firstWeekDay.AddDays(6).Year != year);
        }

        public static DateTime FirstDateOfWeekISO8601NotCheking(int year, int weekOfYear, CalendarWeekRule rule = CalendarWeekRule.FirstFourDayWeek, DayOfWeek firstDayOfWeek = DayOfWeek.Monday)
        {

            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, rule, firstDayOfWeek);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }

            var result = firstThursday.AddDays(weekNum * 7).AddDays(-3);
            return result;
        }

        public static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear, CalendarWeekRule rule = CalendarWeekRule.FirstFourDayWeek, DayOfWeek firstDayOfWeek = DayOfWeek.Monday, bool checkFirstWeek = true)
        {
            //check first week
            bool firstWeekOnYear = CheckingFirstDateOfWeekIsInYear(year, rule, firstDayOfWeek);
            if (firstWeekOnYear && checkFirstWeek)
            {
                return FirstDateOfWeekISO8601(year, weekOfYear + 1, rule, firstDayOfWeek, checkFirstWeek: false);
            }
            else
            {
                return FirstDateOfWeekISO8601NotCheking(year, weekOfYear, rule, firstDayOfWeek);
            }
        }
        
    }
}
