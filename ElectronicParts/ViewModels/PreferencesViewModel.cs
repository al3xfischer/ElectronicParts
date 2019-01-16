using ElectronicParts.Commands;
using ElectronicParts.Services;
using ElectronicParts.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace ElectronicParts.ViewModels
{
    public class PreferencesViewModel : BaseViewModel
    {
        public IConfigurationService ConfigurationService { get; }

        public ICommand ApplyCommand { get; }

        public Color SelectedStringColor { get; set; }

        public string SelectedStringColorString { get; set; }

        public string StringValue { get; set; }

        public Color SelectedIntColor { get; set; }

        public string SelectedIntColorString { get; set; }

        public string IntValue { get; set; }

        public Color SelectedBoolColor { get; set; }

        public string SelectedBoolColorString { get; set; }

        public string BoolValue { get; set; }

        public PreferencesViewModel(IConfigurationService configurationService)
        {
            this.ConfigurationService = configurationService;

            this.SelectedStringColorString = configurationService.Configuration.StringColor;
            this.SelectedIntColorString = configurationService.Configuration.IntColor;
            this.SelectedBoolColorString = configurationService.Configuration.BoolColor;

            this.StringValue = configurationService.Configuration.StringValue;
            this.IntValue = configurationService.Configuration.IntValue.ToString();
            this.BoolValue = configurationService.Configuration.BoolValue.ToString();

            this.SelectedStringColor = (Color)ColorConverter.ConvertFromString(configurationService.Configuration.StringColor);
                       
            this.SelectedIntColor = (Color)ColorConverter.ConvertFromString(configurationService.Configuration.IntColor);

            this.SelectedBoolColor = (Color)ColorConverter.ConvertFromString(configurationService.Configuration.BoolColor);

            this.ApplyCommand = new RelayCommand(obj => 
            {
                this.ConfigurationService.Configuration.StringColor = this.SelectedStringColorString;
                this.ConfigurationService.Configuration.IntColor = this.SelectedIntColorString;
                this.ConfigurationService.Configuration.BoolColor = this.SelectedBoolColorString;

                this.ConfigurationService.Configuration.StringValue = this.StringValue;
                int.TryParse(this.IntValue, out int newIntValue);
                this.ConfigurationService.Configuration.IntValue = newIntValue;
                this.ConfigurationService.Configuration.BoolValue = "true" == this.BoolValue;

                this.ConfigurationService.SaveConfiguration();
            });
        }
    }
}
