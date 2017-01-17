namespace AMPSystem.Interfaces
{
    internal interface ISimpleFilter<T> : IFilter
    {
        T FilterAttribute { get; set; }
    }
}