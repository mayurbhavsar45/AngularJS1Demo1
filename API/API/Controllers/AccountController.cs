using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using BLL;
using API.Models;
using System.Configuration;
using AttributeRouting.Web.Http;
using Common;

namespace API.Controllers
{
    public class AccountController : ApiController
    {
        //
        // GET: /Account/
        [System.Web.Http.HttpGet]
        [GET("Account/Getinformation/{id}")]
        public string Getinformation(string id)
        {
            AccountInfo CT = BAccount.GetAccountInfo(id);
            if (CT != null)
            {
                return Utilities.JSONSerialize(CT);
            }
            return "";
        }

        [System.Web.Http.HttpGet]
        [GET("Account/GetTransactions/{id}")]
        public string GetTransactions(string id)
        {
            List<AccountStatement> objACStatements = BAccount.GetAccountTransactions(id);
            if (objACStatements != null)
            {
                return Utilities.JSONSerialize(objACStatements);
            }
            return "";
        }
        
        [System.Web.Http.HttpGet]
        [GET("Account/GetcheckAccountAvailability/{id}")]
        public string GetcheckAccountAvailability(string id)
        {
            AccountInfo CT = BTransfer.checkAccountAvailability(id);
            if (CT != null)
            {
                return Utilities.JSONSerialize(CT);
            }
            return "";         
        }       
        
        [System.Web.Http.HttpPost]
        [POST("Account/Deposit")]
        public string Deposit(Deposit_model @deposit)
        {
            return BDeposit.CashDeposit(@deposit);
        }


        [System.Web.Http.HttpPost]
        [POST("Account/Withdraw")]
        public string Withdraw(Withdraw_model @withdraw)
        {
            return BWithdraw.CashWithdraw(@withdraw);
        }

        [System.Web.Http.HttpPost]
        [POST("Account/TransferDeposit")]
        public string TransferDeposit(Deposit_model @Transferdeposit)
        {
            return BTransfer.Transfer_Deposit(@Transferdeposit);
        }

                 
    }
}
