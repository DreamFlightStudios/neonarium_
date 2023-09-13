namespace HealthSystem
{
    public interface IHealthSystem
    {
        void ApplyDamage(byte damage);

        void Recovery(byte health);

        void Revival();
    }
}