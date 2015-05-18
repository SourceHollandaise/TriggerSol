using System;

namespace TriggerSol.JStore
{
    public class GuidIdGenerator : IMappingIdGenerator
    {
        public object GetId()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}