using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.UserData;
using Microsoft.Phone.Tasks;

namespace Circular
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            Loaded += MainPage_Loaded;
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var ProxDiretos = App.TodosHorarios.Diretos.Where(x => DateTime.Parse(x).CompareTo(DateTime.Now) > 0).Take(5);
            LoadInfo(ProxDiretos);
        }

        private void LoadInfo(IEnumerable<string> prox)
        {
            StackPanel spProx = new StackPanel();
            foreach (var item in prox)
            {
                TextBlock tbHorario = new TextBlock();
                tbHorario.Text = item;

                tbHorario.Hold += tbHorario_Hold;

                spProx.Children.Add(tbHorario);
            }
            AbaDireto.Content = spProx;
        }

        void tbHorario_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TextBlock horario = sender as TextBlock;

            ContextMenu ctHorario = ContextMenuService.GetContextMenu(horario.Parent);

            if (ctHorario == null)
                ctHorario = new ContextMenu();
            else
                ctHorario.Items.Clear();

            var menuItem = new MenuItem();
            menuItem.Header = "Alerta";
            menuItem.Click += (o, arg) => menuItem_Click(o, arg, horario.Text);

            ctHorario.Items.Add(menuItem);


            ContextMenuService.SetContextMenu(horario.Parent, ctHorario);
            ctHorario.IsOpen = true;
        }

        void menuItem_Click(object sender, RoutedEventArgs e, String horario)
        {
            DateTime hora = DateTime.Parse(horario);

            SaveAppointmentTask saTask = new SaveAppointmentTask();
            saTask.AppointmentStatus = AppointmentStatus.Free;
            saTask.StartTime = hora;
            saTask.Reminder = Reminder.FiveMinutes;
            saTask.Subject = "Circular";
            saTask.Show();

        }
    }
}