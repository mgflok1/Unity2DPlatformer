using UnityEngine.Events;

public interface ILevelManager
{
    int GetSelectedLevel();
    bool IsLevelSelected();
    void SelectLevel(int levelIndex);
    event UnityAction<int> OnLevelSelected;
}