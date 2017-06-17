using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Common;

namespace BLL
{
    #region Register

    public static class Register
    {
        #region New Register

        public static string NewRegister(Register_model _register)
        {

            var user = new UserDetail
            {
                UserId = Guid.NewGuid(),
                FirstName = _register.FirstName,
                LastName = _register.LastName,
                Address = _register.Address,
                Mobile = _register.Mobile,
                Email = _register.EmailID,
                RegisterDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Password = Utilities.Encrypt(_register.Password),
                TransactionPassword = Utilities.Encrypt(_register.TransactionPassword)
            };

            return HelperMethods.GetEntities().SaveEntity_Addresses(user);
        }

        #endregion New Register

        public static Boolean CheckEmailAvailability(string email)
        {
            DAL.UserDetail CLoginuser = HelperMethods.GetEntities().CheckEmailAvailability(email);
            if (CLoginuser != null)
            {
                return true;
            }
            return false;
        }
    }

    #endregion Register

    #region Token
    public static class Token
    {
        #region Get Token by token
        public static DAL.Token GetTokenByToken(string Token)
        {
            DAL.Token token = HelperMethods.GetEntities().GetTokenByToken(Token);

            return token;
        }

        public static bool ValidateToken(string Token)
        {
            return HelperMethods.GetEntities().ValidateToken(Token);
        }
        #endregion

    }
    #endregion

    #region Login

    public static class BLogin
    {
        public static CBannkLogin ValidateLogin(string email, string password)
        {
            DAL.UserDetail CLoginuser = HelperMethods.GetEntities().ValidateLogin(email, password);
            if (CLoginuser != null)
            {
                return new CBannkLogin { ID = CLoginuser.UserId, EmailID = CLoginuser.Email, Password = "", Username = (CLoginuser.FirstName + " " + CLoginuser.LastName) };
            }
            return null;


        }
    }
    #endregion Login

    #region Account

    public static class BAccount
    {
        public static AccountInfo GetAccountInfo(string userid)
        {
            try
            {
                DAL.UserBankAccount CAccountInfo = HelperMethods.GetEntities().GetAccountInfo(userid);
                if (CAccountInfo != null)
                {
                    return new AccountInfo { AccountNo = CAccountInfo.AccountNo, AccountType = CAccountInfo.AccountType.AccountTypeName, BranchName = CAccountInfo.Branch.BranchName, CurrentBalance = CAccountInfo.CurrentBalance, Username = (CAccountInfo.UserDetail.FirstName + " " + CAccountInfo.UserDetail.LastName), Status = CAccountInfo.Status, StatusDate = CAccountInfo.ModifiedDate, UserID = CAccountInfo.UserID };
                }
                return null;
            }
            catch
            {
                return null;
            }

        }

        public static List<AccountStatement> GetAccountTransactions(string userid)
        {
            List<DAL.UserTransaction> CAccountTransactions = HelperMethods.GetEntities().GetAccountTransactions(userid);
            if (CAccountTransactions != null)
            {
                List<AccountStatement> objAcStatements = new List<AccountStatement>();
                int I = 1;
                foreach (var item in CAccountTransactions)
                {
                    objAcStatements.Add(new AccountStatement { Index = I++, TrancationDate = item.TransactionDate, Remark = item.Remark, Amount = ((item.CRAmount != null) ? item.CRAmount : item.DRAmount), TransactionType = item.TransactionType.Name });
                }
                return objAcStatements;
            }
            return null;
        }
    }
    #endregion Account

    #region Deposit

    public static class BDeposit
    {
        #region Cash Deposit

        public static string CashDeposit(Deposit_model _deposit)
        {
            DAL.UserDetail CLoginuser = HelperMethods.GetEntities().ValidateTransactionLogin(_deposit.UserID, _deposit.TransactionPassword);
            if (CLoginuser != null)
            {
                DAL.UserBankAccount CAccountInfo = HelperMethods.GetEntities().GetAccountInfo(_deposit.UserID);
                if (CAccountInfo != null)
                {
                    var cashdeposit = new UserTransaction
                    {
                        ID = Guid.NewGuid(),
                        TransactionTypeID = (byte)CTransactionType.Deposit,
                        CRAmount = Convert.ToDecimal(_deposit.DepositAmount),
                        DRAmount = null,
                        Balance = (CAccountInfo.CurrentBalance + Convert.ToDecimal(_deposit.DepositAmount)),
                        TransactionDate = DateTime.Now.ToUniversalTime(),
                        CreatedDate = DateTime.Now.ToUniversalTime(),
                        UserId = CLoginuser.UserId,
                        Remark = _deposit.Remark
                    };

                    return HelperMethods.GetEntities().SaveEntity_UserTrasaction(cashdeposit);
                }
                else
                {
                    return "-2";
                }

            }
            return "-1";
        }

        #endregion
    }

    #endregion Deposit

    #region Withdraw

    public static class BWithdraw
    {
        #region Cash Withdraw

