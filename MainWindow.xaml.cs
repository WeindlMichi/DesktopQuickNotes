using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
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
    public class NoteData
    {
        public string Text { get; set;  }
        public double Width { get; set; } = 350;
        public double Height { get; set; } = 300;
        public double X { get; set; } = 100;
        public double Y { get; set; } = 100;
        public double FontSize { get; set; } = 14;
    }
    public partial class MainWindow : Window
    {
        string saveFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DesktopQuickNotes.json");

        public MainWindow()
        {
            InitializeComponent();

            LoadNotes();

            NotesBox.TextChanged += NotesBox_TextChanged;
            this.SizeChanged += MainWindow_SizeChanged;
            this.LocationChanged += MainWindow_LocationChanged;
        }

        private void NotesBox_TextChanged( object sender, TextChangedEventArgs e)
        {
            SaveNotes();
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Window size changed! \n new width: {this.ActualWidth} \n new height: {this.ActualHeight}");
            SaveNotes();
        }

        private void MainWindow_LocationChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Window pos changed! \n new x: {this.Left} \n new y: {this.Top}");
            SaveNotes();
        }
        
        private void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontSizeComboBox.SelectedItem is ComboBoxItem item)
            {
                double newSize = Convert.ToDouble(item.Content);
                NotesBox.FontSize = newSize;
                SaveNotes();
            }
        }

        private void SaveNotes()
        {
            var data = new NoteData
            {
                Text = NotesBox.Text,
                Width = this.ActualWidth,
                Height = this.ActualHeight,
                X = this.Left,
                Y = this.Top,
                FontSize = NotesBox.FontSize
            };

            string jsonString = JsonSerializer.Serialize(data);

            File.WriteAllText(saveFile, jsonString);
        }

        private void LoadNotes()
        {
            if (File.Exists(saveFile))
            {
                try
                {
                    string jsonString = File.ReadAllText(saveFile);
                    var data = JsonSerializer.Deserialize<NoteData>(jsonString);

                    if(data != null)
                    {
                        NotesBox.Text = data.Text;
                        this.Width = data.Width;
                        this.Height = data.Height;
                        this.Left = data.X;
                        this.Top = data.Y;
                        NotesBox.FontSize = data.FontSize;
                    }
                }
                catch
                {
                    // use default value
                }
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

        private void NoteBox_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            FontSizeComboBox.Text = NotesBox.FontSize.ToString();

            SettingsPopUp.IsOpen = true;
        }
    }
}
