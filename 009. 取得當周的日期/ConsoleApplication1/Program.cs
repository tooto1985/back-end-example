using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            foreach (var s in GetSameWeekDateTimes(new DateTime(2014,7,25)))
            {
                Console.WriteLine(s);
            }
            Console.ReadLine();
        }

        public static int GetWeeks(DateTime dateTime)
        {
            var dfi = DateTimeFormatInfo.CurrentInfo;
            var cal = dfi.Calendar;
            return cal.GetWeekOfYear(dateTime, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }

        public static List<DateTime> GetSameWeekDateTimes(DateTime dateTime)
        {
            var list = new List<DateTime>();
            var weeks = GetWeeks(dateTime);
            for (var i = 0; i < 2; i++)
            {
                var n = i;
                while (GetWeeks(dateTime.AddDays(n)) == weeks)
                {
                    list.Add(dateTime.AddDays(n));
                    n = i < 1 ? n - 1 : n + 1;
                }
            }
            return list.OrderByDescending(x => x).ToList();
        }
    }

    
}
