using ElectronicParts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace ElectronicParts.ViewModels
{
    public class RuleViewModel<T>
    {
        public RuleViewModel(Rule<T> rule, ICommand deletionCommand)
        {
            this.Rule = rule;
            this.DeletionCommand = deletionCommand;
            this.Color = (Color)ColorConverter.ConvertFromString(rule.Color);
        }

        public Color Color { get; set; }

        public ICommand DeletionCommand { get; } 

        public Rule<T> Rule { get; }
    }
}
