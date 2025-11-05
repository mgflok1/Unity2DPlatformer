using UnityEngine;
using Zenject;

public interface IAudioManager
{
    void PlaySound(string soundId, Vector3? position = null);
    void SetMasterVolume(float volume);
}