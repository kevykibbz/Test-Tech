using DataAccess.Repositories;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace AppLogic.Tests.NUnit;

[TestFixture]
public class LegalLogicTests
{
    [Test]
    public void ConstructorThrowsArgumentNullException_When_LegalRepoNull()
    {
        // arrange
        var mapper = Mock.Of<IObjectMapper>();

        // act
        var action = () => new LegalLogic(legalRepo: null!, mapper);

        // assert
        action.ShouldThrow<ArgumentNullException>();
    }

    [Test]
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
