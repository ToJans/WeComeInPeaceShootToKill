using System;
using System.Collections.Generic;

namespace RicardoDeSilvaBoundedContext.Modules
{
    public interface IGenerateASchedule
    {
        IEnumerable<DateTime> StartingFrom(DateTime startDate);
    }
}
