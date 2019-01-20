using ElectronicParts.ViewModels.Commands;
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
using System.Windows;

namespace ElectronicParts.ViewModels
{
    public class PreferencesViewModel : BaseViewModel
    {
        public IConfigurationService ConfigurationService { get; }

        public ICommand ApplyCommand { get; }
        public ICommand AddStringRuleCommand { get; }
        public ICommand AddIntRuleCommand { get; }
        public ICommand AddBoolRuleCommand { get; }

        public ObservableCollection<RuleViewModel<string>> StringRules { get; }

        public ObservableCollection<RuleViewModel<int>> IntRules { get; }

        public ObservableCollection<RuleViewModel<bool>> BoolRules { get; }
        
        public PreferencesViewModel(IConfigurationService configurationService)
        {
            this.ConfigurationService = configurationService;

            ICommand stringDeletionCommand = new RelayCommand(ruleObj =>
            {
                RuleViewModel<string> ruleVM = ruleObj as RuleViewModel<string>;

                this.StringRules.Remove(ruleVM);
                configurationService.Configuration.StringRules.Remove(ruleVM.Rule);
            });

            ICommand intDeletionCommand = new RelayCommand(ruleObj =>
            {
                RuleViewModel<int> ruleVM = ruleObj as RuleViewModel<int>;

                this.IntRules.Remove(ruleVM);
                configurationService.Configuration.IntRules.Remove(ruleVM.Rule);
            });

            ICommand boolDeletionCommand = new RelayCommand(ruleObj =>
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
                this.StringRules.Add(new RuleViewModel<string>(stringRule, stringDeletionCommand));                
            }

            foreach (var intRule in configurationService.Configuration.IntRules)
            {
                this.IntRules.Add(new RuleViewModel<int>(intRule, intDeletionCommand));
            }

            foreach (var boolRule in configurationService.Configuration.BoolRules)
            {
                this.BoolRules.Add(new RuleViewModel<bool>(boolRule, boolDeletionCommand));
            }

            this.ApplyCommand = new RelayCommand(obj => 
            {
                this.ConfigurationService.SaveConfiguration();
                MessageBox.Show("Changes were saved successfully.", "Saved", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
            });

            this.AddStringRuleCommand = new RelayCommand(obj =>
            {
                Rule<string> newRule = new Rule<string>("", "Black");
                this.StringRules.Add(new RuleViewModel<string>(newRule, stringDeletionCommand));
                configurationService.Configuration.StringRules.Add(newRule);
            });

            this.AddIntRuleCommand = new RelayCommand(obj =>
            {
                Rule<int> newRule = new Rule<int>(0, "Black");
                this.IntRules.Add(new RuleViewModel<int>(newRule, intDeletionCommand));
                configurationService.Configuration.IntRules.Add(newRule);
            });

            this.AddBoolRuleCommand = new RelayCommand(obj =>
            {
                Rule<bool> newRule = new Rule<bool>(false, "Black");
                this.BoolRules.Add(new RuleViewModel<bool>(newRule, boolDeletionCommand));
                configurationService.Configuration.BoolRules.Add(newRule);
            });
        }
    }
}
