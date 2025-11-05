using UnityEngine.Events;

public interface IScoreable
{
    int Value { get; }  // Значение монеты (по умолчанию 1)
    event UnityAction<IScoreable> OnCollected;  // Событие для уведомления менеджера
}