namespace CronExpression.NET
{
    /// <summary>
    ///   用于“检查给定的小时值是否满足 Cron 表达式的小时部分所包含的条件”的 ICronCheck 实现类
    /// </summary>
    public class HourCronCheck : BaseCronCheck, ICronCheck
    {
        #region ICronCheck Members

        /// <summary>
        ///   检查给定的小时值是否满足 Cron 表达式的小时部分所包含的条件
        /// </summary>
        /// <param name="datePartValue"> 小时值 </param>
        /// <returns> </returns>
        public bool Check(int datePartValue)
        {
            //在调用基类的基础条件检查之前先检查给定值的范围
            if (datePartValue < 0 || datePartValue > 23) return false;
            return BaseCheck(datePartValue);
        }

        #endregion
    }
}