using System;
using System.Linq;

namespace CronExpression.NET
{
    /// <summary>
    ///   用于“检查给定的时间是否满足 Cron 表达式的日期部分所包含的条件”的 IDayCronCheck 实现类
    /// </summary>
    public class MonthDayCronCheck : BaseCronCheck, IDayCronCheck
    {
        #region IDayCronCheck Members

        /// <summary>
        ///   屏蔽了 ICronCheck 接口中的方法
        /// </summary>
        /// <param name="datePartValue"> </param>
        /// <returns> </returns>
        public bool Check(int datePartValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   检查给定的时间是否满足 Cron 表达式的日期部分所包含的条件
        /// </summary>
        /// <param name="dateTime"> 给定的时间值 </param>
        /// <returns> </returns>
        public bool Check(DateTime dateTime)
        {
            var datePartValue = dateTime.Day;
            return CheckMonthDay(datePartValue, dateTime);
        }

        #endregion

        /// <summary>
        ///   检查给定的时间是否满足 Cron 表达式的日期部分所包含的条件
        /// </summary>
        /// <param name="datePartValue"> 日期值 </param>
        /// <param name="dateTime"> 给定的时间值 </param>
        /// <returns> </returns>
        public bool CheckMonthDay(int datePartValue, DateTime dateTime)
        {
            var sArr = Exp.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(item => item.Trim());
            foreach (var s in sArr)
            {
                //先调用基类的基础条件检查
                if (BaseItemCheck(datePartValue, s))
                    return true;
                //是不是当月的最后一天
                if (s.Equals("L"))
                {
                    DateTime d1 = new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1).AddDays(-1);
                    if (datePartValue == d1.Day)
                        return true;
                }
                //是不是当月的最后一个非周末
                if (s.Equals("LW"))
                {
                    DateTime d1 = new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1).AddDays(-1);
                    var lastDay = d1.DayOfWeek;
                    switch (lastDay)
                    {
                        case DayOfWeek.Sunday:
                            d1 = d1.AddDays(-2);
                            break;
                        case DayOfWeek.Saturday:
                            d1 = d1.AddDays(-1);
                            break;
                    }
                    if (datePartValue == d1.Day)
                        return true;
                }
                //是不是当月的第N个非周末
                else if (s.EndsWith("W"))
                {
                    int day;
                    var dbool = int.TryParse(s.Trim('W'), out day);
                    if (!dbool)
                        throw new Exception("cron 表达式格式不对");
                    DateTime d1 = new DateTime(dateTime.Year, dateTime.Month, 1).AddDays(day - 1);
                    //不能跨月份的
                    if (d1.Month == dateTime.Month)
                    {
                        var lastDay = d1.DayOfWeek;
                        switch (lastDay)
                        {
                            case DayOfWeek.Sunday:
                                d1 = d1.AddDays(1);
                                break;
                            case DayOfWeek.Saturday:
                                d1 = d1.AddDays(-1);
                                break;
                        }
                        if (d1.Month > dateTime.Month)
                            d1 = d1.AddDays(-3);
                        else if (d1.Month < dateTime.Month)
                            d1 = d1.AddDays(3);
                        if (datePartValue == d1.Day)
                            return true;
                    }
                }
            }
            return false;
        }
    }
}