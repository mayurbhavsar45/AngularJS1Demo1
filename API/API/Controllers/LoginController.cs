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
    public class LoginController : ApiController
    {

        public CBannkLogin Get()
        {
            return null;
        }

        [System.Web.Http.HttpGet]
        [GET("Login/ValidateLogin/{Email}/{Password}")]
        public string ValidateLogin(string Email, string Password)
        {
            CBannkLogin CT = BLogin.ValidateLogin(Email, Common.Utilities.Encrypt(Password));
            if (CT != null)
            {
                return Utilities.JSONSerialize(CT);
            }
            return "";
        }
    }
}
