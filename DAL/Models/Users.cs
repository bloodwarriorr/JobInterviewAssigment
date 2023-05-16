using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        public string email { get; set; }    
        public string first_name { get; set; }    
        public string last_name { get; set; }    
        public string avatar { get; set; }

        public Users(int id, string email, string first_name, string last_name, string avatar)
        {
            this.id = id;
            this.email = email;
            this.first_name = first_name;
            this.last_name = last_name;
            this.avatar = avatar;
        }
        public Users()
        {
            
        }
    }
}
