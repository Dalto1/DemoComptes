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
            AccountNumber = 1,
            AccountBalance = 99,
            AccountCreationDate = DateTime.Now,
            AccountHolderFirstName = "Paul",
            AccountHolderLastName = "Houde",
            IsActive = true
        }; 
        private AccountsController CreateAccountsController()
        {
            Mock<IAccountsRepository> mockAccountRepository = new();
            return new AccountsController(mockAccountRepository.Object);
        }

        [Fact]
        public void GivenCreateAccount_WhenCreateAccountVaild_ThenResponseIsValidResult()
        {
            //Arrange
            Mock<IAccountsRepository> mockAccountRepository = new();
            mockAccountRepository.Setup(m => m.AccountCreate(accountA)).ReturnsAsync(accountA);

            //Act
            var sut = mockAccountRepository.Object.AccountCreate(accountA);

            //Assert
            Assert.IsType<AccountModel>(sut?.Result);
            mockAccountRepository.Verify(v => v.AccountCreate(accountA));
        }
        [Fact]
        public void GivenCreateAccount_WhenCreateAccountVaild_ThenResponseIsCorrectModel()
        {
            //Arrange
            Mock<IAccountsRepository> mockAccountRepository = new();
            mockAccountRepository.Setup(m => m.AccountCreate(accountA)).ReturnsAsync(accountA);

            //Act
            var sut = mockAccountRepository.Object.AccountCreate(accountA).Result;

            //Assert
            accountA.Should().BeEquivalentTo(sut);
            mockAccountRepository.Verify(v => v.AccountCreate(accountA));
        }
        [Fact]
        public async void GivenNewAccountController_WhenAccountFindANonExistentAccountId_ThenResponseIsTypeNotFoundResult()
        {
            //Arrange
            var sut = this.CreateAccountsController();

            //Act
            ActionResult<AccountModel> response = await sut.AccountFind(aNonExistentAccountId);

            //Assert
            Assert.IsType<NotFoundResult>(response?.Result);
        }
    }
}
