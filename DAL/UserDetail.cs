//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserDetail
    {
        public UserDetail()
        {
            this.UserBankAccounts = new HashSet<UserBankAccount>();
            this.UserTransactions = new HashSet<UserTransaction>();
            this.Tokens = new HashSet<Token>();
        }
    
        public System.Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> RegisterDate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Password { get; set; }
        public string TransactionPassword { get; set; }
    
        public virtual ICollection<UserBankAccount> UserBankAccounts { get; set; }
        public virtual ICollection<UserTransaction> UserTransactions { get; set; }
        public virtual ICollection<Token> Tokens { get; set; }
    }
}