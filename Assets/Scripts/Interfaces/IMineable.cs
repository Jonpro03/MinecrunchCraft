using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Interfaces
{
    public interface IMineable
    {
        uint MiningDifficulty { get; }

        void OnDestroyed();
    }
}
