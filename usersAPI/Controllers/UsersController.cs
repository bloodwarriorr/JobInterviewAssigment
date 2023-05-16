using DAL;
using DAL.Services;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Routing;

namespace usersAPI.Controllers
{
    public class UsersController : ApiController
    {
        IUsersService _usersService = new UserService();
        [Route("getUsers/{pageNumber:int}")]
       
        public async Task<IHttpActionResult> GetUsers(int pageNumber)
        {
            try
            {

                IEnumerable<Users> httpUsersRequest = await HttpRequestFunc(pageNumber, -1);
                _usersService.AddUsers(httpUsersRequest);     
                return Ok(httpUsersRequest);
            }

            catch (Exception)
            {

                return NotFound();
            }
        }
        [Route("getUser/{id:int}")]
       
        public async Task<IHttpActionResult> GetUser(int id)
        {
            try
            {
                if (!_usersService.IsExist(id))
                {
                    IEnumerable<Users> httpUsersRequest = await HttpRequestFunc(-1, id);
                    _usersService.AddUsers(httpUsersRequest);
                }
                Users user = await _usersService.GetUser(id);
                return Ok(user);
            }

            catch (Exception)
            {

              return NotFound();
            }
        }
        [Route("createUser")]

        public IHttpActionResult PostUser([FromBody] Users user)
        {
            try
            {
                if (_usersService.IsExist(user.id))
                {
                    return BadRequest("user already exists!");
                }
                _usersService.AddUser(user);
                return Ok("User Created successfully!");
            }
            catch (Exception)
            {
                return NotFound();
            }

        }
        [Route("updateUser/{id:int}")]
        public IHttpActionResult PutUser([FromBody] Users user)
        {
            try
            {
                if (!_usersService.IsExist(user.id))
                {
                    return BadRequest("user doesn't exists!");
                }
                _usersService.UpdateUser(user);
                return Ok("User updated successfully!");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        [Route("deleteUser/{id:int}")]
        public IHttpActionResult DeleteUser([FromBody] Users user)
        {
            try
            {
                if (!_usersService.IsExist(user.id))
                {
                    return BadRequest("user doesn't exists!");
                }
                _usersService.DeleteUser(user);
                return Ok("User deleted successfully!");
            }
            catch (Exception)
            {
                return NotFound();
            }

        }

        //this functions is a help method to retrive data from the external api.
        //in order to reuse code, it has 2 arguments, to decide which get request to apply
        //users by id or users by page.
        private async Task<IEnumerable<Users>> HttpRequestFunc(int pageNumber = -1, int id = -1)
        {
            string requestString = "https://reqres.in/api/users/0";
            if (pageNumber != -1)
            {
                requestString = $"https://reqres.in/api/users?page={pageNumber}";
            }
            else if (id != -1)
            {
                requestString = $"https://reqres.in/api/users/{id}";
            }
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(requestString),

                };

                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    string result = await response.Content.ReadAsStringAsync();
                    JsonDocument jsonDocument = JsonDocument.Parse(result);
                    JsonElement dataElement = jsonDocument.RootElement.GetProperty("data");
                    string dataValue = dataElement.ToString();
                    if (pageNumber != -1)
                    {

                        Users[] users = JsonConvert.DeserializeObject<Users[]>(dataValue);
                        return users;

                    }
                    else
                    {
                        List<Users> list = new List<Users>();
                        Users user = JsonConvert.DeserializeObject<Users>(dataValue);
                        list.Add(user);
                        return list;
                    }

                }
            }

            catch (Exception)
            {

                return null;
            }
        }
    }
}
