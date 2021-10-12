using Domain.Interfaces;
using Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using REST.Controllers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace REST.SmallTest.Controllers
{
    public class AccountsControllerTests
    {
        private readonly int aNonExistentAccountId = 999;
        private readonly AccountModel accountA = new()
        {
            AccountId = 1,
            AccountBalance = 99,
            AccountCreationDate = DateTime.Now,
            AccountHolderFirstName = "Paul",
            AccountHolderLastName = "Houde",
            IsActive = true
        }; 
        private AccountsController CreateAccountsController(IAccountsRepository accountsRepository = null)
        {
            var mockAccountRepository = accountsRepository ?? new Mock<IAccountsRepository>().Object;
            return new AccountsController(mockAccountRepository);
        }


        [Fact]
        public async void GivenCreateAccount_WhenCreateAccountVaild_ThenResponseIsValidResult()
        {
            //Arrange
            Mock<IAccountsRepository> mockAccountRepository = new();
            var sut = this.CreateAccountsController(mockAccountRepository.Object);

            //Act
            ActionResult<AccountModel> response = await sut.Create(accountA);

            //Assert
            Assert.IsType<AccountModel>(response?.Result);
        }
        /*[Fact]
        public void GivenCreateAccount_WhenCreateAccountVaild_ThenResponseIsValidResult()
        {
            //Arrange
            Mock<IAccountsRepository> mockAccountRepository = new();
            mockAccountRepository.Setup(m => m.Create(accountA)).ReturnsAsync(accountA);

            //Act
            var sut = mockAccountRepository.Object.Create(accountA);

            //Assert
            Assert.IsType<AccountModel>(sut?.Result);
            mockAccountRepository.Verify(v => v.Create(accountA));
        }*/
        [Fact]
        public void GivenCreateAccount_WhenCreateAccountVaild_ThenResponseIsCorrectModel()
        {
            //Arrange
            Mock<IAccountsRepository> mockAccountRepository = new();
            mockAccountRepository.Setup(m => m.Create(accountA)).ReturnsAsync(accountA);

            //Act
            var sut = mockAccountRepository.Object.Create(accountA).Result;

            //Assert
            accountA.Should().BeEquivalentTo(sut);
            mockAccountRepository.Verify(v => v.Create(accountA));
        }
        [Fact]
        public async void GivenNewAccountController_WhenAccountFindANonExistentAccountId_ThenResponseIsTypeNotFoundResult()
        {
            //Arrange
            var sut = this.CreateAccountsController();

            //Act
            ActionResult<AccountModel> response = await sut.FindByAccountId(aNonExistentAccountId);

            //Assert
            Assert.IsType<NotFoundResult>(response?.Result);
        }
    }
}
