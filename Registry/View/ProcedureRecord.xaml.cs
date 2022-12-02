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
using Registry.Model;
using Registry.ViewModel;

namespace Registry.View
{
    /// <summary>
    /// Логика взаимодействия для ProcedureRecord.xaml
    /// </summary>
    public partial class ProcedureRecord : Window
    {
        public ProcedureRecord(ProcedureModel proc)
        {
            InitializeComponent();
            DataContext = new ProcedureRecordViewModel(proc, this);
        }
    }
}
