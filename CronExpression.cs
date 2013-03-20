using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CronExpression.NET
{
    /// <summary>
    ///   Cron 表达式类
    /// </summary>
    /// <remarks>
    ///   不支持"*/5"的写法，不支持"C"和"?",不支持星期和月份的英文缩写写法
    /// </remarks>
    public class CronExpression
    {
        private readonly string _cronExpressionString;
        private MonthDayCronCheck _daysOfMonth;
        private WeekDayCronCheck _daysOfWeek;
        private HourCronCheck _hours;
        private MinuteCronCheck _minutes;
        private MonthCronCheck _months;
        private SecondCronCheck _seconds;
        private YearCronCheck _years;

        public CronExpression(string cronExpression)
        {
            if (cronExpression == null)
                throw new ArgumentException("cronExpression cannot be null");
            _cronExpressionString = cronExpression.ToUpper(CultureInfo.InvariantCulture).Trim();
            BuildExpression(_cronExpressionString);
        }

        private void BuildExpression(string cronExpressionString)
        {
            char[] delimArr = new[]
                                  {
                                      ' ',
                                      '\t',
                                      '\r',
                                      '\n'
                                  };
            var sArr =
                cronExpressionString.Split(delimArr, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList
                    ();
            if (sArr.Count != 7)
            {
                cronExpressionString += " *";
                sArr =
                    cronExpressionString.Split(delimArr, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).
                        ToList();
                if (sArr.Count != 7)
                    throw new Exception("cron 表达式格式不对");
            }

            _seconds = new SecondCronCheck
                           {
                               Exp = sArr[0]
                           };
            _minutes = new MinuteCronCheck
                           {
                               Exp = sArr[1]
                           };
            _hours = new HourCronCheck
                         {
                             Exp = sArr[2]
                         };
            _daysOfMonth = new MonthDayCronCheck
                               {
                                   Exp = sArr[3]
                               };
            _months = new MonthCronCheck
                          {
                              Exp = sArr[4]
                          };
            _daysOfWeek = new WeekDayCronCheck
                              {
                                  Exp = sArr[5]
                              };
            _years = new YearCronCheck
                         {
                             Exp = sArr[6]
                         };
        }

        public IEnumerable<DateTime> GetNextDateTime(int count)
        {
            var ret = new List<DateTime>();
            var time = DateTime.Now;
            var i = 0;
            while (i < count && time < DateTime.Parse("2100-01-01"))
            {
                if (!_years.Check(time.Year))
                {
                    time = new DateTime(time.Year + 1, 1, 1);
                    continue;
                }
                if (!_months.Check(time.Month))
                {
                    time = new DateTime(time.Year, time.Month, 1).AddMonths(1);
                    continue;
                }
                if (!_daysOfMonth.Check(time) || !_daysOfWeek.Check(time))
                {
                    time = new DateTime(time.Year, time.Month, time.Day).AddDays(1);
                    continue;
                }
                if (!_hours.Check(time.Hour))
                {
                    time = new DateTime(time.Year, time.Month, time.Day, time.Hour, 0, 0).AddHours(1);
                    continue;
                }
                if (!_minutes.Check(time.Minute))
                {
                    time = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0).AddMinutes(1);
                    continue;
                }
                if (_seconds.Check(time.Second))
                {
                    ret.Add(time);
                    i++;
                }
                time = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second).AddSeconds(1);
            }
            return ret;
        }

        public bool IsSatisfiedBy(DateTime time)
        {
            return _years.Check(time.Year) &&
                   _months.Check(time.Month) &&
                   _daysOfMonth.Check(time) &&
                   _daysOfWeek.Check(time) &&
                   _hours.Check(time.Hour) &&
                   _minutes.Check(time.Minute) &&
                   _seconds.Check(time.Second);
        }
    }
}