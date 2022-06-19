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
    public abstract class Keyboard
    {
        const int KeyCount = 61, LayerCount = 4;
        protected KeyboardKey[] _visualKeys; //Should be 61 item

        private KeyboardLayout _layout;
        public KeyboardLayout Layout
        {
            get => _layout;
            set
            {
                _layout = value;

                for (int i = 0; i < KeyCount; i++)
                {
                    _visualKeys[i].MainKey = _layout.Keys[i, 0];
                    _visualKeys[i].ShiftKey = _layout.Keys[i, 1];
                    _visualKeys[i].AltGrKey = _layout.Keys[i, 2];
                    _visualKeys[i].ShiftAltGrKey = _layout.Keys[i, 3];
                }
            }
        }


        public abstract Grid TargetGrid { get; set; }

        protected List<int> _previousKeys = new List<int>();

        public void HighlightKey(string key, bool isMistake = false)
        {
            var position = KeyIndexWithChar(key);
            HighlightKeyAt(position, isMistake);
        }

        public void HighlightKeyAt((int, int) position, bool isMistake = false)
        {
            foreach (var index in _previousKeys)
            {
                _visualKeys[index].EndHighlightAnimation();
            }

            if (position.Item1 == -1)
            {
                return;
            }

            var isLeftHandActive =
                position.Item1.IsInRange(0, 5) ||
                position.Item1.IsInRange(15, 19) ||
                position.Item1.IsInRange(29, 33) ||
                position.Item1.IsInRange(42, 46);

            _previousKeys = position switch
            {
                var (k, l) when l == 0 => new List<int>() { k },
                var (k, l) when l == 1 && !isLeftHandActive => new List<int>() { k, 41 },      //Left shift
                var (k, l) when l == 1 && isLeftHandActive => new List<int>() { k, 52 },       //Right shift
                var (k, l) when l == 2 => new List<int>() { k, 57 },                           //AltGr
                var (k, l) when l == 3 && !isLeftHandActive => new List<int>() { k, 41, 57 },  //Left shift + AltGr
                var (k, l) when l == 3 && isLeftHandActive => new List<int>() { k, 52, 57 },   //Right shift + AltGr
                _ => new List<int>()
            };

            foreach (var index in _previousKeys)
            {
                _visualKeys[index].BeginHighlightAnimation();
            }
        }

        protected (int, int) KeyIndexWithChar(string key)
        {
            for (int i = 0; i < KeyCount; i++)
            {
                for (int j = 0; j < LayerCount; j++)
                {
                    if (Layout.Keys[i, j] == key)
                    {
                        return (i, j);
                    }
                }
            }

            return (-1, -1);
        }
    
        protected void ClearChildren(Grid targetGrid)
        {
            if (targetGrid == null)
            {
                return;
            }

            foreach (var child in targetGrid.Children)
            {
                ClearChildren(child as Grid);
            }

            targetGrid.Children.Clear();
        }

        public Keyboard()
        {
            _visualKeys = new KeyboardKey[KeyCount];
            for (int i = 0; i < KeyCount; i++)
            {
                _visualKeys[i] = new KeyboardKey("", "", "", "");
            }
        }
    }
}
