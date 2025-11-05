using UnityEngine;
using Zenject;

public class CameraFollow : MonoBehaviour
{
    // Инъекции зависимостей
    [Inject] private ILevelManager levelManager;
    [Inject] private ICameraBoundariesProvider boundariesProvider;

    private Transform target;
    private Camera cam;

    // Смещение камеры
    [SerializeField] private Vector3 offset = new Vector3(0f, 1f, -10f);

    // Кэш границ и уровня
    private int cachedLevel = -1;
    private Transform cachedLeft;
    private Transform cachedRight;
    private Transform cachedBottom;

    // Конструктор для инъекций
    [Inject]
    public void Construct()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("Компонент Camera не найден на этом объекте!");
            enabled = false;
        }

        // Подписка на смену уровня
        levelManager.OnLevelSelected += OnLevelSelected;
    }

    // Отписка от события
    private void OnDestroy()
    {
        levelManager.OnLevelSelected -= OnLevelSelected;
    }

    private void LateUpdate()
    {
        if (target == null || cam == null)
        {
            return;
        }

        if (cachedLevel == -1 || cachedLeft == null || cachedRight == null || cachedBottom == null)
        {
            Debug.LogWarning("Границы не закэшированы или недопустимы.");
            return;
        }

        UpdateCameraPosition(target, cachedLeft, cachedRight, cachedBottom);
    }

    // Установка цели
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // Обновление позиции камеры
    private void UpdateCameraPosition(Transform target, Transform leftBoundary, Transform rightBoundary, Transform bottomBoundary)
    {
        float halfWidth = cam.orthographicSize * cam.aspect;
        float dynamicMinX = leftBoundary.position.x + halfWidth;
        float dynamicMaxX = rightBoundary.position.x - halfWidth;

        Vector3 desiredPosition = target.position + offset;
        float clampedX = Mathf.Clamp(desiredPosition.x, dynamicMinX, dynamicMaxX);

        float halfHeight = cam.orthographicSize;
        float dynamicMinY = bottomBoundary.position.y + halfHeight;
        float clampedY = Mathf.Max(desiredPosition.y, dynamicMinY);

        transform.position = new Vector3(clampedX, clampedY, desiredPosition.z);
    }

    // Обработчик смены уровня
    private void OnLevelSelected(int selectedLevel)
    {
        cachedLevel = selectedLevel;
        cachedLeft = boundariesProvider.GetLeftBoundary(selectedLevel);
        cachedRight = boundariesProvider.GetRightBoundary(selectedLevel);
        cachedBottom = boundariesProvider.GetBottomBoundary(selectedLevel);

        if (cachedLeft == null || cachedRight == null || cachedBottom == null)
        {
            Debug.LogWarning($"Границы не найдены для уровня {selectedLevel}");
            cachedLevel = -1;
        }
    }
}