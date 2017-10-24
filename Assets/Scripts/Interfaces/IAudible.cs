using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Interfaces
{
    public interface IAudible
    {
        string SoundWalkedOnAsset { get; }

        string SoundBeingMinedAsset { get; }

        string SoundBlockBrokenAsset { get; }

        string SoundBlockPlacedAsset { get; }
    }
}
