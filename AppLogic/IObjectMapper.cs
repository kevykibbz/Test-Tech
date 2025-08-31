namespace AppLogic;

public interface IObjectMapper
{
    TDest Map<TSource, TDest>(TSource item);
    IEnumerable<TDest> Map<TSource, TDest>(IEnumerable<TSource> sourceCollection) => sourceCollection.Select(Map<TSource, TDest>);
}
