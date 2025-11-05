using UnityEngine;
using DG.Tweening;

public class EnemyHealthBar : MonoBehaviour
{
    // Спрайт заполнения
    [SerializeField] private SpriteRenderer fillSprite;
    // Фоновый спрайт
    [SerializeField] private SpriteRenderer backgroundSprite;
    // Смещение
    [SerializeField] private Vector3 offset = new Vector3(0f, 1f, 0f);
    // Длительность анимации
    [SerializeField] private float tweenDuration = 0.3f;

    private Transform target;
    private float originalWidth;

    private void Awake()
    {
        if (fillSprite == null)
        {
            fillSprite = GetComponent<SpriteRenderer>();
        }
        if (fillSprite != null)
        {
            originalWidth = fillSprite.transform.localScale.x;
        }
        if (backgroundSprite != null)
        {
            backgroundSprite.gameObject.SetActive(false);
        }
        Hide();
    }

    // Установка цели
    public void SetTarget(Transform enemyTransform)
    {
        target = enemyTransform;
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }

    // Обновление здоровья
    public void UpdateHealth(float normalizedHealth)
    {
        if (fillSprite != null)
        {
            Vector3 targetScale = fillSprite.transform.localScale;
            targetScale.x = originalWidth * normalizedHealth;

            fillSprite.transform.DOScaleX(targetScale.x, tweenDuration).SetEase(Ease.OutQuad);

            Color targetColor = Color.Lerp(Color.red, Color.green, normalizedHealth);
            fillSprite.DOColor(targetColor, tweenDuration).SetEase(Ease.OutQuad);
        }
        if (backgroundSprite != null)
        {
            backgroundSprite.gameObject.SetActive(true);
        }
        Show();
    }

    // Показать бар
    public void Show()
    {
        gameObject.SetActive(true);
        if (backgroundSprite != null)
        {
            backgroundSprite.gameObject.SetActive(true);
        }
        if (fillSprite != null)
        {
            fillSprite.gameObject.SetActive(true);
        }
    }

    // Скрыть бар
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    // Сброс бара
    public void ResetBar()
    {
        Hide();
        if (fillSprite != null)
        {
            fillSprite.transform.DOScaleX(originalWidth, tweenDuration).SetEase(Ease.OutQuad);
            fillSprite.DOColor(Color.green, tweenDuration).SetEase(Ease.OutQuad);
        }
    }
}