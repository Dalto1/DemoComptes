using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using REST.Controllers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class TestAccountsController
    {
        private AccountsController _testAccountsController = null;
        [Fact]
        public void TestAccountCreate()
        {
            AccountModel a = new AccountModel
            {
                AccountNumber = 1,
                AccountBalance = 99,
                AccountCreationDate = DateTime.Now,
                AccountHolderFirstName = "Paul",
                AccountHolderLastName = "Houde",
                IsActive = true
            };
            /*{
                "accountNumber": 1,
                "accountBalance": 99,
                "accountCreationDate": "1999-10-01T00:00:00",
                "isActive": true
            }*/




            var actual = _testAccountsController.AccountCreate(a);

            //Assert
            //Assert.Equal(a, actual, 0);
        }
    }
}
