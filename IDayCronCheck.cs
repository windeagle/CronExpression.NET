using System;

namespace CronExpression.NET
{
    /// <summary>
    /// 用于“检查给定的时间是否满足 Cron 表达式的日期或者星期部分所包含的条件”的接口
    /// </summary>
    /// <remarks>由于日期和星期的条件判断比较复杂，只传入 datePartValue 是不够的，需要传入整个时间值</remarks>
    public interface IDayCronCheck : ICronCheck
    {
        /// <summary>
        /// 检查给定的时间是否满足 Cron 表达式的日期或者星期部分所包含的条件
        /// </summary>
        /// <param name="dateTime">给定的时间值</param>
        /// <returns></returns>
        bool Check(DateTime dateTime);
    }
}