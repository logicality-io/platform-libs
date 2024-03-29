using Amazon.Lambda.TestUtilities;
using Logicality.Lambda.Example;
using Shouldly;
using Xunit;

namespace Logicality.Lambda;

public class SynchronousInvokeFunctionTests
{
    [Fact]
    public async Task Can_activate_lambda()
    {
        var testFunction = new ExampleSynchronousInvokeFunction();

        var request = new Request("http://example.com");
        var response = await testFunction.Handle(request, new TestLambdaContext());

        response.Body.ShouldNotBeNullOrEmpty();
    }
}