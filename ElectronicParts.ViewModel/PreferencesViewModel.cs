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
    using System.ComponentModel.DataAnnotations;
    using System.Windows.Media;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using ElectronicParts.Models;
    using ElectronicParts.Services.Interfaces;
    using ElectronicParts.ViewModels.Commands;
    using System.ComponentModel;
    using System.Collections;
    using System.Collections.Generic;
    using System;

    /// <summary>
    /// The view model used for the preferences window.
    /// </summary>
    public class PreferencesViewModel : BaseViewModel
    {
        private string integerRuleValueText;
        private ListSortDirection sortDirection;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreferencesViewModel"/> class.
        /// </summary>
        /// <param name="configurationService">The configuration service of the ElectronicParts program.</param>
        public PreferencesViewModel(IConfigurationService configurationService)
        {
            this.ConfigurationService = configurationService;
            this.IntegerRuleValueText = "0";

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
                if (!this.ConfigurationService.Configuration.StringRules.Any(rule => rule.Value == this.TempStringRule.Rule.Value))
                {
                    Rule<string> newRule = new Rule<string>(this.TempStringRule.Rule.Value, this.TempStringRule.Color.ToString(), (value) =>
                    {
                        return !this.ConfigurationService.Configuration.StringRules.Any(rule => rule.Value == value);
                    });
                    this.StringRules.Add(new RuleViewModel<string>(newRule, stringDeletionCommand));
                    configurationService.Configuration.StringRules.Add(newRule);

                    this.TempStringRule.Value = string.Empty;
                    this.TempStringRule.Color = (Color)ColorConverter.ConvertFromString("Black");
                }
            }, arg =>
            {
                return !this.ConfigurationService.Configuration.StringRules.Any(rule => rule.Value == this.TempStringRule.Rule.Value);
            });

            this.AddIntRuleCommand = new RelayCommand(obj =>
            {
                var ruleValue = int.Parse(this.IntegerRuleValueText);
                if (!this.ConfigurationService.Configuration.IntRules.Any(rule => rule.Value == ruleValue))
                {
                    Rule<int> newRule = new Rule<int>(ruleValue, this.TempIntRule.Color.ToString(), (value) =>
                    {
                        return !this.ConfigurationService.Configuration.IntRules.Any(rule => rule.Value == value);
                    });
                    this.IntRules.Add(new RuleViewModel<int>(newRule, intDeletionCommand));
                    configurationService.Configuration.IntRules.Add(newRule);

                    this.IntegerRuleValueText = "0";
                    this.TempIntRule.Value = 0;
                    this.TempIntRule.Color = (Color)ColorConverter.ConvertFromString("Black");
                }
            }, arg =>
            {
                if (!int.TryParse(this.IntegerRuleValueText, out int result))
                {
                    return false;
                }
                if (this.ConfigurationService.Configuration.IntRules.Any(rule => rule.Value == result))
                {
                    return false;
                }

                return true;
            });

            Rule<string> tempStringRule = new Rule<string>(string.Empty, "Black", (value) =>
            {
                return true;
            });

            this.TempStringRule = new RuleViewModel<string>(tempStringRule, stringDeletionCommand);

            Rule<int> tempIntRule = new Rule<int>(0, "Black", (value) =>
            {
                return true;
            });

            this.TempIntRule = new RuleViewModel<int>(tempIntRule, intDeletionCommand);
        }

        /// <summary>
        /// Sorts the rule lists.
        /// </summary>
        public void SortByValue()
        {
            this.SortRuleCollection(this.StringRules, this.sortDirection);
            this.SortRuleCollection(this.IntRules, this.sortDirection);

            if (this.sortDirection == ListSortDirection.Ascending)
            {
                this.sortDirection = ListSortDirection.Descending;
            }
            else
            {
                this.sortDirection = ListSortDirection.Ascending;
            }
        }

        private void SortRuleCollection<T>(ObservableCollection<RuleViewModel<T>> values, ListSortDirection sortDirection)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var sortedList = new List<RuleViewModel<T>>();
            if (sortDirection == ListSortDirection.Ascending)
            {
                sortedList = values.OrderBy(rule => rule.Value).ToList();
            }
            else
            {
                sortedList = values.OrderByDescending(rule => rule.Value).ToList();
            }

            for (int i = 0; i < sortedList.Count; i++)
            {
                values.Move(values.IndexOf(sortedList[i]), i);
            }
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

        public string IntegerRuleValueText
        {
            get => integerRuleValueText;
            set
            {
                int.Parse(value);
                this.integerRuleValueText = value;
                FirePropertyChanged(nameof(IntegerRuleValueText));
            }
        }

        /// <summary>
        /// Gets all string rule view models.
        /// </summary>
        /// <value>All string rule view models.</value>
        public ObservableCollection<RuleViewModel<string>> StringRules { get; }

        public RuleViewModel<string> TempStringRule { get; }

        /// <summary>
        /// Gets all integer rule view models.
        /// </summary>
        /// <value>All integer rule view models.</value>
        public ObservableCollection<RuleViewModel<int>> IntRules { get; }

        public RuleViewModel<int> TempIntRule { get; }

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
                if (value < 200)
                {
                    throw new ValidationException();
                }

                this.ConfigurationService.Configuration.BoardHeight = value;
            }
        }

        public int BoardWidth
        {
            get => this.ConfigurationService.Configuration.BoardWidth;

            set
            {
                if(value < 200)
                {
                    throw new ValidationException();
                }

                this.ConfigurationService.Configuration.BoardWidth = value;
            }
        }
    }
}
