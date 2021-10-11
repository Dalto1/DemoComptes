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
        [Fact]
        public async Task GivenFindAccount_WhenAccountFindANonExistentAccountId_ThenResponseIsTypeNotFoundResult()
        {
            //Arrange
            Mock<IAccountsRepository> mockAccountRepository = new();
            AccountsController controller = new(mockAccountRepository.Object);
            int nonExistentAccountId = 999;

            //Act
            var sut = await controller.AccountFind(nonExistentAccountId);

            //Assert
            Assert.IsType<NotFoundResult>(sut?.Result);
        }
        [Fact]
        public void GivenCreateAccount_WhenCreateAccountVaild_ThenResponseIsValidResult()
        {
            //Arrange
            AccountModel a = new()
            {
                AccountNumber = 1,
                AccountBalance = 99,
                AccountCreationDate = DateTime.Now,
                AccountHolderFirstName = "Paul",
                AccountHolderLastName = "Houde",
                IsActive = true
            };
            Mock<IAccountsRepository> mockAccountRepository = new();
            mockAccountRepository.Setup(m => m.AccountCreate(a)).ReturnsAsync(a);

            //Act
            var sut = mockAccountRepository.Object.AccountCreate(a);

            //Assert
            Assert.IsType<AccountModel>(sut?.Result);
            mockAccountRepository.Verify(v => v.AccountCreate(a));
        }
        [Fact]
        public void GivenCreateAccount_WhenCreateAccountVaild_ThenResponseIsCorrectModel()
        {
            //Arrange
            AccountModel a = new()
            {
                AccountNumber = 1,
                AccountBalance = 99,
                AccountCreationDate = DateTime.Now,
                AccountHolderFirstName = "Paul",
                AccountHolderLastName = "Houde",
                IsActive = true
            };
            Mock<IAccountsRepository> mockAccountRepository = new();
            mockAccountRepository.Setup(m => m.AccountCreate(a)).ReturnsAsync(a);

            //Act
            var sut = mockAccountRepository.Object.AccountCreate(a).Result;

            //Assert
            a.Should().BeEquivalentTo(sut);
            mockAccountRepository.Verify(v => v.AccountCreate(a));
        }
    }
}
