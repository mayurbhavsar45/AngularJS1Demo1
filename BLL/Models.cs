using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{

    public enum  CTransactionType
    {
        Deposit = 1,
        Withdraw = 2,
        Transfer = 3
    }

    #region Register_model

    public class Register_model
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string EmailID { get; set; }

        public string Password { get; set; }
        public string TransactionPassword { get; set; }
 
    }

    #endregion Register_model

    #region EmailLogin_model
    public class CBannkLogin
    {
        public Guid ID { get; set; }
        public string EmailID { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
    }

    #endregion EmailLogin_model

#region Account_Model
    public class AccountInfo
    {
        public Guid? UserID { get; set; }
        public string Username { get; set; }
        public string AccountNo { get; set; }

        public string BranchName { get; set; }

        public string AccountType { get; set; }

        public decimal? CurrentBalance { get; set; }

        public string Status { get; set; }

        public DateTime? StatusDate { get; set; }

    }

    public class AccountStatement
    {
        public int Index { get; set; }
        public DateTime? TrancationDate { get; set; }
        public string Remark { get; set; }

        public string TransactionType { get; set; }

        public decimal? Amount { get; set; }

    }


#endregion Account_Model

#region Deposit_model
    public class Deposit_model
    {
        public string UserID { get; set; }
        public string DepositAmount { get; set; }
        public string TransactionPassword { get; set; }
        public string Remark { get; set; }
        public Guid Transfer_UserID { get; set; }
    }
#endregion Deposit_model

    #region Withdraw_model
    public class Withdraw_model
    {
        public string UserID { get; set; }
        public string WithdrawAmount { get; set; }
        public string TransactionPassword { get; set; }
        public string Remark { get; set; } 
    }
    #endregion Withdraw_model

   
  

}
