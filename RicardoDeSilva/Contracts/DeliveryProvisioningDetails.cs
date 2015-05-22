using System;

namespace RicardoDeSilvaBoundedContext.Contracts
{
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
