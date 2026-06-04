using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace DesktopQuickNotes
{
    public partial class MainWindow : Window
    {
        string saveFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DesktopQuickNotes.txt");

        public MainWindow()
        {
            InitializeComponent();

            LoadNotes();

            NotesBox.TextChanged += NotesBox_TextChanged;
        }

        private void NotesBox_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            SaveNotes();
        }

        private void SaveNotes()
        {
            File.WriteAllText(
                saveFile,
                NotesBox.Text);
        }

        private void LoadNotes()
        {
            if (File.Exists(saveFile))
            {
                NotesBox.Text =
                    File.ReadAllText(saveFile);
            }
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NotesBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            Point mousePosition = e.GetPosition(NotesBox);

            int caretIndex = NotesBox.GetCharacterIndexFromPoint(mousePosition, true);

            System.Diagnostics.Debug.WriteLine($"caretIndex: {caretIndex}");

            if (caretIndex >= 0)
            {
                NotesBox.CaretIndex = caretIndex;

                int lineIndex = NotesBox.GetLineIndexFromCharacterIndex(caretIndex);

                if (lineIndex >= 0)
                {
                    int lineStart = NotesBox.GetCharacterIndexFromLineIndex(lineIndex);
                    int lineLength = NotesBox.GetLineLength(lineIndex);

                    NotesBox.Select(lineStart, lineLength);
                }
            }
        }
    }
}
