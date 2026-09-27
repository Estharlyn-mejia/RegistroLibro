namespace RegistroLibro.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var a = 10; var b = 20;
        var resultado = a + b;
        Assert.Equal(30, resultado);
    }
}
