using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public abstract class Enemy : MonoBehaviour, IEnemy, IResettable
{
    // Настройки врага
    [SerializeField] protected float speed = 2f;
    [SerializeField] protected float scale = 0.1f;
    [SerializeField] protected float damageOnContact = 20f;
    [SerializeField] private GameObject healthBarPrefab;

    protected Rigidbody2D rb;
    protected bool isDead = false;
    protected bool isFacingRight = true;
    protected Vector2 initialPosition;

    private EnemyHealthBar healthBarInstance;

    // Свойство урона при контакте
    public float DamageOnContact => damageOnContact;

    // Инъекция resetManager
    [Inject] private IResetManager resetManager;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        initialPosition = transform.position;

        EnemyHealth health = GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.OnDeath += Die;
            health.OnHealthChanged += OnHealthChanged;
        }

        // Регистрация в менеджере сброса
        resetManager.RegisterResettable(this);
    }

    protected virtual void Update()
    {
        if (isDead) return;
        HandleMovement();
    }

    // Обработка движения
    protected abstract void HandleMovement();

    // Смерть
    public virtual void Die()
    {
        if (isDead) return;
        isDead = true;
        rb.velocity = Vector2.zero;
        gameObject.SetActive(false);
        if (healthBarInstance != null)
        {
            healthBarInstance.Hide();
        }
    }

    // Поворот
    protected void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(isFacingRight ? scale : -scale, scale, scale);
    }

    // Сброс
    public virtual void Reset()
    {
        transform.position = initialPosition;
        gameObject.SetActive(true);
        isDead = false;
        rb.velocity = Vector2.zero;

        // Сброс здоровья
        EnemyHealth health = GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.ResetHealth();
        }

        // Сброс полоски здоровья
        if (healthBarInstance != null)
        {
            healthBarInstance.ResetBar();
        }
    }

    protected virtual void OnDestroy()
    {
        // Отписка от событий
        EnemyHealth health = GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.OnDeath -= Die;
            health.OnHealthChanged -= OnHealthChanged;
        }
        if (healthBarInstance != null)
        {
            Destroy(healthBarInstance.gameObject);
        }
    }

    // Обработчик изменения здоровья
    private void OnHealthChanged(float normalizedHealth)
    {
        if (healthBarPrefab == null) return;

        if (healthBarInstance == null)
        {
            // Инстанцирование полоски здоровья
            GameObject instance = Instantiate(healthBarPrefab, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
            healthBarInstance = instance.GetComponent<EnemyHealthBar>();
            if (healthBarInstance != null)
            {
                healthBarInstance.SetTarget(transform);
            }
        }

        if (healthBarInstance != null)
        {
            healthBarInstance.UpdateHealth(normalizedHealth);
            if (normalizedHealth >= 1f || normalizedHealth <= 0f)
            {
                healthBarInstance.Hide();
            }
        }
    }
}