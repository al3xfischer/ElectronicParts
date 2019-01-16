using ElectronicParts.Commands;
using ElectronicParts.Models;
using ElectronicParts.Services;
using ElectronicParts.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<RuleViewModel<string>> StringRules { get; }

        public ObservableCollection<RuleViewModel<int>> IntRules { get; }

        public ObservableCollection<RuleViewModel<bool>> BoolRules { get; }
        
        public PreferencesViewModel(IConfigurationService configurationService)
        {
            this.ConfigurationService = configurationService;

            ICommand StringDeletionCommand = new RelayCommand(ruleObj =>
            {
                RuleViewModel<string> ruleVM = ruleObj as RuleViewModel<string>;

                this.StringRules.Remove(ruleVM);
                configurationService.Configuration.StringRules.Remove(ruleVM.Rule);
            });

            ICommand IntDeletionCommand = new RelayCommand(ruleObj =>
            {
                RuleViewModel<int> ruleVM = ruleObj as RuleViewModel<int>;

                this.IntRules.Remove(ruleVM);
                configurationService.Configuration.IntRules.Remove(ruleVM.Rule);
            });

            ICommand BoolDeletionCommand = new RelayCommand(ruleObj =>
            {
                RuleViewModel<bool> ruleVM = ruleObj as RuleViewModel<bool>;

                this.BoolRules.Remove(ruleVM);
                configurationService.Configuration.BoolRules.Remove(ruleVM.Rule);
            });
            
            this.StringRules = new ObservableCollection<RuleViewModel<string>>();

            this.IntRules = new ObservableCollection<RuleViewModel<int>>();

            this.BoolRules = new ObservableCollection<RuleViewModel<bool>>();

            foreach (var stringRule in configurationService.Configuration.StringRules)
            {
                this.StringRules.Add(new RuleViewModel<string>(stringRule, StringDeletionCommand));                
            }

            foreach (var intRule in configurationService.Configuration.IntRules)
            {
                this.IntRules.Add(new RuleViewModel<int>(intRule, StringDeletionCommand));
            }

            foreach (var boolRule in configurationService.Configuration.BoolRules)
            {
                this.BoolRules.Add(new RuleViewModel<bool>(boolRule, StringDeletionCommand));
            }

            this.ApplyCommand = new RelayCommand(obj => 
            {
                this.ConfigurationService.SaveConfiguration();
            });
        }
    }
}
