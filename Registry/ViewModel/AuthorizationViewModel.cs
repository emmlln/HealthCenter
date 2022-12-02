using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Registry.Model;
using Registry.View;

namespace Registry.ViewModel
{
    public class AuthorizationViewModel : INotifyPropertyChanged
    {
        private string login;
        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged("Login");
            }
        }

        private DBAccess db;
        private MainWindow window;
        public AuthorizationViewModel(MainWindow autorization)
        {
            window = autorization;
            db = new DBAccess();
            setEnterCommand();
        }

        private void setEnterCommand()
        {
            Enter = new Command(obj =>
            {
                PasswordBox password = obj as PasswordBox;
                if (password != null)
                {
                    UserModel user = db.GetUserByLogin(login);
                    if (user != null)
                    {
                        if (user.Password == password.Password)
                        {
                            if(user.UserType == 1)
                            {
                                DoctorSees reg = new DoctorSees(user.DoctorID.Value);
                                reg.Show();
                                window.Close();
                            }
                            if (user.UserType == 0)
                            {
                                RegistrationMain reg = new RegistrationMain();
                                reg.Show();
                                window.Close();
                            }
                            if (user.UserType == 2)
                            {
                                Procedural reg = new Procedural();
                                reg.Show();
                                window.Close();
                            }
                        }
                        else
                        {
                            password.Password = "";
                            MessageBox.Show("Пароль неверный", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Пользователь не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            },
                  obj => { return login != ""; });
        }

        public Command Enter { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