        public static string CashWithdraw(Withdraw_model _withdraw)
        {
            DAL.UserDetail CLoginuser = HelperMethods.GetEntities().ValidateTransactionLogin(_withdraw.UserID, _withdraw.TransactionPassword);
            if (CLoginuser != null)
            {
                DAL.UserBankAccount CAccountInfo = HelperMethods.GetEntities().GetAccountInfo(_withdraw.UserID);
                if (CAccountInfo != null)
                {
                    if (CAccountInfo.CurrentBalance > Convert.ToDecimal(_withdraw.WithdrawAmount))
                    {
                        var cashwithdraw = new UserTransaction
                        {
                            ID = Guid.NewGuid(),
                            TransactionTypeID = (byte)CTransactionType.Withdraw,
                            DRAmount = Convert.ToDecimal(_withdraw.WithdrawAmount),
                            CRAmount = null,
                            Balance = (CAccountInfo.CurrentBalance - Convert.ToDecimal(_withdraw.WithdrawAmount)),
                            TransactionDate = DateTime.Now.ToUniversalTime(),
                            CreatedDate = DateTime.Now.ToUniversalTime(),
                            UserId = CLoginuser.UserId,
                            Remark = _withdraw.Remark
                        };

                        return HelperMethods.GetEntities().SaveEntity_UserTrasaction(cashwithdraw);
                    }
                    else
                    {
                        return "-3";
                    }
                }
                else
                {
                    return "-2";
                }

            }
            return "-1";
        }

        #endregion
    }

    #endregion Deposit

    #region Transfer

    public static class BTransfer
    {
        #region Transfer Money

        public static AccountInfo checkAccountAvailability(string AccountNo)
        {           
            try
            {
                DAL.UserBankAccount CAccountInfo = HelperMethods.GetEntities().GetAccountInfoByACNO(AccountNo);
                if (CAccountInfo != null)
                {
                    return new AccountInfo { AccountNo = CAccountInfo.AccountNo, AccountType = CAccountInfo.AccountType.AccountTypeName, BranchName = CAccountInfo.Branch.BranchName, CurrentBalance = CAccountInfo.CurrentBalance, Username = (CAccountInfo.UserDetail.FirstName + " " + CAccountInfo.UserDetail.LastName), Status = CAccountInfo.Status, StatusDate = CAccountInfo.ModifiedDate, UserID = CAccountInfo.UserID };
                }
                return null;
            }
            catch (Exception)
            {
                return null;
                throw;
            }            
        }


        public static string Transfer_Deposit(Deposit_model _deposit)
        {            
                DAL.UserBankAccount CAccountInfo = HelperMethods.GetEntities().GetAccountInfo(_deposit.UserID);
                if (CAccountInfo != null)
                {
                    var cashdeposit = new UserTransaction
                    {
                        ID = Guid.NewGuid(),
                        TransactionTypeID = (byte)CTransactionType.Deposit,
                        CRAmount = Convert.ToDecimal(_deposit.DepositAmount),
                        DRAmount = null,
                        Balance = (CAccountInfo.CurrentBalance + Convert.ToDecimal(_deposit.DepositAmount)),
                        TransactionDate = DateTime.Now.ToUniversalTime(),
                        CreatedDate = DateTime.Now.ToUniversalTime(),
                        UserId = _deposit.Transfer_UserID,
                        Remark = _deposit.Remark
                    };

                    return HelperMethods.GetEntities().SaveEntity_UserTrasaction(cashdeposit);
                }
                else
                {
                    return "-2";
                }
                        
            return "-1";
        }

        #endregion
    }

    #endregion Transfer

    #region HELPER METHODS
    public class HelperMethods
    {
        public static Entities GetEntities()
        {
            return new Entities();
        }

        //public static int GetObjectTypeId(string name)
        //{
        //    var orgTypes = HelperMethods.GetEntities().GetAll_APPLICATION_Object_Types();

        //    return orgTypes.FirstOrDefault(f => f.Title.ToLower() == name.ToLower()).Id;
        //}

        //public static void ClearEventFromCache(int id)
        //{
        //    CacheHelper.ClearCacheItem("GetEvent-" + id);
        //    CacheHelper.ClearCacheItem("GetEventOptimized-" + id);
        //    CacheHelper.ClearCacheItem("EventSummary-" + id);
        //    CacheHelper.ClearCacheItem("AdminEventReport-" + id);
        //    CacheHelper.ClearCacheItem("ParticipantsByEvent-" + id);
        //    CacheHelper.ClearCacheItem("EventComments-" + id);
        //    CacheHelper.ClearCacheItem("GetRecentPublicEvents");
        //    CacheHelper.ClearCacheItem("EventParticipants-" + id);
        //    CacheHelper.ClearCacheItem("AdminParticipantsReport-" + id);
        //    CacheHelper.ClearCacheItem("AdminPledgesReport-" + id);
        //    CacheHelper.ClearCacheItem("AdminProgressReport-" + id);
        //    CacheHelper.ClearCacheItem("GetEventForPayment-" + id);
        //}

        ///// <summary>
        ///// to Clear Cache After Update Total Units Completed
        ///// </summary>
        //public static void ClearCacheAfterUpdateTotalUnitsCompleted(int eventId)
        //{
        //    CacheHelper.ClearCacheItem("AdminParticipantsReport-" + eventId);
        //    CacheHelper.ClearCacheItem("AdminPledgesReport-" + eventId);
        //    CacheHelper.ClearCacheItem("AdminProgressReport-" + eventId);
        //}

        ///// <summary>
        ///// to Clear Participant From Cache
        ///// </summary>
        //public static void ClearParticipantFromCache(int participantId)
        //{
        //    CacheHelper.ClearCacheItem("Participant-" + participantId);
        //}

        //public static User_model GetCachedUser(int id)
        //{
        //    return Utilities.HelperMethods.Cacheable(() => User.GetUserById(id), "User-" + id);
        //}
    }

    #endregion HELPER METHODS


}
