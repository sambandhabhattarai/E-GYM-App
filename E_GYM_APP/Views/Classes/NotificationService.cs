using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_GYM_APP.Views.Classes
{
    public class NotificationService
    {
        public void ScheduleDailyWorkoutNotification()
        {
            var now = DateTime.Now;
            var notifyTime = new DateTime(now.Year, now.Month, now.Day, 11, 25, 0);


            if (notifyTime < now)
            {
                notifyTime = notifyTime.AddDays(1);
            }

            var notificationRequest = new NotificationRequest
            {
                NotificationId = 1001,
                Title = "Daily Workout Reminder",
                Description = "It's time for your daily workout! Let's stay fit!",
                Schedule =
            {
                NotifyTime = notifyTime,
                RepeatType = NotificationRepeat.Daily
            }
            };
            LocalNotificationCenter.Current.Show(notificationRequest);
        }
    }
}
