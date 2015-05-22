using RicardoDeSilvaBoundedContext.Contracts;
using RicardoDeSilvaBoundedContext.Modules;
using RicardoDeSilvaBoundedContext.Repositories;
using System;
using System.Linq;

namespace RicardoDeSilvaBoundedContext.Domain
{
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
