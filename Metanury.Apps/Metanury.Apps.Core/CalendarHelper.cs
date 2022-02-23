using System;
using System.Collections.Generic;

namespace Metanury.Apps.Core
{
    public class CalendarHelper
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public DateTime FirstDate { get; set; }
        public DateTime LastDate { get; set; }

        public DateTime TargetDate { get; set; }

        public List<DateTime> List { get; set; } = new List<DateTime>();

        public CalendarHelper()
        {
            this.Year = DateTime.Now.Year;
            this.Month = DateTime.Now.Month;
            this.Day = 1;
            this.Set();
        }

        public CalendarHelper(int year, int month)
        {
            this.Year = year;
            this.Month = month;
            this.Day = 1;
            this.Set();
        }

        public void Set()
        {
            this.TargetDate = new DateTime(this.Year, this.Month, this.Day);
            this.FirstDate = this.TargetDate.FirstDay();
            this.LastDate = this.TargetDate.LastDay();
            int week = (int)this.FirstDate.DayOfWeek;
            this.FirstDate = this.FirstDate.AddDays(-week);
            week = (int)this.LastDate.DayOfWeek;
            this.LastDate = this.LastDate.AddDays(6 - week);
            this.List = new List<DateTime>();
            for (DateTime dt = this.FirstDate; dt.Date <= this.LastDate.Date; dt = dt.AddDays(1))
            {
                this.List.Add(dt);
            }
        }

        public bool IsCurrentDate(DateTime tmp)
        {
            if (tmp.Year == this.Year && tmp.Month == this.Month)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int NextYear
        {
            get
            {
                DateTime date = new DateTime(Year, Month, 1);
                return date.AddMonths(1).Year;
            }
        }

        public int NextMonth
        {
            get
            {
                DateTime date = new DateTime(Year, Month, 1);
                return date.AddMonths(1).Month;
            }
        }

        public int PreviousYear
        {
            get
            {
                DateTime date = new DateTime(Year, Month, 1);
                return date.AddMonths(-1).Year;
            }
        }

        public int PreviousMonth
        {
            get
            {
                DateTime date = new DateTime(Year, Month, 1);
                return date.AddMonths(-1).Month;
            }
        }
    }
}
