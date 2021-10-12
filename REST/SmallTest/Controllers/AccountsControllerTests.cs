using Domain.Interfaces;
using Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using REST.Controllers;
using System;
using Xunit;

namespace REST.SmallTest.Controllers
{
    public class AccountsControllerTests
    {
        private readonly int aNonExistentAccountId = 999;
        private readonly AccountModel accountA = new()
        {
            AccountId = 1,
            AccountNumber = "1234-CELI",
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
        public async void GivenCreateAccount_WhenCreateAccountVaild_ThenResponseIsCorrectModel()
        {
            //Arrange
            var sut = this.CreateAccountsController();

            //Act
            ActionResult<AccountModel> response = await sut.Create(accountA);

            //Assert
            Assert.IsType<CreatedAtActionResult>(response?.Result);

        }
        [Fact]
        public async void GivenCreateAccount_WhenCreateAccountVaild_ThenResponseIsCorrespondingToGiven()
        {          
            //Arrange
            var sut = this.CreateAccountsController();

            //Act
            ActionResult<AccountModel> response = await sut.Create(accountA);

            //Assert
            accountA.Should().BeEquivalentTo(response?.Value);
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
