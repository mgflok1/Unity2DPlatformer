using UnityEngine;
using Zenject;
using System.Collections.Generic;
using System.Linq;

public class CoinRegistry : ICoinRegistry
{
    private List<Coin> _registeredCoins = new List<Coin>();
    private Dictionary<int, int> _totalCoinsPerLevel = new Dictionary<int, int>();

    // Инъекция boundariesProvider
    [Inject] private ICameraBoundariesProvider boundariesProvider;

    // Регистрация монеты
    public void RegisterCoin(Coin coin)
    {
        if (coin == null)
        {
            Debug.LogWarning("Попытка зарегистрировать null монету.");
            return;
        }
        if (!_registeredCoins.Contains(coin))
        {
            _registeredCoins.Add(coin);
        }
    }

    // Получение общего количества монет на уровне
    public int GetTotalCoinsForLevel(int level)
    {
        if (_totalCoinsPerLevel.TryGetValue(level, out int total))
        {
            return total;
        }

        Transform left = boundariesProvider.GetLeftBoundary(level);
        Transform right = boundariesProvider.GetRightBoundary(level);

        if (left == null || right == null)
        {
            Debug.LogWarning($"Границы не найдены для уровня {level}. Общее количество монет установлено в 0.");
            total = 0;
        }
        else
        {
            float minX = left.position.x;
            float maxX = right.position.x;

            total = _registeredCoins
                .Where(coin => coin != null && coin.gameObject != null &&
                               coin.transform.position.x >= minX && coin.transform.position.x <= maxX)
                .Sum(coin => coin.Value);
        }

        _totalCoinsPerLevel[level] = total;
        return total;
    }
}