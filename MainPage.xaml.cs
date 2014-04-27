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
using Microsoft.Phone.Scheduler;
using System.IO.IsolatedStorage;
using System.Threading;

namespace Circular
{
    public partial class MainPage : PhoneApplicationPage
    {
        IsolatedStorageSettings configs;
        Timer timer;

        List<string> saidasDiretas;
        List<string> saidasInversas;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            configs = IsolatedStorageSettings.ApplicationSettings;

            saidasDiretas = new List<string>();
            foreach (Carro carro in App.Diretos)
                saidasDiretas.AddRange(carro.Saidas.Saida);
            saidasDiretas.Sort();

            saidasInversas = new List<string>();
            foreach (Carro carro in App.Inversos)
                saidasInversas.AddRange(carro.Saidas.Saida);
            saidasInversas.Sort();

            LoadAll();
        }

        private void LoadAll()
        {
            LoadSaidas(saidasDiretas, spDiretos);

            LoadSaidas(saidasInversas, spInversos);
        }

        private void LoadSaidas(IEnumerable<string> lista, StackPanel panel)
        {
            foreach (var item in lista)
            {
                TextBlock tbHorario = new TextBlock();
                tbHorario.Style = Resources["ListaHorario"] as Style;

                tbHorario.Text = item;
                tbHorario.Hold += tbHorario_Hold;

                panel.Children.Add(tbHorario);
            }
        }

        private void ontimer(object state)
        {
            try
            {
                Dispatcher.BeginInvoke(delegate()
                {
                    LoadProximos();
                });
            }
            catch (Exception e)
            {
                Dispatcher.BeginInvoke(delegate() { MessageBox.Show(e.Message); });
            }

        }

        // Load data for the ViewModel Items
        private void LoadProximos()
        {
            
            HorarioProximo(saidasDiretas, tbHorarioProxDireto);
            QuantoFalta(tbHorarioProxDireto, tbFaltamDireto);

            HorarioProximo(saidasInversas, tbHorarioProxInverso);
            QuantoFalta(tbHorarioProxInverso, tbFaltamInverso);
        }

        private void QuantoFalta(TextBlock tbHorario, TextBlock tbFaltam)
        {
            TimeSpan ts = DateTime.Parse(tbHorario.Text).TimeOfDay.Subtract(DateTime.Now.TimeOfDay);
            if (ts.TotalMinutes <= 60)
                tbFaltam.Text = String.Format("Faltam {0} minutos", ts.TotalMinutes.ToString("F0", System.Globalization.CultureInfo.InvariantCulture));
            else
                tbFaltam.Text = "Vai demorar um pouco.";
        }



        private void HorarioProximo(List<string> saidasDiretas, TextBlock tb)
        {
            string proxSaida;
            try
            {
                proxSaida = saidasDiretas.Where(x => String.Compare(x, DateTime.Now.ToShortTimeString()) > 0).First();
            }
            catch (Exception)
            {
                proxSaida = saidasDiretas.First();
            }

            tb.Text = proxSaida;
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            this.timer = new Timer(ontimer, null, 0, 60000);
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            this.timer.Dispose();
            base.OnNavigatedFrom(e);
        }

        void tbHorario_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TextBlock horario = sender as TextBlock;

            ContextMenu ctHorario = ContextMenuService.GetContextMenu(horario.Parent);

            if (ctHorario == null)
            {
                ctHorario = new ContextMenu();
                ctHorario.IsZoomEnabled = false;
                ctHorario.IsFadeEnabled = false;
            }
            else
                ctHorario.Items.Clear();

            var menuItem = new MenuItem();
            menuItem.Header = "Criar alerta";
            menuItem.Click += (o, arg) => menuItem_Click(o, arg, horario.Text);

            ctHorario.Items.Add(menuItem);


            ContextMenuService.SetContextMenu(horario.Parent, ctHorario);
            ctHorario.IsOpen = true;
        }

        void menuItem_Click(object sender, RoutedEventArgs e, String horario)
        {

            Settings.AlarmDelay delay;
            if (configs.Contains("AlarmDelay"))
                delay = (Settings.AlarmDelay)configs["AlarmDelay"];
            else
                delay = Settings.AlarmDelay.Quinze;

            int atraso;
            switch (delay)
            {
                case Settings.AlarmDelay.Cinco:
                    atraso = 5;
                    break;
                case Settings.AlarmDelay.Dez:
                    atraso = 10;
                    break;
                case Settings.AlarmDelay.Quinze:
                    atraso = 15;
                    break;
                default:
                    atraso = 0;
                    break;
            }

            DateTime expiration = DateTime.Parse(horario);
            if (expiration.TimeOfDay.Subtract(DateTime.Now.TimeOfDay).TotalMinutes < atraso)
            {
                MessageBox.Show("Ops! Muito em cima da hora para criar o alarme! Sugiro ir para a parada agora.");
                return;
            }

            Reminder reminder = new Reminder(Guid.NewGuid().ToString())
            {
                Title = "Saindo do terminal",
                Content = String.Format("Um circular está saindo do terminal às {0}", expiration.ToLocalTime()),
                RecurrenceType = RecurrenceInterval.None,
                BeginTime = expiration.AddMinutes(-atraso),
                ExpirationTime = expiration
            };

            // Register the reminder with the system.
            ScheduledActionService.Add(reminder);
            string message = String.Format("Um alerta foi criado para a saída: {0}. Você será avisado com {1} minutos de antecedência.", expiration.ToLongTimeString(), atraso);
            MessageBox.Show(message);
        }

        private void btnAjustes_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Sobre.xaml", UriKind.Relative));
        }
    }
}