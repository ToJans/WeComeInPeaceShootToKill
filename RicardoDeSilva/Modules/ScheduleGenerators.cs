using RicardoDeSilvaBoundedContext.Contracts;
using System;
using System.Collections.Generic;

namespace RicardoDeSilvaBoundedContext.Modules
{
    // will probably not be a static class, probably depends on the context
    public static class ScheduleGenerators
    {
        public static bool TryGetGenerator(ScheduleGeneratorType @type, out IGenerateASchedule result)
        {
            switch (@type)
            {
                case (ScheduleGeneratorType.Daily):
                    result = new DailyScheduleGenerator();
                    return true;
                case (ScheduleGeneratorType.Hourly):
                    result = new HourlyScheduleGenerator();
                    return true;
                default:
                    result = null;
                    return false;
            }
        }

        private class HourlyScheduleGenerator : IGenerateASchedule
        {
            IEnumerable<DateTime> IGenerateASchedule.StartingFrom(DateTime startDate)
            {
                var currentDate = startDate;
                while (true)
                {
                    // this will probably be way more complicated
                    if (currentDate.TimeOfDay.TotalHours >= 8.5 &&
                       currentDate.TimeOfDay.TotalHours < 17)
                    {
                        yield return currentDate;
                    }
                    currentDate += TimeSpan.FromHours(1);
                }
            }
        }

        private class DailyScheduleGenerator : IGenerateASchedule
        {
            IEnumerable<DateTime> IGenerateASchedule.StartingFrom(DateTime startDate)
            {
                var currentDate = startDate;
                while (true)
                {
                    // this will probably be way more complicated
                    if (currentDate.DayOfWeek != DayOfWeek.Saturday &&
                        currentDate.DayOfWeek != DayOfWeek.Sunday)
                    {
                        yield return currentDate;
                    }
                    currentDate += TimeSpan.FromDays(1);
                }
            }
        }
    }
}
