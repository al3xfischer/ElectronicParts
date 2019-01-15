using ElectronicParts.Commands;
using ElectronicParts.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ElectronicParts.ViewModels
{
    public class PreferencesViewModel : BaseViewModel
    {
        public IConfigurationService ConfigurationService { get; }

        public ICommand ChangeStringColorConfig { get; }
        public ICommand ChangeIntColorConfig { get; }
        public ICommand ChangeBoolColorConfig { get; }

        public ICommand ChangeStringValueConfig { get; }
        public ICommand ChangeIntValueConfig { get; }
        public ICommand ChangeBoolValueConfig { get; }

        public PreferencesViewModel(IConfigurationService configurationService)
        {
            this.ConfigurationService = configurationService;

            this.ChangeStringColorConfig = new RelayCommand((colorString) =>
            {
                this.ConfigurationService.Configuration.StringColor = colorString as string;
            });

            this.ChangeIntColorConfig = new RelayCommand((colorString) =>
            {
                this.ConfigurationService.Configuration.IntColor = colorString as string;
            });

            this.ChangeBoolColorConfig = new RelayCommand((colorString) =>
            {
                this.ConfigurationService.Configuration.BoolColor = colorString as string;
            });

            this.ChangeStringValueConfig = new RelayCommand((valueString) =>
            {
                this.ConfigurationService.Configuration.StringValue = valueString as string;
            });

            this.ChangeIntValueConfig = new RelayCommand((valueString) =>
            {
                int.TryParse(valueString as string, out int newIntValue);
                this.ConfigurationService.Configuration.IntValue = newIntValue;
            });

            this.ChangeBoolValueConfig = new RelayCommand((valueString) =>
            {
                this.ConfigurationService.Configuration.BoolValue = valueString as string == "True";
            });
        }
    }
}
