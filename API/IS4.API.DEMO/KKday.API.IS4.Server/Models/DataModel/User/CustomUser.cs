using System;
namespace KKday.API.IS4.Server.Models.DataModel.User {

    public class CustomUser {
        
        public string SubjectId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }
}
