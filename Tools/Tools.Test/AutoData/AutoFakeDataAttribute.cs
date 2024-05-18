using AutoFixture.Xunit2;

namespace Tools.Test.AutoData;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AutoFakeDataAttribute : AutoDataAttribute
{
    public AutoFakeDataAttribute()
        : base(AutoFakeDataTestBase.CreateFixture)
    {
    }
}