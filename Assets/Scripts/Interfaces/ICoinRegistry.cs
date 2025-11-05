using System.Collections.Generic;

public interface ICoinRegistry
{
    void RegisterCoin(Coin coin);
    int GetTotalCoinsForLevel(int level);
}