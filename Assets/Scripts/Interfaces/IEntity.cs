using Assets.Scripts.Blocks;

namespace Assets.Scripts.Interfaces
{
    public interface IEntity
    {
        Block Block { get; set; }

        void Draw();
    }
}
