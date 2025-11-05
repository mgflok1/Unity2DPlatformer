using System.Collections.Generic;

public class ResetManager : IResetManager
{
    // Список сбрасываемых объектов
    private List<IResettable> _resettables = new List<IResettable>();

    // Регистрация сбрасываемых объектов
    public void RegisterResettable(IResettable resettable)
    {
        if (!_resettables.Contains(resettable))
        {
            _resettables.Add(resettable);
        }
    }

    // Получение сбрасываемых объектов
    public IEnumerable<IResettable> GetResettables() => _resettables;
}