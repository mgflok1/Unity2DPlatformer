using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerAttack))]
public class CharacterMovement : MonoBehaviour
{
    // Настройки движения
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 1f;

    // Проверка земли
    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform feetPos;
    [SerializeField] private float checkRadius = 0.1f;

    // Клавиши ввода
    [Header("Input Keys")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode attackKey = KeyCode.C;

    private Rigidbody2D rb;
    private Animator animator;
    private float moveInput;
    private bool isFacingRight = true;
    private bool isGrounded;
    private bool isDead = false;

    // Инъекции зависимостей
    [Inject] private IGameStateManager gameStateManager;
    [Inject] private IPlayerHealth playerHealth;
    [Inject] private IAudioManager audioManager;
    private IPlayerAttack playerAttack;

    // Свойство для доступа
    public bool IsFacingRight => isFacingRight;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Компонент Rigidbody2D не найден на этом GameObject!");
            enabled = false;
            return;
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Компонент Animator не найден на этом GameObject!");
            enabled = false;
            return;
        }

        playerAttack = GetComponent<IPlayerAttack>();
        if (playerAttack == null)
        {
            Debug.LogError("Компонент PlayerAttack не найден на этом GameObject!");
            enabled = false;
            return;
        }
    }

    private void Start()
    {
        // Подписка на событие смерти
        playerHealth.OnDeath += Die;
    }

    private void OnDestroy()
    {
        // Отписка от события
        playerHealth.OnDeath -= Die;
    }

    private void Update()
    {
        if (gameStateManager.IsGameOver || isDead) return;

        HandleInput();
        HandleMovement();
        HandleAnimations();
    }

    // Обработка ввода
    private void HandleInput()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, groundLayer);
        moveInput = Input.GetAxisRaw("Horizontal");

        if (isGrounded && Input.GetKeyDown(jumpKey))
        {
            Jump();
        }

        if (Input.GetKeyDown(attackKey))
        {
            // Триггер анимации атаки
            animator.SetTrigger("Attack");
        }
    }

    // Обработка движения
    private void HandleMovement()
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Поворот персонажа
        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    // Обработка анимаций
    private void HandleAnimations()
    {
        animator.SetBool("IsRunning", isGrounded && Mathf.Abs(moveInput) > 0);
        animator.SetBool("IsJumping", !isGrounded);
    }

    // Прыжок
    private void Jump()
    {
        if (!isGrounded) return;

        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        audioManager.PlaySound("Jump");
    }

    // Поворот персонажа
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(isFacingRight ? 1 : -1, 1, 1);
    }

    // Смерть
    public void Die()
    {
        if (isDead) return;
        isDead = true;
        animator.SetTrigger("Dead");
        rb.velocity = new Vector2(0f, rb.velocity.y);
        gameStateManager.SetGameOver(true);
    }

    // Событие анимации атаки
    public void OnAttackAnimationEvent()
    {
        if (playerAttack != null && !gameStateManager.IsGameOver && !isDead)
        {
            playerAttack.PerformAttack();
        }
    }
}