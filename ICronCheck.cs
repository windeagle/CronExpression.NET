namespace CronExpression.NET
{
    /// <summary>
    /// 用于“检查给定的值是否满足 Cron 表达式的对应部分所包含的条件”的接口
    /// </summary>
    public interface ICronCheck
    {
        /// <summary>
        /// 检查给定的值是否满足 Cron 表达式的对应部分所包含的条件
        /// </summary>
        /// <param name="datePartValue">日期时间类型的指定部分的值</param>
        /// <returns>是否满足 Cron 表达式条件</returns>
        bool Check(int datePartValue);
    }
}