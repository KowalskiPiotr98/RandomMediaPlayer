using Microsoft.Win32;
using RandomMediaPlayer.Core.Directory;
using RandomMediaPlayer.Core.Displayers;
using RandomMediaPlayer.MoviePlayer;
using RandomMediaPlayer.PhotoShower;
using RandomMediaPlayer.PhotoShower.PhotosDirectory;
using System.Windows;

namespace RandomMediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IDisplayer displayer;
        private System.Uri directory;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectDir_Click(object sender, RoutedEventArgs e)
        {
            var folderPicker = new OpenFileDialog(); //TODO: find an alternative for folder picker; for now just make users select any file in the chosen dir
            var result = folderPicker.ShowDialog();
            if (result.HasValue && result.Value)
            {
                directory = new System.Uri(folderPicker.FileName.Substring(0, folderPicker.FileName.LastIndexOf('\\')));
                CreateDisplayer();
            }
        }

        private void NextDisplayable_Click(object sender, RoutedEventArgs e)
        {
            displayer?.Next();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            displayer?.Refresh();
        }

        private void UseExternalView_Click(object sender, RoutedEventArgs e)
        {
            CreateDisplayer();
        }
        private void DisplayableTypeSelectionChanged(object sender, RoutedEventArgs e)
        {
            CreateDisplayer();
        }

        private void CreateDisplayer()
        {
            if (directory is null)
            {
                return;
            }
            displayer?.Hide();
            if (UseExternalView.IsChecked.Value)
            {
                DirectoryPicker dirPicker = null;
                if (PhotoRadioButton.IsChecked.Value)
                {
                    dirPicker = new PhotoDirectoryPicker(directory);
                }
                else if (VideoRadioButton.IsChecked.Value)
                {
                    dirPicker = new MovieDirectoryPicker(directory);
                }
                displayer = new ExternalDisplayer(dirPicker);
            }
            else
            {
                displayer = new PhotoDisplayer(DisplayArea, directory);
            }
        }

    }
}
