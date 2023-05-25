using Antelcat.Foundation.Core.Attributes;

namespace Feast.Foundation.Test;

public interface IA { }
public class A : IA
{
    public A(IB b)
    {
                
    }
    private static int count = 0;
    private readonly int number = ++count;
    [Autowired]
    private IB B { get; set; }
}
public interface IB { }
public class B : IB
{
    private static int count = 0;
    private readonly int number = ++count;
    [Autowired]
    private IA A { get; set; }
}
public interface IC { }
public class C : IC
{
    private static int count = 0;
    private readonly int number = ++count;
    [Autowired]
    private readonly IA A;
    [Autowired]
    private readonly IB B;
}
public interface ID { }
public class D : ID
{
    private static int count = 0;
    private readonly int number = ++count;
    [Autowired]
    private readonly IA A;
    [Autowired]
    private readonly IB B;
    [Autowired]
    private readonly IC C;
}
