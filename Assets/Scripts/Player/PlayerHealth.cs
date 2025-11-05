using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class PlayerHealth : IPlayerHealth
{
    private float _currentHealth;
    private readonly float _maxHealth;

    // Событие изменения здоровья
    public event UnityAction<float> OnHealthChanged;
    // Событие смерти
    public event UnityAction OnDeath;

    // Текущее здоровье
    public float CurrentHealth => _currentHealth;
    // Максимальное здоровье
    public float MaxHealth => _maxHealth;

    // Инъекция audioManager
    [Inject] private IAudioManager audioManager;

    // Конструктор с инъекцией
    [Inject]
    public PlayerHealth()
    {
        _maxHealth = 100f;
        _currentHealth = _maxHealth;
    }

    // Получение урона
    public void TakeDamage(float damage)
    {
        if (_currentHealth <= 0) return;

        _currentHealth = Mathf.Max(0, _currentHealth - damage);
        OnHealthChanged?.Invoke(_currentHealth / _maxHealth);

        audioManager.PlaySound("PlayerDamage");

        if (_currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    // Сброс здоровья
    public void ResetHealth()
    {
        _currentHealth = _maxHealth;
        OnHealthChanged?.Invoke(1f);
    }
}