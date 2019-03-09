using System.Collections.Generic;

namespace GameInterfaces
{
    public interface IStorage
    {
        void Store(string item);
        
        bool isStorageFull();
    }
}
