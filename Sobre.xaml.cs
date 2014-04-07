using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;

namespace Circular
{
    public partial class Sobre : PhoneApplicationPage
    {
        public Sobre()
        {
            InitializeComponent();

            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains("AlertDelay"))
            {
                Settings.AlarmDelay delay = (Settings.AlarmDelay)settings["AlertDelay"];
                switch (delay)
                {
                    case Settings.AlarmDelay.Cinco:
                        lpDelay.SelectedItem = opt5min;
                        break;
                    case Settings.AlarmDelay.Dez:
                        lpDelay.SelectedItem = opt10min;
                        break;
                    case Settings.AlarmDelay.Quinze:
                        lpDelay.SelectedItem = opt15min;
                        break;
                    default:
                        break;
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SaveDelaySettings();

            base.OnNavigatedFrom(e);
        }

        private void SaveDelaySettings()
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            switch (lpDelay.SelectedIndex)
            {
                case 0:
                    if (settings.Contains("AlertDelay"))
                        settings["AlertDelay"] = Settings.AlarmDelay.Quinze;
                    else
                        settings.Add("AlertDelay", Settings.AlarmDelay.Quinze);
                    break;
                case 1:
                    if (settings.Contains("AlertDelay"))
                        settings["AlertDelay"] = Settings.AlarmDelay.Dez;
                    else
                        settings.Add("AlertDelay", Settings.AlarmDelay.Dez);
                    break;
                default:
                    if (settings.Contains("AlertDelay"))
                        settings["AlertDelay"] = Settings.AlarmDelay.Cinco;
                    else
                        settings.Add("AlertDelay", Settings.AlarmDelay.Cinco);
                    break;
            }
            settings.Save();
        }
    }
}