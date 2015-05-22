using System;
using System.Collections.Generic;
using System.Linq;

namespace RicardoDeSilvaBoundedContext
{
    namespace Contracts
    {
        public enum ScheduleGeneratorType { Daily, Hourly /*,  Add others */ };

        public class DeliveryProvisioningDetails
        {
            public string Id { get; protected set; }
            public string Description { get; protected set; }
            public DateTime? DeliveryStartingFrom { get; protected set; }
            public ScheduleGeneratorType ScheduleGenerator { get; protected set; }

            public DeliveryProvisioningDetails(string id, string description, ScheduleGeneratorType scheduleGenerator, DateTime deliveryStartingFrom)
            {
                this.Id = id;
                this.Description = description;
                this.ScheduleGenerator = scheduleGenerator;
                this.DeliveryStartingFrom = deliveryStartingFrom;
            }
        }
    }

    namespace Modules
    {
        using Contracts;

        public interface IGenerateASchedule
        {
            IEnumerable<DateTime> StartingFrom(DateTime startDate);
        }

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

    namespace Repositories
    {
        public interface IQueryDeliveryState
        {
            // something
        }

        public interface IPersistDeliveryChanges
        {
            void DeliveryWasProvisioned(string Id, string Description, DateTime scheduledDate);
        }
    }


    namespace Domain
    {
        using Contracts;
        using Modules;
        using Repositories;

        public class Delivery
        {
            private IPersistDeliveryChanges changes;
            private IQueryDeliveryState state;

            public Delivery(IPersistDeliveryChanges changes, IQueryDeliveryState state)
            {
                this.changes = changes;
                this.state = state;
            }

            public void ProvisionDelivery(DeliveryProvisioningDetails provisioningDetails)
            {
                IGenerateASchedule scheduleGenerator;

                var scheduleGeneratorWasFound = ScheduleGenerators.TryGetGenerator(provisioningDetails.ScheduleGenerator, out scheduleGenerator);

                Guard.That(scheduleGeneratorWasFound, "Unknown schedule generator");

                // a later starting date might be picked for whatever reason (f.e. not in stock etc)
                var earliestDate = provisioningDetails.DeliveryStartingFrom.HasValue ? provisioningDetails.DeliveryStartingFrom.Value : DateTime.Now;

                // here other dates might be picked (f.e. national holiday)
                var scheduledDate = scheduleGenerator.StartingFrom(earliestDate).First();

                // everything is ok, persist
                changes.DeliveryWasProvisioned(provisioningDetails.Id, provisioningDetails.Description, scheduledDate);
            }
        }

    }

    public static class Guard
    {
        public static void Against(bool condition, string message)
        {
            if (condition)
            {
                throw new InvalidOperationException(message);
            }
        }

        public static void That(bool condition, string message)
        {
            Guard.Against(!condition, message);
        }
    }
}
