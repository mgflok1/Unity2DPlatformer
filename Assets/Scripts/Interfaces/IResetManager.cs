using System.Collections.Generic;

public interface IResetManager
{
    void RegisterResettable(IResettable resettable);
    IEnumerable<IResettable> GetResettables();
}