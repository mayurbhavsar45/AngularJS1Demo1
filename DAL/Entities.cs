using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Data;

namespace DAL
{
    public class Entities
    {
        public BankTestEntities db = null;

        #region Public

        #region Register
        public string SaveEntity_Addresses(UserDetail entity)
        {
            BankTestEntities db = GetDataContext();
            db.UserDetails.Add(entity);
            db.SaveChanges();

            UserBankAccount uba = new UserBankAccount();
            uba.ID = Guid.NewGuid();
            uba.UserID = entity.UserId;
            uba.AccountNo = null;
            uba.AccountTypeID = null;
            uba.BranchID = null;
            uba.CurrentBalance = 0;
            uba.Status = "notvarify";
            uba.CreatedDate = DateTime.Now.ToUniversalTime();
            uba.ModifiedDate = DateTime.Now.ToUniversalTime();
            db.UserBankAccounts.Add(uba);
            db.SaveChanges();

            Token t = new Token();
            t.UserID = entity.UserId;
            t.Token1 = Utilities.Encrypt(Guid.NewGuid().ToString());
            t.ExpireDate = DateTime.Now.AddHours(24);
            t.IsUsed = false;
            t.Createdate = DateTime.Now.ToUniversalTime();
            db.Tokens.Add(t);
            db.SaveChanges();

            return t.Token1;
        }


        public UserDetail CheckEmailAvailability(string Email)
        {
            BankTestEntities db = GetDataContext();
            var obj = (from TL in db.UserDetails
                       join b in db.UserBankAccounts on TL.UserId equals b.UserID
                       where TL.Email.Equals(Email) select TL).FirstOrDefault();
            return obj;
        }




        #endregion Register

        #region Token

        #region Get Token by token
        public DAL.Token GetTokenByToken(string Token)
        {

            BankTestEntities db = GetDataContext();
            var token = db.Tokens.Where(x => x.Token1 == Token).FirstOrDefault();
            return token;
        }

        public bool ValidateToken(string Token)
        {
            string decrypttoken = Utilities.Decrypt(Token);
            BankTestEntities db = GetDataContext();
            var token = db.Tokens.Where(x => x.Token1 == Token).FirstOrDefault();
            if (Utilities.Decrypt(token.Token1) == decrypttoken && token.ExpireDate > DateTime.Now && token.IsUsed == false)
            {
                token.IsUsed = true;
                db.SaveChanges();

                string AccountNo = "";
                do
                {
                    AccountNo = Utilities.GetAccountNo();
                } while (CheckAccountNumberExistsOrNot(AccountNo));

                var useraccount = db.UserBankAccounts.Where(x => x.UserID == token.UserID).FirstOrDefault();
                useraccount.Status = "varified";
                useraccount.AccountNo = AccountNo;
                useraccount.BranchID = 1;
                useraccount.AccountTypeID = 1;
                useraccount.ModifiedDate = DateTime.Now.ToUniversalTime();
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool CheckAccountNumberExistsOrNot(string AcNo)
        {
            BankTestEntities db = GetDataContext();
            if (db.UserBankAccounts.Where(x => x.AccountNo == AcNo).FirstOrDefault() != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        #endregion

        #endregion Token

        #region Login

        public UserDetail ValidateLogin(string Email, string Password)
        {

            BankTestEntities db = GetDataContext();

            var obj = (from TL in db.UserDetails join b in db.UserBankAccounts on TL.UserId equals b.UserID
                       where TL.Email.Equals(Email) && TL.Password.Equals(Password) && b.Status.Equals("varified")
                       select TL).FirstOrDefault();
            return obj;
        }

        public UserDetail ValidateTransactionLogin(string userId, string TransactionPassword)
        {

            BankTestEntities db = GetDataContext();
            var UID = Guid.Parse(userId);
            TransactionPassword = Utilities.Encrypt(TransactionPassword);
            var obj = (from TL in db.UserDetails
                       where TL.UserId == UID && TL.TransactionPassword.Equals(TransactionPassword)
                       select TL).FirstOrDefault();
            return obj;
        }

        #endregion Login

        #region Account

        public UserBankAccount GetAccountInfo(string userID)
        {

            BankTestEntities db = GetDataContext();
            var UID = Guid.Parse(userID);
            var obj = db.UserBankAccounts.Where(x => x.UserID == UID).FirstOrDefault();
            return obj;
        }
        public List<UserTransaction> GetAccountTransactions(string userID)
        {

            BankTestEntities db = GetDataContext();
            var UID = Guid.Parse(userID);
            List<UserTransaction> objTransactions = db.UserTransactions.OrderBy(x => x.TransactionDate).Where(x => x.UserId == UID).ToList();
            return objTransactions;
        }
        #endregion Account

        #region Transfer

        public UserBankAccount GetAccountInfoByACNO(string AccountNo)
        {
            BankTestEntities db = GetDataContext();
            var obj = db.UserBankAccounts.Where(x => x.AccountNo == AccountNo).FirstOrDefault();
            return obj;
        }

        #endregion Transfer

        #region Deposit
        public string SaveEntity_UserTrasaction(UserTransaction entity)
        {
            try
            {
                BankTestEntities db = GetDataContext();
                db.UserTransactions.Add(entity);
                db.SaveChanges();

                var userBankDetail = db.UserBankAccounts.Where(x => x.UserID == entity.UserId).FirstOrDefault();
                if (userBankDetail != null)
                {
                    userBankDetail.CurrentBalance = entity.Balance;
                    db.SaveChanges();
                }
                return "1";
            }
            catch (Exception ex)
            {
                return "0";
            }

        }
        #endregion Deposit

        #endregion public

        #region Private

        public BankTestEntities GetDataContext()
        {
            return new BankTestEntities();
        }

        #endregion Private
    }
}
