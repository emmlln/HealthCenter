using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Registry.ViewModel;

namespace Registry.View
{
    /// <summary>
    /// Логика взаимодействия для DoctorSees.xaml
    /// </summary>
    public partial class DoctorSees : Window
    {
        public DoctorSees(int id)
        {
            InitializeComponent();
            DataContext = new DoctorSeesViewModel(id, this);
        }
    }
}
