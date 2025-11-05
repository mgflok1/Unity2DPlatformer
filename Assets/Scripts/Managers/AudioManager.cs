using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;
using Zenject;
using Cysharp.Threading.Tasks;
using System;

public class AudioManager : MonoBehaviour, IAudioManager
{
    // Класс конфигурации звука
    [System.Serializable]
    public class SoundConfig
    {
        public string id;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
    }

    // Массив конфигураций звуков
    [SerializeField] private SoundConfig[] sounds;

    private readonly Dictionary<string, SoundConfig> soundConfigDict = new();
    private ObjectPool<AudioSource> sourcePool;
    private Transform poolParent;

    private const int MaxPoolSize = 50;
    private const int PreloadCount = 10;

    private void Awake()
    {
        // Заполнение словаря конфигураций
        foreach (var config in sounds)
        {
            if (config == null || string.IsNullOrEmpty(config.id))
            {
                Debug.LogWarning("Недопустимая конфигурация звука (null или пустой ID). Пропуск.");
                continue;
            }
            if (soundConfigDict.ContainsKey(config.id))
            {
                Debug.LogWarning($"Дубликат ID звука '{config.id}'");
                continue;
            }
            soundConfigDict[config.id] = config;
        }

        // Создание пула AudioSource
        poolParent = new GameObject("AudioPool").transform;
        poolParent.SetParent(transform);
        sourcePool = new ObjectPool<AudioSource>(
            createFunc: CreateSource,
            actionOnGet: source => source.gameObject.SetActive(true),
            actionOnRelease: source => source.gameObject.SetActive(false),
            actionOnDestroy: source => UnityEngine.Object.Destroy(source.gameObject),
            maxSize: MaxPoolSize
        );

        // Предзагрузка источников
        List<AudioSource> preloaded = new List<AudioSource>();
        for (int i = 0; i < PreloadCount; i++)
        {
            preloaded.Add(sourcePool.Get());
        }
        foreach (var source in preloaded)
        {
            sourcePool.Release(source);
        }
    }

    // Создание источника звука
    private AudioSource CreateSource()
    {
        GameObject go = new GameObject("PooledAudioSource");
        go.transform.SetParent(poolParent);
        return go.AddComponent<AudioSource>();
    }

    // Воспроизведение звука
    public void PlaySound(string soundId, Vector3? position = null)
    {
        if (string.IsNullOrEmpty(soundId))
        {
            Debug.LogWarning("Недопустимый ID звука (null или пустой).");
            return;
        }

        if (!soundConfigDict.TryGetValue(soundId, out SoundConfig config))
        {
            Debug.LogWarning($"ID звука '{soundId}' не найден");
            return;
        }

        if (config.clip == null)
        {
            Debug.LogWarning($"AudioClip для '{soundId}' равен null.");
            return;
        }

        AudioSource source = sourcePool.Get();
        ConfigureSource(source, config, position);
        source.Play();

        // Возврат в пул после проигрывания
        ReturnToPoolAfterPlay(source, config.clip.length).Forget();
    }

    // Настройка источника
    private void ConfigureSource(AudioSource source, SoundConfig config, Vector3? position)
    {
        source.clip = config.clip;
        source.volume = config.volume;
        source.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        source.loop = false;
        source.priority = 128;
        source.spatialBlend = 0f;

        if (position.HasValue)
        {
            source.transform.position = position.Value;
            source.spatialBlend = 1f;
        }
    }

    // Асинхронный метод для возврата в пул
    private async UniTaskVoid ReturnToPoolAfterPlay(AudioSource source, float delay)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delay));
        sourcePool.Release(source);
    }

    // Установка общей громкости
    public void SetMasterVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0f, 1f);
        AudioListener.volume = volume;
    }
}