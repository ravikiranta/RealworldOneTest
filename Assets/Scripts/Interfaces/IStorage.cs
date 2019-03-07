using System.Collections.Generic;

namespace GameInterfaces
{
    public interface IStorage
    {
        void Store(List<string> items);
        List<string> Retrieve();
    }
}
