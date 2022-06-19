using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Typishe.Input.ExerciseTextViews
{
    /// <summary>
    /// Логика взаимодействия для StaticCaretTextView.xaml
    /// </summary>
    public partial class StaticCaretTextView : Page, IExerciseTextView
    {
        public StaticCaretTextView()
        {
            InitializeComponent();
        }

        public void UpdateTextOutput(string enteredText, string leftText)
        {
            EnteredTextBlock.Text = enteredText;
            LeftTextBlock.Text = leftText;
        }

        public void UpdateTextOutput(string mistakenText)
        {
            MistakenTextBlock.Text = mistakenText.Replace(' ', '‧');
        }
    }
}
