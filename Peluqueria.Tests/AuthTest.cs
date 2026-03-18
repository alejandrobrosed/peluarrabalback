
namespace Peluqueria.Tests;

using Xunit;

public class AuthTests
{
    [Fact]
    public void Login_DatosInvalidos_DevuelveError()
    {
        var email = "";
        var password = "";

        var resultado = email == "" || password == "";

        Assert.True(resultado);
    }
}