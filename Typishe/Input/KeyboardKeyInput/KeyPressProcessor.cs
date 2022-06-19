using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Typishe.Pages;
using Typishe.Setup;

namespace Typishe.Input
{
    public class KeyPressProcessor
    {
        private Page _currentPage;
        private TypingPage _currentTypingPage;
        private List<Key> _keyDownList = new List<Key>();

        public Page CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage != null)
                {
                    _currentPage.KeyDown -= PageKeyDown;
                    _currentPage.KeyUp -= PageKeyUp;
                }

                _currentPage = value;
                _currentTypingPage = value as TypingPage;

                _currentPage.KeyDown += PageKeyDown;
                _currentPage.KeyUp += PageKeyUp;
            }
        }

        public KeyPressProcessor(Page page)
        {
            CurrentPage = page;
        }

        private void PageKeyUp(object sender, KeyEventArgs e)
        {
            _keyDownList.Remove(e.Key);
            UpdateKeyOutput();
        }

        private void PageKeyDown(object sender, KeyEventArgs e)
        {
            if (!_keyDownList.Contains(e.Key))
            {
                _keyDownList.Add(e.Key);
            }

            UpdateKeyOutput();
            HanldeKeyPress();
        }

        private void HanldeKeyPress()
        {
            foreach (var pair in ApplicationSetup.FunctionalSetup.ShortCuts)
            {
                if (_keyDownList.SequenceEqual(pair.Key))
                {
                    ApplicationCommander.ExecuteCommand(pair.Value);
                }
            }
        }

        private void UpdateKeyOutput()
        {
            if (_currentTypingPage == null)
            {
                return;
            }

            _currentTypingPage.PressedKeysTextBlock.Text = KeyboardLayoutConverter.KeysToText(_keyDownList); 
        }
    }
}
