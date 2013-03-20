using System;
using System.Globalization;
using System.Linq;

namespace CronExpression.NET
{
    public abstract class BaseCronCheck
    {
        /// <summary>
        /// Cron 表达式的七个部分中的某个部分的子表达式文本
        /// </summary>
        public string Exp { get; set; }

        /// <summary>
        /// 检查给定的值是否满足 Cron 表达式的对应部分所包含的基础条件
        /// </summary>
        /// <param name="datePartValue">日期时间类型的指定部分的值</param>
        /// <returns></returns>
        /// <remarks>
        /// 所谓基础条件，是指以下条件：
        /// 1.通配符条件
        /// 2.数值相等条件
        /// 3.取值范围条件
        /// 4.间隔值条件
        /// 5.以上四种条件以逗号组合
        /// 
        /// Cron 表达式的年、月、小时、分钟、秒五个部分只用到基础条件
        /// 日和星期除了基础条件外，还有其他更复杂的条件
        /// </remarks>
        public bool BaseCheck(int datePartValue)
        {
            //把子表达式用逗号分隔
            var sArr = Exp.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(item => item.Trim());
            //只要给定的值满足一项条件就返回true，逗号相当于OR逻辑运算符
            return sArr.Any(s => BaseItemCheck(datePartValue, s));
        }

        /// <summary>
        /// 检查给定的值是否满足某项基础条件
        /// </summary>
        /// <param name="datePartValue">日期时间类型的指定部分的值</param>
        /// <param name="itemExp">条件表达式，支持四种基础条件：通配符条件、数值相等条件、取值范围条件和间隔值条件</param>
        /// <returns></returns>
        public static bool BaseItemCheck(int datePartValue, string itemExp)
        {
            //通配符条件
            if (itemExp.Equals("*"))
                return true;
            //数值相等条件
            if (itemExp.Equals(datePartValue.ToString(CultureInfo.InvariantCulture)))
                return true;
            //取值范围条件
            if (itemExp.Contains('-'))
            {
                var isArr = itemExp.Split(new[] { '-' }).Select(item => item.Trim()).ToList();
                if (isArr.Count != 2)
                    throw new Exception("cron 表达式格式不对");
                int small, big;
                bool sbool = int.TryParse(isArr[0], out small);
                bool bbool = int.TryParse(isArr[1], out big);
                if (!sbool || !bbool)
                    throw new Exception("cron 表达式格式不对");
                //NOTE: 范围是全包含的范围
                return small <= datePartValue && big >= datePartValue;
            }
            //间隔值条件
            if (itemExp.Contains('/'))
            {
                var isArr = itemExp.Split(new[] { '/' }).Select(item => item.Trim()).ToList();
                if (isArr.Count != 2)
                    throw new Exception("cron 表达式格式不对");
                int small, stepSize;
                bool sbool = int.TryParse(isArr[0], out small);
                bool tbool = int.TryParse(isArr[1], out stepSize);
                if (!sbool || !tbool || stepSize == 0)
                    throw new Exception("cron 表达式格式不对");
                return small <= datePartValue && ((datePartValue - small) % stepSize) == 0;
            }
            return false;
        }
    }
}