namespace CronExpression.NET
{
    /// <summary>
    /// 用于“检查给定的月份值是否满足 Cron 表达式的月份部分所包含的条件”的 ICronCheck 实现类
    /// </summary>
    public class MonthCronCheck : BaseCronCheck, ICronCheck
    {
        /// <summary>
        /// 检查给定的月份值是否满足 Cron 表达式的月份部分所包含的条件
        /// </summary>
        /// <param name="datePartValue"></param>
        /// <returns></returns>
        public bool Check(int datePartValue)
        {
            //在调用基类的基础条件检查之前先检查给定值的范围
            if (datePartValue < 1 || datePartValue > 12) return false;
            return BaseCheck(datePartValue);
        }
    }
}
