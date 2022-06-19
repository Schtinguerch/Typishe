using System;
using System.Windows.Media;

namespace Typishe.Resources.Controls
{
    public class ColorPicker
    {
        public delegate void SelectedColorChangedHandler(Brush selectedBrush);
        public event SelectedColorChangedHandler SelectedColorChanged;

        private Brush _selectedBrush;
        public Brush SelectedBrush
        {
            get => _selectedBrush;
            set
            {
                _selectedBrush = value;
                SelectedColorChanged?.Invoke(value);
            }
        }

        public Brush StandardBrush { get; set; }
    }
}
