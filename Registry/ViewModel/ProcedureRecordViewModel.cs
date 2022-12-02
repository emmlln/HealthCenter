using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
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
    public class ProcedureRecordViewModel : INotifyPropertyChanged
    {
        public List<NurceModel> Nurces { get; set; }

        private NurceModel selectedNurce;
        public NurceModel SelectedNurce
        {
            get { return selectedNurce; }
            set
            {
                selectedNurce = value;
                OnPropertyChanged("SelectedNurce");
            }
        }
        private ProcedureModel procedure;
        public RecordPatientModel Record { get; set; }
        private DBAccess dbAccess;
        private Window thisWindow;

        public ProcedureRecordViewModel(ProcedureModel proc, Window thisWindow)
        {
            dbAccess = new DBAccess();
            procedure = proc;
            this.thisWindow = thisWindow;
            Nurces = dbAccess.GetNurcesForProcedure(procedure.DoProc.ProcID);
            Record = new RecordPatientModel();
            Record.Diagnosis = "";
            Record.PatientID = procedure.PatientID;
            Record.Diagnosis = dbAccess.GetTypeProc(procedure.DoProc.ProcID).Type;
            commands();
        }

        private void Back()
        {
            Procedural window = new Procedural();
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
            Paragraph paragraph = new Paragraph();
            section.Add(paragraph);
            paragraph.Format.Font.Size = 16;
            paragraph.AddText(Record.ToText());
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true);
            pdfRenderer.Document = document;
            pdfRenderer.RenderDocument();
            pdfRenderer.PdfDocument.Save("Процедура" + Record.ID.ToString() + ".pdf");
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
                Record.NurseID = SelectedNurce.ID;
                Record.ID = dbAccess.addPatientRecord(Record);
                procedure.Visited = true;
                dbAccess.UpdateProcedure(procedure);
                print();
                Back();
            }, obj =>
            {
                return Record != null && Record.Result != "" && SelectedNurce != null;
            });
        }

        public Command Exit { get; set; }
        public Command Submit { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
