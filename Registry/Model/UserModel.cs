using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registry.Model
{
    public class UserModel
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int? DoctorID { get; set; }
        public int UserType { get; set; }

        public UserModel()
        {

        }

        public UserModel(Users user)
        {
            ID = user.ID;
            Login = user.Login;
            Password = user.Password;
            DoctorID = user.DoctorID;
            UserType = user.UserType;
        }
    }
}
