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

namespace Typishe.Input.ExerciseTextViews
{
    /// <summary>
    /// Логика взаимодействия для SingleLineTextView.xaml
    /// </summary>
    public partial class SingleLineTextView : Page, IExerciseTextView
    {
        private Style
            _basicTextBlockStyle,
            _highlightedTextBlockStyle,
            _raidedTextBlockStyle,

            _basicRunStyle,
            _currentRunStyle;

        private List<TextBlock> _wordTextBlocks;
        private List<Run> _charRuns;

        private int
            _enteredTextLength = 0,
            _maxTextLength = 0,
            _enderedWordsCount = 0;

        public SingleLineTextView()
        {
            InitializeComponent();

            _basicTextBlockStyle = FindResource("BasicTextBlockStyle") as Style;
            _highlightedTextBlockStyle = FindResource("HighlightedTextBlockStyle") as Style;
            _raidedTextBlockStyle = FindResource("RaidedTextBlockStyle") as Style;

            _basicRunStyle = FindResource("BasicRunStyle") as Style;
            _currentRunStyle = FindResource("CurrentRunStyle") as Style;
        }

        public void UpdateTextOutput(string enteredText, string leftText)
        {
            if (enteredText == string.Empty)
            {
                LoadTextBlocks(leftText);
                TextScrollViewer.TargetHorizontalOffset = 0;
            }

            HighlightNextCharacter();
        }

        public void UpdateTextOutput(string mistakenText)
        {
            MistakenTextBlock.Text = mistakenText == "" ? "" : "Mistake: " + mistakenText.Replace(' ', '‧');
        }

        private void LoadTextBlocks(string text)
        {
            _enteredTextLength = -1;
            _maxTextLength = text.Length;
            _enderedWordsCount = 0;

            TextOutputStackPanel.Children.Clear();
            var words = text.Split(' ');

            TextOutputStackPanel.Children.Add(new Rectangle()
            {
                Width = Setup.CenterNode.AppWindow.ActualWidth / 2d,
            });

            _wordTextBlocks = new List<TextBlock>(words.Length);
            _charRuns = new List<Run>();

            foreach (var word in words)
            {
                var newTextBlock = new TextBlock();
                newTextBlock.Style = _basicTextBlockStyle;

                foreach (var character in word)
                {
                    var newRun = new Run()
                    {
                        Style = _basicRunStyle,
                        Text = character.ToString()
                    };

                    newTextBlock.Inlines.Add(newRun);
                    _charRuns.Add(newRun);
                }

                var newSpaceRun = new Run()
                {
                    Style = _basicRunStyle,
                    Text = " ",
                };

                newTextBlock.Inlines.Add(newSpaceRun);
                _charRuns.Add(newSpaceRun);

                _wordTextBlocks.Add(newTextBlock);
                TextOutputStackPanel.Children.Add(newTextBlock);
            }

            _wordTextBlocks[0].Style = _highlightedTextBlockStyle;
            TextOutputStackPanel.Children.Add(new Rectangle()
            {
                Width = Setup.CenterNode.AppWindow.ActualWidth / 2d,
            });
        }

        private void HighlightNextCharacter()
        {
            _enteredTextLength += 1;

            if (_enteredTextLength == _maxTextLength)
            {
                return;
            }

            _charRuns[_enteredTextLength].Style = _currentRunStyle;

            if (_charRuns[_enteredTextLength].Text == " ")
            {
                _enderedWordsCount += 1;

                var wordOffset = RelativePoint(_wordTextBlocks[_enderedWordsCount], TextScrollViewer).X;
                var startOffset = RelativePoint(TextOutputStackPanel.Children[0], TextScrollViewer).X;

                var absoluteHorizontalOffset = wordOffset - startOffset;

                var windowWidth = Setup.CenterNode.AppWindow.ActualWidth;
                var borderValue = 300d;

                if (windowWidth - wordOffset < borderValue)
                {
                    TextScrollViewer.TargetHorizontalOffset = absoluteHorizontalOffset - borderValue;
                }

                _wordTextBlocks[_enderedWordsCount].Style = _highlightedTextBlockStyle;
                _wordTextBlocks[_enderedWordsCount - 1].Style = _raidedTextBlockStyle;
            } 

            if (_enteredTextLength == 0)
            {
                return;
            }

            _charRuns[_enteredTextLength - 1].Style = _basicRunStyle;
        }

        private Point RelativePoint(UIElement element, UIElement relativeTo)
        {
            return element.TransformToAncestor(relativeTo).Transform(new Point(0, 0));
        }
    }
}
