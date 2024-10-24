using System;
using GameFramework;
namespace GameLogic
{
    public class TimeUtil
    {
        /// <summary>
        /// 返回的日期格式
        /// </summary>
        public enum TimeType
        {
            天,
            时,
            分,
            秒
        }

        /// <summary>
        /// 根据秒数返回时间格式
        /// </summary>
        /// <param name="DiffTiem">秒数</param>
        /// <param name="type">返回日期格式的类型</param>
        /// <returns></returns>
        public static string GetTimeFormat(long DiffTiem, TimeType type = TimeType.时)
        {
            string formatStr = "";
            string time = "";
            int Hours = (int)DiffTiem / 3600;           // 取出小时
            int Minute = (int)(DiffTiem % 3600) / 60;   // 取出分钟
            int Second = (int)DiffTiem % 60;            // 取出秒数
            switch (type)
            {
                case TimeType.时:
                    time = string.Format("{0:D2}：{1:D2}：{2:D2}", Hours, Minute, Second);//装箱版本
                    break;
                case TimeType.分:
                    time = string.Format("{0:D2}：{1:D2}", Minute, Second);//装箱版本
    
                    break;
                case TimeType.秒:
                    time = string.Format("{0:D2}", Second);//装箱版本
                    break;
            }
            return time;
        }

        /// <summary>
        /// 把时间转化成字符串x分x秒
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>字符串00:00:00</returns>
        public static string TransTimeSecondToString(float second)
        {
            string str = "";
            try
            {
                long min = (long)second / 60;
                long sec = (long)second % 60;
                if (min > 0)
                {
                    str += min + "分";
                }
                str += sec + "秒";
            }
            catch 
            {
            }
            return str;
        }
        /// <summary>
        /// 把时间转化成字符串x:x
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>字符串00:00</returns>
        public static string TransTimeSecond(float second)
        {
            string sHour = String.Empty;
            string sMin = String.Empty;
            string sSec = String.Empty;
            try
            {
                long hour = (long)second / 3600;
                long min = (long)second / 60;
                long sec = (long)second % 60;

               
                sHour = hour >= 1 ? hour + ":" : "";
                sMin = min > 0 ? "0" + min + ":" : "00:";
                sMin = min >= 10 ? min + ":" : sMin;
                if (hour >= 1 )
                {
                    if (min % 60 < 10)
                    {
                        sMin = "0" + min % 60 + ":";
                    }
                    else
                    {
                        sMin = min % 60 + ":";
                    }
                }
                sSec = sec < 10 ? "0" + sec : sec.ToString();

            }
            catch 
            {
            }
            return sHour + sMin + sSec;
        }

        /// <summary>
        ///   long格式化为00
        /// </summary>
        private static string FormatterLongTo00(float value)
        {
            string str = "";

            try
            {
                if (value < 10)
                    str += "0" + value.ToString();
                else
                    str += value.ToString();
            }
            catch 
            {
               
            }

            return str;
        }
        
    }
}
