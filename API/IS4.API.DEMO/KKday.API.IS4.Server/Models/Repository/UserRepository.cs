using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KKday.API.IS4.Server.AppCode.DAL;
using KKday.API.IS4.Server.Models.DataModel.User;

namespace KKday.API.IS4.Server.Models.Repository {
    
    public class UserRepository : IUserRepository {
        // some dummy data. Replce this with your user persistence. 
       
        private List<CustomUser> _users
        {
             get{ return UserDAL.GetAllUser(); }
        } 
        //= new List<CustomUser>
        //{
            //new CustomUser{
            //    SubjectId = "123",
            //    UserName = "damienbod",
            //    Password = "damienbod",
            //    Email = "damienbod@email.ch"
            //},
            //new CustomUser{
            //    SubjectId = "124",
            //    UserName = "raphael",
            //    Password = "raphael",
            //    Email = "raphael@email.ch"
            //},
      //  };

        public bool ValidateCredentials(string username, string password) {
            var user = FindByUsername(username);
            if (user != null) {
                return user.Password.Equals(password);
            }

            return false;
        }

        public CustomUser FindBySubjectId(string subjectId) {
            return _users.FirstOrDefault(x => x.SubjectId == subjectId);
        }

        public CustomUser FindByUsername(string username) {
            return _users.FirstOrDefault(x => x.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
        }


        public async Task<CustomUser> FindAsync(string userName) {

            var _cus =_users.FirstOrDefault(x => x.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));

            return _cus ;
        }

        public async Task<CustomUser> FindAsync(long userId) {

            return _users.FirstOrDefault(x => x.SubjectId == userId.ToString());
        }
    }
}
