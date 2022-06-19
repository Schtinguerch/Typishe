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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Typishe.Resources.Controls
{
    public enum MessageType
    {
        Information,
        Warning,
        Error,
        Success,
        Processing,
    }

    public partial class TemporaryPopupMessage : UserControl
    {
        private Dictionary<MessageType, string> _signDictionary = new Dictionary<MessageType, string>()
        {
            { MessageType.Information, "\uE946" },
            { MessageType.Warning, "\uE7BA" },
            { MessageType.Error, "\uE711" },
            { MessageType.Success, "\uE73E" },
            { MessageType.Processing, "\uE9F5" },
        };

        public TemporaryPopupMessage()
        {
            InitializeComponent();
        }

        public void ShowMessage(string messageText, MessageType messageType = MessageType.Information)
        {
            MessageTextBlock.Text = messageText;
            SignTextBlock.Text = _signDictionary[messageType];

            var storyboard = FindResource("ShowMessageStoryboard") as Storyboard;
            storyboard.Begin();
        }

        public void ShowMessage(string messageText, char signCharacter)
        {
            MessageTextBlock.Text = messageText;
            SignTextBlock.Text = signCharacter.ToString();

            var storyboard = FindResource("ShowMessageStoryboard") as Storyboard;
            storyboard.Begin();
        }
    }
}
