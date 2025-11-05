using UnityEngine.Events;

public interface IPlayerHealth
{
    float CurrentHealth { get; }
    float MaxHealth { get; }
    void TakeDamage(float damage);
    void ResetHealth();
    event UnityAction<float> OnHealthChanged; // Аргумент: нормализованное значение (0-1)
    event UnityAction OnDeath;
}