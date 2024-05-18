using AutoFixture;
using AutoFixture.AutoFakeItEasy;

namespace Tools.Test.AutoData;

public class AutoFakeDataTestBase
{
    public static Fixture CreateFixture()
    {
        var fixture = new Fixture();
        fixture.Customize(new AutoFakeItEasyCustomization
        {
            ConfigureMembers = true,
            GenerateDelegates = true,
        });
        fixture.Customize<TimeOnly>(composer => composer.FromFactory<DateTime>(TimeOnly.FromDateTime));
        fixture.Customize<DateOnly>(composer => composer.FromFactory<DateTime>(DateOnly.FromDateTime));
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        return fixture;
    }
}