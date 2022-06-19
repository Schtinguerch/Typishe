using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Typishe.Input.Keyboards.XamlPatterns
{
    /// <summary>
    /// Логика взаимодействия для StandardKeyboardPattern.xaml
    /// </summary>
    public partial class StandardKeyboardPattern : UserControl, IKeyboardGridPattern
    {
        public StandardKeyboardPattern()
        {
            InitializeComponent();
        }

        public Grid KeyboardGridPattern => KeyboardGrid;
    }
}
