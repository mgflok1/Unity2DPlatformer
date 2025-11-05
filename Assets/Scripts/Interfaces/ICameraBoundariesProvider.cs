using UnityEngine;

public interface ICameraBoundariesProvider
{
    Transform GetLeftBoundary(int levelIndex);
    Transform GetRightBoundary(int levelIndex);
    Transform GetBottomBoundary(int levelIndex);
}