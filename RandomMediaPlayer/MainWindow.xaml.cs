using Microsoft.Win32;
using RandomMediaPlayer.Core.Displayers;
using RandomMediaPlayer.PhotoShower;
using System.Windows;

namespace RandomMediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IDisplayer displayer;
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
                var dir = folderPicker.FileName.Substring(0, folderPicker.FileName.LastIndexOf('\\'));
                displayer = new PhotoDisplayer(DisplayArea, new System.Uri(dir));
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
    }
}
