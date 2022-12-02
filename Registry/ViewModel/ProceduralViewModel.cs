using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Registry.Model;
using Registry.View;

namespace Registry.ViewModel
{
    public class ProceduralViewModel : INotifyPropertyChanged
    {
        private DBAccess dbAccess;
        private Window thisWindow;

        public ObservableCollection<ProcedureModel> Procedures { get; set; }

        private ProcedureModel selectedProcedure;
        public ProcedureModel SelectedProcedure
        {
            get { return selectedProcedure; }
            set
            {
                selectedProcedure = value;
                OnPropertyChanged("SelectedProcedure");
            }
        }

        public ProceduralViewModel(Window thisWindow)
        {
            dbAccess = new DBAccess();
            this.thisWindow = thisWindow;
            Procedures = new ObservableCollection<ProcedureModel>(dbAccess.GetProcedures(DateTime.Now));
            SelectedProcedure = null;
            commands();
        }

        private void commands()
        {
            Exit = new Command(obj =>
            {
                MainWindow window = new MainWindow();
                window.Show();
                thisWindow.Close();
            });
            NotVisited = new Command(obj =>
            {
                SelectedProcedure.Visited = false;
                dbAccess.UpdateProcedure(SelectedProcedure);
                Procedures.Remove(SelectedProcedure);
            }, obj => {
                return SelectedProcedure != null;
            });
            Write = new Command(obj =>
            {
                Window window = new ProcedureRecord(SelectedProcedure);
                window.WindowState = thisWindow.WindowState;
                window.Top = thisWindow.Top;
                window.Left = thisWindow.Left;
                window.Height = thisWindow.Height;
                window.Width = thisWindow.Width;
                window.Show();
                thisWindow.Close();
            }, obj => {
                return SelectedProcedure != null;
            });
        }

        public Command NotVisited { get; set; }
        public Command Exit { get; set; }
        public Command Write { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
