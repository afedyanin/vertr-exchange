using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vertr.ExchCore.Application.Subscribers.Enums
{
    internal enum GroupPriority
    {
        PreProcessing = 0, // Journaling, Replication
        Processing = 1,
        PostProcessing = 2, // Posing events, post journaling, cleanup 
    }
}
