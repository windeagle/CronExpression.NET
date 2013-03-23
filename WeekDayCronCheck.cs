using System;
using System.Linq;

namespace CronExpression.NET
{
    /// <summary>
    ///   用于“检查给定的时间是否满足 Cron 表达式的星期部分所包含的条件”的 IDayCronCheck 实现类
    /// </summary>
    public class WeekDayCronCheck : BaseCronCheck, IDayCronCheck
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
        ///   检查给定的时间是否满足 Cron 表达式的星期部分所包含的条件
        /// </summary>
        /// <param name="dateTime"> 给定的时间值 </param>
        /// <returns> </returns>
        public bool Check(DateTime dateTime)
        {
            var datePartValue = (int) dateTime.DayOfWeek;
            return CheckWeekDay(datePartValue, dateTime);
        }

        #endregion

        /// <summary>
        ///   检查给定的时间是否满足 Cron 表达式的星期部分所包含的条件
        /// </summary>
        /// <param name="datePartValue"> 星期值 </param>
        /// <param name="dateTime"> 给定的时间值 </param>
        /// <returns> </returns>
        public bool CheckWeekDay(int datePartValue, DateTime dateTime)
        {
            var sArr = Exp.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(item => item.Trim());
            foreach (var s in sArr)
            {
                //先调用基类的基础条件检查
                if (BaseItemCheck(datePartValue, s))
                    return true;
                //是不是周六
                if (s.Equals("L"))
                {
                    if (datePartValue == 6)
                        return true;
                }
                //是不是本月第N周的第M天
                if (s.Contains('#'))
                {
                    var isArr = s.Split(new[] {'#'}).Select(item => item.Trim()).ToList();
                    if (isArr.Count != 2)
                        throw new Exception("cron 表达式格式不对");
                    int dayOfWeek, addweeks;
                    bool sbool = int.TryParse(isArr[0], out dayOfWeek);
                    bool tbool = int.TryParse(isArr[1], out addweeks);
                    //一个月不可能有第六周
                    if (!sbool || !tbool || addweeks == 0 || addweeks > 5)
                        throw new Exception("cron 表达式格式不对");
                    //cron的星期是从1开始的
                    dayOfWeek = dayOfWeek - 1;
                    if (dayOfWeek == datePartValue)
                    {
                        DateTime d1 = new DateTime(dateTime.Year, dateTime.Month, 1);
                        if (datePartValue >= (int) d1.DayOfWeek)
                            d1 = d1.AddDays(datePartValue - (int) d1.DayOfWeek).AddDays(7*addweeks - 7);
                        else
                            d1 = d1.AddDays(datePartValue - (int) d1.DayOfWeek).AddDays(7*addweeks);

                        //不能跨月
                        if (dateTime.Day == d1.Day && dateTime.Month == d1.Month)
                            return true;
                    }
                }
                //是不是本月最后一个星期几
                else if (s.EndsWith("L"))
                {
                    int dayOfWeek;
                    var dbool = int.TryParse(s.Trim('L'), out dayOfWeek);
                    if (!dbool)
                        throw new Exception("cron 表达式格式不对");
                    //cron的星期是从1开始的
                    dayOfWeek = dayOfWeek - 1;
                    if (dayOfWeek == datePartValue)
                    {
                        DateTime d1 = new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1).AddDays(-1);
                        if (datePartValue > (int) d1.DayOfWeek)
                            d1 = d1.AddDays(datePartValue - (int) d1.DayOfWeek).AddDays(-7);
                        else
                            d1 = d1.AddDays((int) d1.DayOfWeek - datePartValue);
                        if (dateTime.Day == d1.Day)
                            return true;
                    }
                }
            }
            return false;
        }
    }
}