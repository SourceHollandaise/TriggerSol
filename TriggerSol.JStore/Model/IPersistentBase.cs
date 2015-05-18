using System.Collections.Generic;

namespace TriggerSol.JStore
{
    public interface IPersistentBase
    {
        object MappingId { get; set; }

        void Save(bool allowSaving = true);

        void Delete();

        IPersistentBase Clone(bool withId = false);

        IPersistentBase Reload();

        void Initialize();

        IList<T> GetAssociatedCollection<T>(string associatedProperty) where T: IPersistentBase;
    }
}
