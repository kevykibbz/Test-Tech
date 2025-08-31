using DataAccess.Repositories;
using Moq;
using Shouldly;
using Xunit;

namespace AppLogic.Tests.XUnit;

public class LegalLogicTests
{
    [Fact]
    public void ConstructorThrowsArgumentNullException_When_LegalRepoNull()
    {
        // arrange
        var mapper = Mock.Of<IObjectMapper>();

        // act
        var action = () => new LegalLogic(legalRepo: null!, mapper);

        // assert
        action.ShouldThrow<ArgumentNullException>();
    }

    [Fact]
    public void ConstructorThrowsArgumentNullException_When_MapperNull()
    {
        // arrange
        var legalRepo = Mock.Of<ILegalRepository>();

        // act
        var action = () => new LegalLogic(legalRepo, mapper: null!);

        // assert
        action.ShouldThrow<ArgumentNullException>();
    }
}
