using System.ComponentModel.DataAnnotations;

namespace DataAccess.DataModel;

public record DbLawyer
(
    [property: Key] Guid Id,
    string FirstName,
    string LastName,
    string CompanyName
);
