namespace Veruthian.Library.Steps.Actions.Extensions
{
    public static class BooleanExtensions
    {
        public static IStep Boolean(this StepGenerator generator, bool? value)
            => new BooleanStep(value);
    }
}