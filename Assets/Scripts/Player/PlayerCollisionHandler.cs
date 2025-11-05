using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCollisionHandler : MonoBehaviour
{
    // Настройки коллизий
    [Header("Collision Settings")]
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private float damageCooldown = 0.5f;

    // Инъекции зависимостей
    [Inject] private IGameStateManager gameStateManager;
    [Inject] private IPlayerHealth playerHealth;

    private Rigidbody2D rb;
    private bool canTakeEnemyDamage = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D не найден!");
            enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameStateManager.IsGameOver || playerHealth.CurrentHealth <= 0) return;

        if (other.TryGetComponent<ICollectible>(out var collectible))
        {
            collectible.Collect();
            return;
        }

        if (other.CompareTag("BottomBoundary"))
        {
            playerHealth.TakeDamage(100f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameStateManager.IsGameOver) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy == null) return;

            // Если игрок выше врага
            if (transform.position.y > collision.transform.position.y + 1f)
            {
                IEnemy iEnemy = enemy;
                iEnemy.Die();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * 0.5f);
                return;
            }

            // Урон только если cooldown позволяет 
            if (canTakeEnemyDamage)
            {
                float damage = enemy.DamageOnContact > 0 ? enemy.DamageOnContact : 50f;
                if (damage < 0) return;
                playerHealth.TakeDamage(damage);

                // Запуск cooldown
                CooldownAsync().Forget();
            }
        }
    }

    // Cooldown
    private async UniTask CooldownAsync()
    {
        canTakeEnemyDamage = false;
        await UniTask.Delay(TimeSpan.FromSeconds(damageCooldown), ignoreTimeScale: false);
        canTakeEnemyDamage = true;
    }
}