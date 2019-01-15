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

        public IEnumerable<MyColors> AvailableColors
        {
            get
            {
                return Enum.GetValues(typeof(MyColors))
                    .Cast<MyColors>();
            }
        }

        public PreferencesViewModel(IConfigurationService configurationService)
        {

        }
    }
}
