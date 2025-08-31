using DataAccess.DataModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceModel;
using Shouldly;

namespace AppLogic.Tests.MsTest;

[TestClass]
public class ObjectMapperTests
{
    private readonly IObjectMapper _mapper = new ObjectMapper();

    [TestMethod]
    public void Map_unknown_throws() => Should.Throw<NotSupportedException>(() => _mapper.Map<int, string>(2));

    [TestMethod]
    public void Map_nullObject_returnsNull_doesNotThrow()
    {
        var result = Should.NotThrow(() => _mapper.Map<LegalMatter, DbLegalMatter>((LegalMatter)null!));
        result.ShouldBeNull();
    }

    [TestMethod]
    public void Map_nullCollection_returnsNull_doesNotThrow()
    {
        var result = Should.NotThrow(() => _mapper.Map<LegalMatter, DbLegalMatter>((IEnumerable<LegalMatter>)null!));
        result.ShouldBeNull();
    }

    [TestMethod]
    public void Map_Collection_ReturnsMappedCollection()
    {
        var values = new List<LegalMatter>
        {
            new (Guid.NewGuid(), "name1"),
            new (Guid.NewGuid(), "name2")
        };

        var result = _mapper.Map<LegalMatter, DbLegalMatter>(values);

        result.ShouldNotBeEmpty();
        result.Count().ShouldBe(2);
        result.ShouldBeAssignableTo<IEnumerable<DbLegalMatter>>();
    }

    [TestMethod]
    public void Map_LegalMatter_ReturnsCorrectlyMappedDbLegalMatter()
    {
        var source = new LegalMatter(Guid.NewGuid(), "name1");
        var result = _mapper.Map<LegalMatter, DbLegalMatter>(source);

        result.ShouldNotBeNull();
        result.ShouldBeOfType<DbLegalMatter>();
        result.Id.ShouldBe(source.Id);
        result.MatterName.ShouldBe(source.MatterName);
    }

    [TestMethod]
    public void Map_DbLegalMatter_ReturnsCorrectlyMappedLegalMatter()
    {
        var source = new DbLegalMatter(Guid.NewGuid(), "name1");
        var result = _mapper.Map<DbLegalMatter, LegalMatter>(source);

        result.ShouldNotBeNull();
        result.ShouldBeOfType<LegalMatter>();
        result.Id.ShouldBe(source.Id);
        result.MatterName.ShouldBe(source.MatterName);
    }
}
