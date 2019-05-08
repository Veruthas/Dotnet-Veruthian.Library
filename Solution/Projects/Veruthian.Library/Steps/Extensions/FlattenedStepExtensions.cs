namespace Veruthian.Library.Steps.Extensions
{
    public static class FlattenedStepExtensions
    {
        public static FlattenedStep[] Flatten(this IStep step) => FlattenedStep.Flatten(step);
    }
}