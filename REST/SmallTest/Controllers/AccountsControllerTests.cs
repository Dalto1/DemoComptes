﻿using Domain.Interfaces;
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
            Mock<IAccountsRepository> mockAccountRepository = new();
            mockAccountRepository.Setup(m => m.Create(accountA)).ReturnsAsync(accountA);
            AccountsController sut = this.CreateAccountsController(mockAccountRepository.Object);

            //Act
            ActionResult<AccountModel> response = await sut.Create(accountA);

            //Assert
            Assert.IsType<CreatedAtActionResult>(response?.Result);
            mockAccountRepository.Verify(v => v.Create(accountA));
        }
        [Fact]
        public async void GivenCreateAccount_WhenCreateAccountVaild_ThenResponseIsCorrespondingToGiven()
        {
            //Arrange
            Mock<IAccountsRepository> mockAccountRepository = new();
            mockAccountRepository.Setup(m => m.Create(accountA)).ReturnsAsync(accountA);
            AccountsController sut = this.CreateAccountsController(mockAccountRepository.Object);

            //Act
            ActionResult<AccountModel> response = await sut.Create(accountA);

            //Assert
            var createdAtActionResponse = (CreatedAtActionResult)response.Result;
            accountA.Should().BeEquivalentTo((AccountModel)createdAtActionResponse?.Value);
            mockAccountRepository.Verify(v => v.Create(accountA));
        }
        [Fact]
        public async void GivenNewAccountController_WhenAccountFindANonExistentAccountId_ThenResponseIsTypeNotFoundResult()
        {
            //Arrange
            Mock<IAccountsRepository> mockAccountRepository = new();
            mockAccountRepository.Setup(m => m.FindByAccountId(It.IsAny<int>())).ReturnsAsync(null as AccountModel);
            AccountsController sut = this.CreateAccountsController(mockAccountRepository.Object);

            //Act
            ActionResult<AccountModel> response = await sut.FindByAccountId(aNonExistentAccountId);

            //Assert
            Assert.IsType<NotFoundResult>(response?.Result);
            mockAccountRepository.Verify(v => v.FindByAccountId(aNonExistentAccountId));
        }
    }
}
