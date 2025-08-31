using DataAccess.DataModel;
using ServiceModel;
using System.Text.Json;

namespace AppLogic;

public class ObjectMapper : IObjectMapper
{
    public TDest Map<TSource, TDest>(TSource item)
    {
        object? result = item switch
        {
            null => null,
            LegalMatter matter => Map(matter),
            DbLegalMatter dbMatter => Map(dbMatter),
            Lawyer lawyer => Map(lawyer),
            DbLawyer dbLawyer => Map(dbLawyer),
            _ => throw new NotSupportedException()
        };

        return (TDest)result!;
    }

    public IEnumerable<TDest> Map<TSource, TDest>(IEnumerable<TSource> sourceCollection) => 
        sourceCollection?.Select(Map<TSource, TDest>) ?? Enumerable.Empty<TDest>();
    
    private static DbLegalMatter Map(LegalMatter matter) => new(
        matter.Id, 
        matter.MatterName,
        matter.ContractType,
        matter.Parties != null ? JsonSerializer.Serialize(matter.Parties) : null,
        matter.EffectiveDate,
        matter.ExpirationDate,
        matter.GoverningLaw,
        matter.ContractValue,
        matter.Status,
        matter.Description,
        matter.CreatedAt,
        matter.LastModified
    ) 
    { 
        LawyerId = matter.LawyerId 
    };
    
    private static LegalMatter Map(DbLegalMatter matter) => new(
        matter.Id, 
        matter.MatterName,
        matter.ContractType,
        !string.IsNullOrEmpty(matter.Parties) ? JsonSerializer.Deserialize<List<string>>(matter.Parties) : null,
        matter.EffectiveDate,
        matter.ExpirationDate,
        matter.GoverningLaw,
        matter.ContractValue,
        matter.Status,
        matter.Description,
        matter.CreatedAt,
        matter.LastModified
    ) 
    { 
        LawyerId = matter.LawyerId 
    };
    
    private static DbLawyer Map(Lawyer lawyer) => new(lawyer.Id, lawyer.FirstName, lawyer.LastName, lawyer.CompanyName);
    private static Lawyer Map(DbLawyer lawyer) => new(lawyer.Id, lawyer.FirstName, lawyer.LastName, lawyer.CompanyName);
}
