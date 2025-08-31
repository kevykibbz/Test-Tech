using DataAccess.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;

namespace AppLogic.Tests.MsTest;

[TestClass]
public class LegalLogicTests
{
    [TestMethod]
    public void ConstructorThrowsArgumentNullException_When_LegalRepoNull()
    {
        // arrange
        var mapper = Mock.Of<IObjectMapper>();

        // act
        var action = () => new LegalLogic(legalRepo: null!, mapper);

        // assert
        action.ShouldThrow<ArgumentNullException>();
    }

    [TestMethod]
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
