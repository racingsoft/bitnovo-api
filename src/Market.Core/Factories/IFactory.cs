namespace Market.Core.Factories
{
    public interface IFactory<T>
    {
        T Create();
    }
}