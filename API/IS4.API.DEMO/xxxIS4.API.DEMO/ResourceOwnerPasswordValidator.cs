using System;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using IdentityServer4.Validation;
using KKday.API.IS4.Server.Models.DataModel.User;
using KKday.API.IS4.Server.Models.Repository;
using static log4net.Appender.RollingFileAppender;

namespace KKday.API.IS4.Server {
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator {
        //repository to get user from db
        private readonly IUserRepository _userRepository;

        public ResourceOwnerPasswordValidator(IUserRepository userRepository) {
            _userRepository = userRepository; //DI
        }

        //this is used to validate your user account with provided grant at /connect/token
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context) {
            try {
                //get your user model from db (by username - in my case its email)
                var user = await _userRepository.FindAsync(context.UserName);
                if (user != null) {
                    //check if password match - remember to hash password if stored as hash in db
                    if (user.Password == context.Password) {
                        //set the result
                        context.Result = new GrantValidationResult(
                            
                            subject: user.SubjectId.ToString(),
                            authenticationMethod: "custom",

                            claims: GetUserClaims(user));

                        return;
                    }

                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Incorrect password");
                    return;
                }
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "User does not exist.");
                return;
            } catch (Exception ex) {

                Console.WriteLine($"{ex.Message},{ex.StackTrace}");
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid username or password");
            }
        }

        //build claims array from user data
        public static Claim[] GetUserClaims(CustomUser user) {
            return new Claim[]
            {
                new Claim("user_id", user.SubjectId ?? ""),
                new Claim(JwtClaimTypes.Name, user.UserName ?? ""),
             
                new Claim(JwtClaimTypes.Email, user.Email  ?? "")

            //roles
            //new Claim(JwtClaimTypes.Role, user.Role)
            };
        }

    }

}
