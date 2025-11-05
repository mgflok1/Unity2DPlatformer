using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    // Максимальное здоровье
    [SerializeField] private float maxHealth = 1f;

    private float currentHealth;
    public event UnityAction OnDeath;
    // Событие изменения здоровья
    public event UnityAction<float> OnHealthChanged;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    // Получение урона
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth / maxHealth);

        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    // Сброс здоровья
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(1f);
    }
}