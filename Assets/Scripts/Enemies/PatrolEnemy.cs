using UnityEngine;
using Zenject;

public class PatrolEnemy : Enemy
{
    [SerializeField] private float patrolDistance = 3f;

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private bool movingRight = true;

    // Инъекция playerHealth
    [Inject] private IPlayerHealth playerHealth;

    protected override void Awake()
    {
        base.Awake();
        startPosition = transform.position;
        UpdateTargetPosition(); // Инициализация первой цели
    }

    // Обработка движения
    protected override void HandleMovement()
    {
        if (isDead) return;

        // Плавное движение к цели
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Проверка достижения цели
        if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
        {
            movingRight = !movingRight;
            Flip();
            UpdateTargetPosition();
        }
    }

    // Обновление целевой позиции
    private void UpdateTargetPosition()
    {
        targetPosition = startPosition + (movingRight ? Vector2.right * patrolDistance : Vector2.left * patrolDistance);
    }
}