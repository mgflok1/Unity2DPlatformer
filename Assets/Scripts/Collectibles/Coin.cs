using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class Coin : MonoBehaviour, ICollectible, IResettable, IScoreable
{
    // Ценность монеты
    [SerializeField] private int value = 1;

    // Инъекции зависимостей
    [Inject] private ICoinManager coinManager;
    [Inject] private IResetManager resetManager;
    [Inject] private ICoinRegistry coinRegistry;
    [Inject] private IAudioManager audioManager;

    public int Value => value;
    public event UnityAction<IScoreable> OnCollected;

    private void Awake()
    {
        resetManager.RegisterResettable(this);
        coinRegistry.RegisterCoin(this);
    }

    // Сбор монеты
    public void Collect()
    {
        if (coinManager != null)  
        {
            coinManager.AddCoins(value);
        }
        OnCollected?.Invoke(this);
        gameObject.SetActive(false);
        audioManager.PlaySound("CoinCollect");
    }

    // Сброс монеты
    public void Reset()
    {
        gameObject.SetActive(true);
    }
}