using UnityEngine;
using Assets.Scripts.Blocks;

namespace Assets.Scripts.Interfaces
{
    public interface IBlock
    {
        BlockIdentification BlockId { get; }

        Vector3 PositionInChunk { get; }

        Vector3 PositionInWorld { get; }

        bool IsGravityAffected { get; }

        object Clone();
    }
}
