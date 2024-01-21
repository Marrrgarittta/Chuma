using ChumaClasses.Chuma;
using System.Collections.Generic;
using System.Text;

namespace Chuma.ChumaClasses
{
    internal interface IPaintInfo
    {
        StringBuilder PrintCurrentStateAllAgents();

        int GetColorAgent(Agent agent);

        Dictionary<string, (int, int, int)> GetCurrentPaintInfo();
    }
}