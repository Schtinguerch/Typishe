using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Typishe.Input
{
    public static class KeyboardLayoutConverter
    {
        public static KeyboardLayout InstalledLayout { get; set; }

        public static KeyboardLayout EmulatedLayout { get; set; }

        public static string EmulatedInputtedString(Key keyboardKey, int layerIndex)
        {
            var physicalLayoutInput = KeyToText(keyboardKey).ToLower();
            for (int i = 0; i < 61; i++)
            {
                if (InstalledLayout.Keys[i, 0] == physicalLayoutInput)
                {
                    return EmulatedLayout.Keys[i, layerIndex];
                }
            }

            return "";
        }

        public static string KeyToText(Key keyboardKey)
        {
            var keyText = keyboardKey.ToString();

            if (keyText.Length > 1)
            {
                string serviceKeyText;
                var isService = _codeToCharacterDictionary.TryGetValue(keyText, out serviceKeyText);

                if (isService)
                {
                    return serviceKeyText;
                }
            }

            return keyText;
        }

        public static string KeysToText(List<Key> keys)
        {
            if (keys == null || keys.Count == 0)
            {
                return string.Empty;
            }

            var keyText = string.Empty;
            foreach (var key in keys)
            {
                keyText += "+" + KeyToText(key);
            }

            return keyText.Substring(1);
        }

        private static Dictionary<string, string> _codeToCharacterDictionary = new Dictionary<string, string>()
        {
            { "Space", " " },
            { "Oem3", "`" },
            { "D1", "1" },
            { "D2", "2" },
            { "D3", "3" },
            { "D4", "4" },
            { "D5", "5" },
            { "D6", "6" },
            { "D7", "7" },
            { "D8", "8" },
            { "D9", "9" },
            { "D0", "0" },
            { "OemMin", "-" },
            { "OemPlus", "+" },
            { "OemOpenBrackets", "[" },
            { "Oem6", "]" },
            { "Oem5", "\\" },
            { "Oem1", ";" },
            { "OemQuotes", "'" },
            { "OemComma", "," },
            { "OemPeriod", "." },
            { "OemQuestion", "/" },
            { "LeftCtrl", "L Ctrl" },
            { "RightCtrl", "R Ctrl" },
            { "LeftShift", "L Shift" },
            { "RightShift", "R Shift" },
            { "LeftAlt", "Alt" },
            { "RightAlt", "AltGr" },
            { "Apps", "Menu" },
            { "Return", "Enter" },
            { "LWin", "Win" }
        };
    }
}
