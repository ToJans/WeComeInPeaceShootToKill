using System;

namespace RicardoDeSilvaBoundedContext.Domain
{
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
