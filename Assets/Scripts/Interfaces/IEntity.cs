namespace Assets.Scripts.Interfaces
{
    public interface IEntity
    {
        IBlock Block { get; set; }

        void Draw();
    }
}
