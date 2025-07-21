using System;

namespace Vnexpress.Helpers
{
    public static class TimeHelper
    {
        public static string GetTimeAgo(DateTime? date, TimeSpan? time)
        {
            if (!date.HasValue || !time.HasValue)
                return "Không rõ thời gian";

            var createdDateTime = date.Value.Date + time.Value;
            var timeDiff = DateTime.Now - createdDateTime;

            if (timeDiff.TotalMinutes < 1)
                return "Vừa đăng";
            if (timeDiff.TotalMinutes < 60)
                return $"Đăng cách đây {Math.Floor(timeDiff.TotalMinutes)} phút";
            if (timeDiff.TotalHours < 24)
                return $"Đăng cách đây {Math.Floor(timeDiff.TotalHours)} giờ";

            return $"Đăng cách đây {Math.Floor(timeDiff.TotalDays)} ngày";
        }
    }
}
