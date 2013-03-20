namespace CronExpression.NET
{
    /// <summary>
    /// 用于“检查给定的年份值是否满足 Cron 表达式的年份部分所包含的条件”的接口
    /// </summary>
    public class YearCronCheck : BaseCronCheck, ICronCheck
    {
        /// <summary>
        /// 检查给定的年份值是否满足 Cron 表达式的年份部分所包含的条件
        /// </summary>
        /// <param name="datePartValue"></param>
        /// <returns></returns>
        public bool Check(int datePartValue)
        {
            //在调用基类的基础条件检查之前先检查给定值的范围
            if (datePartValue < 1970 || datePartValue > 2099) return false;
            return BaseCheck(datePartValue);
        }
    }
}