using Antelcat.Foundation.Core.Attributes;

namespace Feast.Foundation.Test;

public abstract class Factory<T> where T : Factory<T>
{
    private static int count = 0;
    protected readonly int Number = ++count;
}

public interface IA { }

public class A : Factory<A>, IA
{
    [Autowired] public IB B { get; set; }
}

public interface IB { }

public class B : Factory<B>, IB
{
    [Autowired] private IA A { get; set; }
}

public interface IC { }

public class C : Factory<C>, IC
{
    [Autowired] private readonly IA A;
    [Autowired] private readonly IB B;
}

public interface ID { }

public class D : Factory<D>, ID
{
    [Autowired] private readonly IA A;
    [Autowired] private readonly IB B;
    [Autowired] private readonly IC C;
}

public class GenericType<T>
{
}