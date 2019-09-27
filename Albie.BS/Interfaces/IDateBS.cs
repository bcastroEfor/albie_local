using System;
using System.Collections.Generic;
using Albie.Models;

namespace Albie.BS.Interfaces
{
    public interface IDateBS
    {
        IEnumerable<LabelAndValue<int>> GetWeeks(int year = -1);
        IEnumerable<LabelAndValue<DateTime>> GetDaysInMonth(int year = -1, int month = -1);
        IEnumerable<Weeks> GetWeeksInMonth(int year = -1, int month = -1);
        Dictionary<int, string> GetMonthsInYear();
    }
}