using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Registry.Model;

namespace Registry.ViewModel
{
    public class NurcesViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<NurceModel> nurces;
        public ObservableCollection<NurceModel> Nurces
        {
            get { return nurces; }
            set
            {
                nurces = value;
                OnPropertyChanged("Nurces");
            }
        }
        public List<TypeofProcModel> Procedures { get; set; }
        public NurceModel selectedNurce;
        public NurceModel SelectedNurce
        {
            get { return selectedNurce; }
            set
            {
                selectedNurce = value;
                CurrentProcedure = null;
                if (value != null)
                {
                    FIO = selectedNurce.FullName;
                    WorkEx = selectedNurce.WorkEx.ToString();
                }
                else
                {
                    FIO = "";
                    WorkEx = "";
                }
                OnPropertyChanged("SelectedNurce");
            }
        }

        public TypeofProcModel currentProcedure;
        public TypeofProcModel CurrentProcedure
        {
            get { return currentProcedure; }
            set
            {
                currentProcedure = value;
                OnPropertyChanged("CurrentProcedure");
            }
        }

        public TypeofProcModel selectedProcedure;
        public TypeofProcModel SelectedProcedure
        {
            get { return selectedProcedure; }
            set
            {
                selectedProcedure = value;
                if (value != null)
                {
                    CurrentProcedure = Procedures.Where(i => i.ID == selectedProcedure.ID).FirstOrDefault();
                }
                OnPropertyChanged("SelectedProcedure");
            }
        }

        public string fio;
        public string FIO
        {
            get { return fio; }
            set
            {
                fio = value;
                OnPropertyChanged("FIO");
            }
        }

        public string workEx;
        public string WorkEx
        {
            get { return workEx; }
            set
            {
                workEx = value;
                OnPropertyChanged("WorkEx");
            }
        }
        
        private DBAccess dbAccess;
        private Window thisWindow;

        public NurcesViewModel(Window thisWindow)
        {
            dbAccess = new DBAccess();
            Procedures = dbAccess.GetTypeProc();
            this.thisWindow = thisWindow;
            setNurces();
            commands();
        }

        private void setNurces()
        {
            SelectedNurce = null;
            CurrentProcedure = null;
            Nurces = new ObservableCollection<NurceModel>(dbAccess.GetNurces().OrderBy(i => i.Closed).ToList());
            foreach (NurceModel n in Nurces)
            {
                n.SetProcedures();
            }
        }
        private void commands()
        {
            Exit = new Command(obj =>
            {
                RegistrationMain window = new RegistrationMain();
                window.WindowState = thisWindow.WindowState;
                window.Top = thisWindow.Top;
                window.Left = thisWindow.Left;
                window.Height = thisWindow.Height;
                window.Width = thisWindow.Width;
                window.Show();
                thisWindow.Close();
            });
            Create = new Command(obj =>
            {
                NurceModel nurce = new NurceModel();
                nurce.FullName = fio;
                nurce.WorkEx = int.Parse(workEx);
                nurce.ID = dbAccess.AddNurce(nurce);
                Nurces.Add(nurce);
                nurce.SetProcedures();
            }, obj =>
            {
                return
                fio != "" &&
                workEx != "" && int.TryParse(workEx, out int b);
            });
            Update = new Command(obj =>
            {
                selectedNurce.FullName = fio;
                selectedNurce.WorkEx = int.Parse(workEx);
                dbAccess.UpdateNurce(selectedNurce);
            }, obj =>
            {
                return
                fio != "" &&
                workEx != "" && int.TryParse(workEx, out int b);
            });
            Restore = new Command(obj =>
            {
                selectedNurce.Closed = false;
                dbAccess.UpdateNurce(selectedNurce);
                setNurces();
            }, obj =>
            {
                return
                selectedNurce != null && selectedNurce.Closed == true;
            });
            Delete = new Command(obj =>
            {
                selectedNurce.Closed = true;
                dbAccess.UpdateNurce(selectedNurce);
                setNurces();
            }, obj =>
            {
                return
                selectedNurce != null && selectedNurce.Closed == false;
            });
            MAdd = new Command(obj =>
            {
                TypeofProcModel p = new TypeofProcModel();
                p.ID = currentProcedure.ID;
                p.Type = currentProcedure.Type;
                SelectedNurce.Procedures.Add(p);
                MakingProcedureModel m = new MakingProcedureModel();
                m.NurceID = selectedNurce.ID;
                m.ProcedureID = p.ID;
                m.ID = dbAccess.AddMakingProcedures(m);
                selectedNurce.MakingProcedures.Add(m);
            }, obj => {
                return currentProcedure != null
                && (selectedNurce == null || selectedNurce.Procedures.Where(i => i.ID == currentProcedure.ID).FirstOrDefault() == null);
            });
            MRemove = new Command(obj =>
            {
                int index = SelectedNurce.Procedures.IndexOf(selectedProcedure);
                SelectedNurce.Procedures.Remove(selectedProcedure);
                dbAccess.removeMakingProc(selectedNurce.MakingProcedures[index].ID);
                SelectedNurce.MakingProcedures.RemoveAt(index);
            }, obj => {
                return selectedProcedure != null;
            });
            MChange = new Command(obj =>
            {
                int index = SelectedNurce.Procedures.IndexOf(selectedProcedure);
                selectedProcedure.ID = currentProcedure.ID;
                selectedProcedure.Type = currentProcedure.Type;
                SelectedNurce.MakingProcedures[index].ProcedureID = currentProcedure.ID;
                dbAccess.UpdateMakingProcedures(SelectedNurce.MakingProcedures[index]);
            }, obj => {
                return currentProcedure != null && selectedProcedure != null
                && selectedNurce.Procedures.Where(i => i.ID == currentProcedure.ID).FirstOrDefault() == null;
            });
        }
        public Command MAdd { get; set; }
        public Command MRemove { get; set; }
        public Command MChange { get; set; }
        public Command Exit { get; set; }
        public Command Create { get; set; }
        public Command Update { get; set; }
        public Command Restore { get; set; }
        public Command Delete { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
