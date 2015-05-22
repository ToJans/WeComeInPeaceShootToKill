using System;

namespace RicardoDeSilvaBoundedContext.Repositories
{
    public interface IPersistDeliveryChanges
    {
        void DeliveryWasProvisioned(string Id, string Description, DateTime scheduledDate);
    }
}
