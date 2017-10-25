namespace Assets.Scripts.Interfaces
{
    public interface IDamageable
    {
        float Damage { get; }

        void OnTakeDamage(float damageAmount);
    }
}
