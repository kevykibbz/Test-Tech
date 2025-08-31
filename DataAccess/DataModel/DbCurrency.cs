namespace DataAccess.DataModel;

public class DbCurrency
{
    public required string Id { get; set; }
    public required string Symbol { get; set; }
    public required string Name { get; set; }
    public int DecimalDigits { get; set; }
}
