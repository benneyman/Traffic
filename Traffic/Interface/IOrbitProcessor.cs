using System.Collections.Generic;
using Traffic.Interface;

namespace Traffic.Interface
{
    public interface IOrbitProcessor
    {
        int GetOrbitTrafficSpeed(IOrbit orbit);
    }
}