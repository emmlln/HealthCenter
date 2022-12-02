using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Registry.View;

namespace Registry.ViewModel
{
    public class MainViewModel
    {
        public MainViewModel(Window window)
        {
            RegistrationProc = new Command(obj =>
            {
                ProcedureRegistration window1 = new ProcedureRegistration();
                window1.WindowState = window.WindowState;
                window1.Top = window.Top;
                window1.Left = window.Left;
                window1.Height = window.Height;
                window1.Width = window.Width;
                window1.Show();
                window.Close();
            });
            Registration = new Command(obj =>
            {
                Registration window1 = new Registration();
                window1.WindowState = window.WindowState;
                window1.Top = window.Top;
                window1.Left = window.Left;
                window1.Height = window.Height;
                window1.Width = window.Width;
                window1.Show();
                window.Close();
            });
            Test = new Command(obj =>
            {
                Changing window1 = new Changing();
                window1.WindowState = window.WindowState;
                window1.Top = window.Top;
                window1.Left = window.Left;
                window1.Height = window.Height;
                window1.Width = window.Width;
                window1.Show();
                window.Close();
            });
            Card = new Command(obj =>
            {
                PatientCard window1 = new PatientCard();
                window1.WindowState = window.WindowState;
                window1.Top = window.Top;
                window1.Left = window.Left;
                window1.Height = window.Height;
                window1.Width = window.Width;
                window1.Show();
                window.Close();
            });
            Patients = new Command(obj =>
            {
                Patients window1 = new Patients();
                window1.WindowState = window.WindowState;
                window1.Top = window.Top;
                window1.Left = window.Left;
                window1.Height = window.Height;
                window1.Width = window.Width;
                window1.Show();
                window.Close();
            });
            Doctors = new Command(obj =>
            {
                Doctors window1 = new Doctors();
                window1.WindowState = window.WindowState;
                window1.Top = window.Top;
                window1.Left = window.Left;
                window1.Height = window.Height;
                window1.Width = window.Width;
                window1.Show();
                window.Close();
            });
            Nurces = new Command(obj =>
            {
                Nurces window1 = new Nurces();
                window1.WindowState = window.WindowState;
                window1.Top = window.Top;
                window1.Left = window.Left;
                window1.Height = window.Height;
                window1.Width = window.Width;
                window1.Show();
                window.Close();
            });
            ProcSchedule = new Command(obj =>
            {
                ProcSchedule window1 = new ProcSchedule();
                window1.WindowState = window.WindowState;
                window1.Top = window.Top;
                window1.Left = window.Left;
                window1.Height = window.Height;
                window1.Width = window.Width;
                window1.Show();
                window.Close();
            });
            Schedule = new Command(obj =>
            {
                Schedule window1 = new Schedule();
                window1.WindowState = window.WindowState;
                window1.Top = window.Top;
                window1.Left = window.Left;
                window1.Height = window.Height;
                window1.Width = window.Width;
                window1.Show();
                window.Close();
            });
            Exit = new Command(obj =>
            {
                MainWindow window1 = new MainWindow();
                window1.Show();
                window.Close();
            });
        }
        public Command RegistrationProc { get; set; }
        public Command Registration { get; set; }
        public Command Test { get; set; }
        public Command Exit { get; set; }
        public Command Card { get; set; }
        public Command Patients { get; set; }
        public Command Doctors { get; set; }
        public Command Nurces { get; set; }
        public Command ProcSchedule { get; set; }
        public Command Schedule { get; set; }
    }
}
