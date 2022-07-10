using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Typishe.Resources.Controls.ColorPicker;

namespace Typishe.Resources.Controls
{
    /// <summary>
    /// Логика взаимодействия для EffectPicker.xaml
    /// </summary>
    public partial class EffectPicker : UserControl
    {
        public delegate void SelectedEffectChangedHandler(Effect? selectedEffect);
        public event SelectedEffectChangedHandler SelectedEffectChanged;

        private Effect? _selectedEffect = null;

        public Effect? StandardEffect { get; set; }
        public Effect? SelectedEffect
        {
            get => _selectedEffect;
            set
            {
                _selectedEffect = value;
                SelectedEffectChanged?.Invoke(value);
            }
        }

        public EffectPicker()
        {
            InitializeComponent();
        }

        public void ClearEventDelegates()
        {
            if (SelectedEffectChanged == null)
            {
                return;
            }

            foreach (var eventMethod in SelectedEffectChanged.GetInvocationList())
            {
                SelectedEffectChanged -= (SelectedEffectChangedHandler)eventMethod;
            }
        }
    }
}
