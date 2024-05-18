using AutoFixture.Xunit2;
using Xunit;

namespace Tools.Test.AutoData;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class InlineAutoFakeDataAttribute : CompositeDataAttribute
{
    public InlineAutoFakeDataAttribute(params object?[] values)
        : base(new InlineDataAttribute(values), new AutoFakeDataAttribute())
    {
    }
}