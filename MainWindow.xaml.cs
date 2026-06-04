using System;
using System.Collections.Generic;
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
        string saveFile =
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData),
                "DesktopQuickNotes.txt");

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
    }
}
