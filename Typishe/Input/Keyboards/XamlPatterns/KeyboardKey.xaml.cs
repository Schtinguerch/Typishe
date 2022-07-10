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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using Typishe.Service;

namespace Typishe.Input.Keyboards.XamlPatterns
{
    /// <summary>
    /// Логика взаимодействия для KeyboardKey.xaml
    /// </summary>
    public partial class KeyboardKey : UserControl
    {
        private Storyboard _splashRectangleStoryboard;

        private string _mainKey;
        private string _shiftKey;
        private string _altGrKey;
        private string _shiftAltGrKey;

        public string MainKey
        {
            get => _mainKey;
            set
            {
                _mainKey = value;
                CheckAndRenderKeys();
            }
        }

        public string ShiftKey
        {
            get => _shiftKey;
            set
            {
                _shiftKey = value;
                CheckAndRenderKeys();
            }
        }

        public string AltGrKey
        {
            get => _altGrKey;
            set
            {
                _altGrKey = value;
            }
        }

        public string ShiftAltGrKey
        {
            get => _shiftAltGrKey;
            set
            {
                _shiftAltGrKey = value;
            }
        }

        public KeyboardKey(string mainKey, string shiftKey, string altGrKey, string shiftAltGrKey)
        {
            InitializeComponent();
            _splashRectangleStoryboard = FindResource("SplashRectangleStoryboard") as Storyboard;

            if (mainKey == shiftKey && shiftKey == altGrKey && altGrKey == shiftAltGrKey && shiftAltGrKey == "")
            {
                return;
            }

            _mainKey = mainKey;
            _shiftKey = shiftKey;
            AltGrKey = altGrKey;
            ShiftAltGrKey = shiftAltGrKey;
            CheckAndRenderKeys();
        }

        public KeyboardKey()
        {
            InitializeComponent();
            _splashRectangleStoryboard = FindResource("SplashRectangleStoryboard") as Storyboard;

            _mainKey = "a";
            _shiftKey = "A";
            _altGrKey = _shiftAltGrKey = "";
            CheckAndRenderKeys();
        }

        private void CheckAndRenderKeys()
        {
            if (_mainKey.Length > 2 && _mainKey.ToLower().IsEqualOr("l shift", "r shift", "l ctrl", "r ctrl", "alt", "altgr", "l win", "r win", "caps", "tab", "back", "menu", "enter"))
            {
                StandardKeyGrid.Visibility = Visibility.Collapsed;
                ServiceKeyGrid.Visibility = Visibility.Visible;
                CommandTextBlock.Text = _mainKey;
            }

            else
            {
                StandardKeyGrid.Visibility = Visibility.Visible;
                ServiceKeyGrid.Visibility = Visibility.Collapsed;
            }

            if (MainKey.ToUpper() == ShiftKey)
            {
                MainCharacterTextBlock.Text = ShiftKey;
                ShiftCharacterTextBlock.Text = "";
                return;
            }

            MainCharacterTextBlock.Text = MainKey;
            ShiftCharacterTextBlock.Text = ShiftKey;
        }
    
        
        public void BeginHighlightAnimation()
        {
            KeyHighlightRectangle.Opacity = 1;
        }

        public void EndHighlightAnimation()
        {
            _splashRectangleStoryboard.Begin();
            KeyHighlightRectangle.Opacity = 0;
        }

        public void ClickAnimation()
        {

        }
    }
}
