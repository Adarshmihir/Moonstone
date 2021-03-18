namespace Core
{
    public interface IEvaluator
    {
        bool? Evaluate(string name, string parameter);
    }
}
