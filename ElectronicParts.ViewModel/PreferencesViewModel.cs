// ***********************************************************************
// Assembly         : ElectronicParts.ViewModels
// Author           : Peter Helf
// ***********************************************************************
// <copyright file="PreferencesViewModel.cs" company="FHWN">
//     Copyright ©  2019
// </copyright>
// <summary>Represents the PreferencesViewModel class of the ElectronicParts programm</summary>
// ***********************************************************************

namespace ElectronicParts.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;
    using ElectronicParts.Models;
    using ElectronicParts.Services.Interfaces;
    using ElectronicParts.ViewModels.Commands;

    /// <summary>
    /// The view model used for the preferences window.
    /// </summary>
    public class PreferencesViewModel : BaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PreferencesViewModel"/> class.
        /// </summary>
        /// <param name="configurationService">The configuration service of the ElectronicParts program.</param>
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
                Rule<string> newRule = new Rule<string>(string.Empty, "Black");
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

        /// <summary>
        /// Gets the configuration service of the ElectronicParts program.
        /// </summary>
        /// <value>The configuration service of the ElectronicParts program.</value>
        public IConfigurationService ConfigurationService { get; }

        /// <summary>
        /// Gets the command which is  used to save changes made to the preferences in a file.
        /// </summary>
        /// <value>The command which is  used to save changes made to the preferences in a file.</value>
        public ICommand ApplyCommand { get; }

        /// <summary>
        /// Gets the command which is used to add a string rule.
        /// </summary>
        /// <value>The command which is used to add a string rule.</value>
        public ICommand AddStringRuleCommand { get; }

        /// <summary>
        /// Gets the command which is used to add a integer rule.
        /// </summary>
        /// <value>The command which is used to add a integer rule.</value>
        public ICommand AddIntRuleCommand { get; }

        /// <summary>
        /// Gets the command which is used to add a boolean rule.
        /// </summary>
        /// <value>The command which is used to add a boolean rule.</value>
        public ICommand AddBoolRuleCommand { get; }

        /// <summary>
        /// Gets all string rule view models.
        /// </summary>
        /// <value>All string rule view models.</value>
        public ObservableCollection<RuleViewModel<string>> StringRules { get; }

        /// <summary>
        /// Gets all integer rule view models.
        /// </summary>
        /// <value>All integer rule view models.</value>
        public ObservableCollection<RuleViewModel<int>> IntRules { get; }

        /// <summary>
        /// Gets all boolean rule view models.
        /// </summary>
        /// <value>All boolean rule view models.</value>
        public ObservableCollection<RuleViewModel<bool>> BoolRules { get; }

        public int BoardHeight
        {
            get => this.ConfigurationService.Configuration.BoardHeight;

            set
            {
                this.ConfigurationService.Configuration.BoardHeight = value;
            }
        }

        public int BoardWidth
        {
            get => this.ConfigurationService.Configuration.BoardWidth;

            set
            {
                this.ConfigurationService.Configuration.BoardWidth = value;
            }
        }
    }
}
