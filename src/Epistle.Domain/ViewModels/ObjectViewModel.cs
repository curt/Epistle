namespace Epistle.Domain.ViewModels;

public class ObjectViewModel(Object obj, Actor actor)
{
    public Object Object { get; } = obj;

    public Actor Actor { get; } = actor;
}
