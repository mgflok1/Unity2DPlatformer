using UnityEngine;
using Zenject;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerAttack : MonoBehaviour, IPlayerAttack
{
    // Настройки атаки
    [Header("Attack Settings")]
    [SerializeField] private float attackDamage = 50f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackWidth = 0.5f;
    [SerializeField] private LayerMask enemyLayer;

    // Смещения и вариации
    [Header("Offsets and Variations")]
    [SerializeField] private Vector2 attackOffset = new Vector2(0.5f, 0f);

    // Инъекции зависимостей
    [Inject] private IGameStateManager gameStateManager;
    [Inject] private IAudioManager audioManager;

    private CharacterMovement characterMovement;
    private readonly Collider2D[] hits = new Collider2D[10];

    private void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        if (characterMovement == null)
        {
            Debug.LogError("Компонент CharacterMovement не найден!");
            enabled = false;
        }
    }

    // Выполнение атаки
    public void PerformAttack()
    {
        if (gameStateManager.IsGameOver) return;

        // Вычисление позиции и размера box для overlap
        Vector2 attackPosition = (Vector2)transform.position + (characterMovement.IsFacingRight ? attackOffset : -attackOffset);
        Vector2 attackSize = new Vector2(attackRange, attackWidth);

        // Non-alloc OverlapBox
        int hitCount = Physics2D.OverlapBoxNonAlloc(attackPosition, attackSize, 0f, hits, enemyLayer);

        audioManager.PlaySound("Attack");

        for (int i = 0; i < hitCount; i++)
        {
            Collider2D hit = hits[i];
            if (hit == null) continue;

            // Проверка направления к врагу
            float directionToEnemy = hit.transform.position.x - transform.position.x;
            if ((characterMovement.IsFacingRight && directionToEnemy > 0) || (!characterMovement.IsFacingRight && directionToEnemy < 0))
            {
                // Нанесение урона через EnemyHealth
                EnemyHealth enemyHealth = hit.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(attackDamage);
                }
            }
        }
    }
}