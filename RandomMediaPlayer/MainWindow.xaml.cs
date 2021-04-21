﻿using Microsoft.Win32;
using RandomMediaPlayer.Actions;
using RandomMediaPlayer.Core.Directory;
using RandomMediaPlayer.Core.Displayers;
using RandomMediaPlayer.MoviePlayer;
using RandomMediaPlayer.PhotoShower;
using RandomMediaPlayer.PhotoShower.PhotosDirectory;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RandomMediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IDisplayer displayer;
        private System.Uri directory;
        private readonly List<IAutoAction> autoActions = new List<IAutoAction>();
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
                if (PhotoRadioButton.IsChecked.Value)
                {
                    displayer = new PhotoDisplayer(DisplayArea, directory);
                }
                else if (VideoRadioButton.IsChecked.Value)
                {
                    displayer = new MovieDisplayer(DisplayArea, directory);
                }
            }
            foreach (var action in autoActions)
            {
                _ = action.Register(displayer);
            }
        }

        private void RefreshDir_Click(object sender, RoutedEventArgs e)
        {
            displayer?.ReloadContent();
        }

        private void AutoAdvanceTime_LostFocus(object sender, RoutedEventArgs e)
        {
            var text = (sender as TextBox)?.Text;
            var parsingResult = int.TryParse(text, out int seconds);
            var autoAdvancer = autoActions.FirstOrDefault(a => a is AutoAdvancer);
            if (autoAdvancer != null)
            {
                autoActions.Remove(autoAdvancer.Unregister());
            }
            if (!string.IsNullOrEmpty(text) && parsingResult && seconds > 0)
            {
                autoActions.Add(new AutoAdvancer(displayer, seconds));
            }
        }
    }
}
