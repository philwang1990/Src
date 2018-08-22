using System;
using System.Threading.Tasks;
using KKday.API.IS4.Server.Models.DataModel.User;

namespace KKday.API.IS4.Server.Models.Repository {
    public interface IUserRepository {

        bool ValidateCredentials(string username, string password);

        CustomUser FindBySubjectId(string subjectId);

        CustomUser FindByUsername(string username);

        Task<CustomUser> FindAsync(string userName);

        Task<CustomUser> FindAsync(long userId);
    }
}
