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
        public async Task FindAccount_ActionResult_ReturnsNotFoundResultForNonExistentAccounts()
        {
            //Arrange
            Mock<IAccountsRepository> mockAccountRepository = new();
            AccountsController controller = new(mockAccountRepository.Object);
            int nonExistentAccountId = 999;

            //Act
            var response = await controller.AccountFind(nonExistentAccountId);

            //Assert
            Assert.NotNull(response);
            Assert.IsType<ActionResult<AccountModel>>(response);
            Assert.IsType<NotFoundResult>(response.Result);
        }
        [Fact]
        public void CreateAccount_ActionResult_ValidResultForExistentAccounts()
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
            var response = mockAccountRepository.Object.AccountCreate(a).Result;

            //Assert
            Assert.NotNull(response);
            Assert.IsType<AccountModel>(response);
            a.Should().BeEquivalentTo(response);

            mockAccountRepository.Verify(v => v.AccountCreate(a));
        }

        /*[Fact]
        public async Task AddAccount_ReturnsBadRequest_GivenInvalidModel()
        {
            // Arrange & Act
            var mockRepo = new Mock<IAccountsRepository>();
            var controller = new AccountsController(mockRepo.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.AccountCreate(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }*/
    }
}
