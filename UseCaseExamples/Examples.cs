using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using BC = RicardoDeSilvaBoundedContext;

namespace UseCaseExamples
{
    [TestClass]
    public class Examples
    {
        [TestMethod]
        public void A_daily_appointement_for_a_thursday_should_return_that_date()
        {
            var deliveryDetails = 
                new BC.Contracts.DeliveryProvisioningDetails(
                    a_delivery_id,
                    a_delivery_description,
                    BC.Contracts.ScheduleGeneratorType.Daily,
                    a_thursday
                );
            
            this.SUT.ProvisionDelivery(deliveryDetails);

            this.Changes.Verify(x => x.DeliveryWasProvisioned(a_delivery_id, a_delivery_description, a_thursday));
        }

        [TestMethod]
        public void A_daily_appointement_for_a_saturday_should_return_the_next_monday()
        {
            var deliveryDetails =
                new BC.Contracts.DeliveryProvisioningDetails(
                    a_delivery_id,
                    a_delivery_description,
                    BC.Contracts.ScheduleGeneratorType.Daily,
                    a_saturday
                );

            this.SUT.ProvisionDelivery(deliveryDetails);

            this.Changes.Verify(x => x.DeliveryWasProvisioned(a_delivery_id, a_delivery_description, a_saturday.AddDays(2)));
        }

        private BC.Domain.Delivery SUT;
        private Mock<BC.Repositories.IPersistDeliveryChanges> Changes;
        private Mock<BC.Repositories.IQueryDeliveryState> Queries;

        const string a_delivery_id = "delivery/1";
        const string a_delivery_description = "A delivery description";
        DateTime a_thursday = new DateTime(2015, 01, 01);
        DateTime a_saturday = new DateTime(2015, 01, 03);

        [TestInitialize]
        public void Setup()
        {
            this.Changes = new Mock<BC.Repositories.IPersistDeliveryChanges>();
            this.Queries = new Mock<BC.Repositories.IQueryDeliveryState>();
            this.SUT = new BC.Domain.Delivery(this.Changes.Object, this.Queries.Object);
        }

    }
}
