using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
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
    public class DoctorRecordlnViewModel : INotifyPropertyChanged
    {
        private int doctorId;
        private int seeID;
        public List<TypeofProcModel> AllProcedures { get; set; }
        public ObservableCollection<TypeofProcModel> Procedures { get; set; }
        
        private TypeofProcModel currentProcedure;
        public TypeofProcModel CurrentProcedure
        {
            get { return currentProcedure; }
            set
            {
                currentProcedure = value;
                OnPropertyChanged("CurrentProcedure");
            }
        }
        private TypeofProcModel selectedProcedure;
        public TypeofProcModel SelectedProcedure
        {
            get { return selectedProcedure; }
            set
            {
                selectedProcedure = value;
                OnPropertyChanged("SelectedProcedure");
            }
        }

        private int patientID;
  
        public RecordPatientModel Record { get; set; }
        private DBAccess dbAccess;
        private Window thisWindow;

        public DoctorRecordlnViewModel(int id, Window thisWindow, int patientID, int seeId)
        {
            dbAccess = new DBAccess();
            this.thisWindow = thisWindow;
            seeID = seeId;
            doctorId = id;
            this.patientID = patientID;
            AllProcedures = dbAccess.GetTypeProc();
            Procedures = new ObservableCollection<TypeofProcModel>();
            Record = new RecordPatientModel();
            Record.Diagnosis = "";
            Record.PatientID = patientID;
            Record.DoctorID = id;
            commands();
        }

        private void Back()
        {
            DoctorSees window = new DoctorSees(doctorId);
            window.WindowState = thisWindow.WindowState;
            window.Top = thisWindow.Top;
            window.Left = thisWindow.Left;
            window.Height = thisWindow.Height;
            window.Width = thisWindow.Width;
            window.Show();
            thisWindow.Close();
        }

        private void print()
        {
            Document document = new Document();
            Section section = document.AddSection();
            Paragraph main = new Paragraph();
            Paragraph paragraph = new Paragraph();
            section.Add(main);
            section.Add(paragraph);
            main.Format.Font.Size = 18;
            main.Format.Alignment = ParagraphAlignment.Center;
            main.AddText("Прием у врача \n");
            paragraph.Format.Font.Size = 16;
            paragraph.AddText(Record.ToText());
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true);
            pdfRenderer.Document = document;
            pdfRenderer.RenderDocument();
            pdfRenderer.PdfDocument.Save("Прием" + Record.ID.ToString() + ".pdf");
        }

        private void commands()
        {
            Exit = new Command(obj =>
            {
                Back();
            });
            Submit = new Command(obj =>
            {
                Record.Date = DateTime.Now;
                Record.ID = dbAccess.addPatientRecord(Record);
                DoctorSeeModel see = dbAccess.GetDoctorSee(seeID);
                see.Visited = true;
                dbAccess.UpdateDoctorSee(see);
                foreach (TypeofProcModel s in Procedures)
                {
                    DoProcModel doProc = new DoProcModel();
                    doProc.RecordID = Record.ID;
                    doProc.ProcID = s.ID;
                    doProc.PatientID = patientID;
                    dbAccess.AddDoProc(doProc);
                }
                print();
                Back();
            }, obj => {
                return Record != null && Record.Diagnosis != "";
            });
            PAdd = new Command(obj =>
            {
                Procedures.Add(CurrentProcedure);
            }, obj => {
                return CurrentProcedure != null && Procedures.Where(i => i.ID == CurrentProcedure.ID).FirstOrDefault() == null;
            });
            PRemove = new Command(obj =>
            {
                Procedures.Remove(SelectedProcedure);
            }, obj => {
                return SelectedProcedure != null;
            });
        }

        public Command Exit { get; set; }
        public Command Submit { get; set; }
        public Command PAdd { get; set; }
        public Command PRemove { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
