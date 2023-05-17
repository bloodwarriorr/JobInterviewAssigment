using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public interface IUsersService
    {
        Task<IEnumerable<Users>> GetAll(); 
        Users GetUser(int id);
        void UpdateUser(Users model);
        void AddUser(Users model);
        void AddUsers(IEnumerable<Users> model);
        void DeleteUser(int id);
        bool IsEmpty();
        bool IsExist(int id);
    }
}
