using UnityEngine;

public interface IStartPositionProvider
{
    Transform GetStartPosition(int levelIndex);
}