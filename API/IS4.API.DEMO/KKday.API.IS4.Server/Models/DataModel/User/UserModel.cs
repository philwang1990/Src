using System;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;

namespace IS4.API.DEMO.Models.DataModel.User {
    public class UserModel

    {
        
        public string USER_NO { get; set; }  //Email
        public string USER_NAME { get; set; }  //暱稱
        public string USER_PASS { get; set; }  //密碼
        public string STATUS { get; set; }  //（預設）00：不允許  01：允許
        public string XID { get; set; } //流水號  
      
    }

}
