using UnityEngine;
using Zenject;
using UnityEngine.Events;

public class CoinManager : ICoinManager
{
    // Событие изменения монет
    public event UnityAction<int> OnCoinsChanged;
    // Событие изменения монет на уровне
    public event UnityAction<int, int> OnLevelCoinsChanged;

    private int _currentCoins = 0;
    private int _levelCoins = 0;
    private int _totalCoinsOnLevel = 0;
    private const string COINS_PREFS_KEY = "TotalCoins";

    // Конструктор с инъекцией
    [Inject]
    public CoinManager()
    {
        LoadCoins();
    }

    // Текущие монеты
    public int CurrentCoins => _currentCoins;
    // Монеты на уровне
    public int LevelCoins => _levelCoins;
    // Общее кол-во монет на уровне
    public int TotalCoinsOnLevel => _totalCoinsOnLevel;

    // Добавление монет
    public void AddCoins(int value)
    {
        if (value <= 0) return;

        _currentCoins += value;
        _levelCoins += value;
        OnCoinsChanged?.Invoke(_currentCoins);
        OnLevelCoinsChanged?.Invoke(_levelCoins, _totalCoinsOnLevel);
        SaveCoins();
    }

    // Инициализация монет на уровне
    public void InitializeLevelCoins(int totalOnLevel)
    {
        _totalCoinsOnLevel = totalOnLevel;
        ResetCoinsForLevel();
    }

    // Сброс монет на уровне
    public void ResetCoinsForLevel()
    {
        _levelCoins = 0;
        OnLevelCoinsChanged?.Invoke(_levelCoins, _totalCoinsOnLevel);
    }

    // Загрузка монет
    private void LoadCoins()
    {
        _currentCoins = PlayerPrefs.GetInt(COINS_PREFS_KEY, 0);
        OnCoinsChanged?.Invoke(_currentCoins);
    }

    // Сохранение монет
    private void SaveCoins()
    {
        PlayerPrefs.SetInt(COINS_PREFS_KEY, _currentCoins);
        PlayerPrefs.Save();
    }
}