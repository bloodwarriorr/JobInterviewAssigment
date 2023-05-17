using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class UserService : IUsersService
    {
        private readonly UserDbContext _usersContext;
        public UserService()
        {
            _usersContext= new UserDbContext();
        }
        public void AddUser(Users model)
        {
            if (!IsExist(model.id))
            {
                _usersContext.usersDTO.Add(model);
            }
            
            _usersContext.SaveChanges();
        }
        public void AddUsers(IEnumerable<Users> model)
        {
            List<Users> users = new List<Users>();
            foreach (Users item in model)
            {
                if (!IsExist(item.id))
                {
                    users.Add(item);
                }
            }
            _usersContext.usersDTO.AddRange(users);
            _usersContext.SaveChanges();
            
        }
        public bool IsEmpty()
        {
            return _usersContext.usersDTO.Count() == 0;
        }
        public void DeleteUser(int id)
        {
            Users exists = GetUser(id);
            _usersContext.usersDTO.Remove(exists);
            _usersContext.SaveChanges();
        }

        public async Task<IEnumerable<Users>> GetAll()
        {
            return await _usersContext.usersDTO.ToListAsync();


        }
        public bool IsExist(int id)
        {
            Users exists= GetUser(id);
            return exists != null;
        }
        public Users GetUser(int id)
        {
           
            Users singleUser= _usersContext.usersDTO.FirstOrDefault(user => user.id == id);
            return singleUser;
        }

        public void UpdateUser(Users model)
        {
            _usersContext.usersDTO.AddOrUpdate(model);
            _usersContext.SaveChanges();
        }

        
    }
}
