using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using Typishe.Input.Keyboards.XamlPatterns;
using Typishe.Service;

namespace Typishe.Input
{
    public class OrtholinearKeyboard : Keyboard
    {
        private Grid _targetGrid;
        public override Grid TargetGrid
        {
            get => _targetGrid;
            set
            {
                ClearChildren(_targetGrid);
                _targetGrid = value;

                var grid = new OrtholinearKeyboardPattern().KeyboardGrid.Copy();
                int column = 0, row = 0;

                for (int i = 0; i < 61; i++)
                {
                    if (i.IsEqualOr(14, 28, 41, 53))
                    {
                        row += 1;
                        column = 0;
                    }

                    Grid.SetColumn(_visualKeys[i], column);
                    Grid.SetRow(_visualKeys[i], row);

                    (grid.Children[row] as Grid).Children.Add(_visualKeys[i]);

                    column += 1;
                }

                _targetGrid.Children.Add(grid);
            }
        }

        public OrtholinearKeyboard(Grid targetGrid, KeyboardLayout layout) : base()
        {
            Layout = layout;
            TargetGrid = targetGrid;
        }
    }
}
