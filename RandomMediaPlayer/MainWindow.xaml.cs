﻿using Microsoft.Win32;
using RandomMediaPlayer.Actions;
using RandomMediaPlayer.Core.Directory;
using RandomMediaPlayer.Core.Displayers;
using RandomMediaPlayer.MoviePlayer;
using RandomMediaPlayer.PhotoShower;
using RandomMediaPlayer.PhotoShower.PhotosDirectory;
using RandomMediaPlayer.SelfUpdater.GitHubConnection;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using RandomMediaPlayer.HistoryTracking;

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
        private bool isFullScreen;
        private Task updateCheckTask;
        private readonly UpdateManager updateManager;
        public MainWindow()
        {
            InitializeComponent();
            updateManager = new UpdateManager(App.Version);
#if RELEASE
            updateCheckTask = CheckForUpdatesAsync(interactive: false);
#endif
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
            SetDisplayedTest();
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
            if (displayer is Displayer internalDisplayer)
            {
                internalDisplayer.ClearDisplayArea();
            }
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
            if (displayer is RandomMediaPlayer.HistoryTracking.IHistoryTracking historyDisplayer)
            {
                TrackHistory.Visibility = Visibility.Visible;
                SeenTotalMedia.Visibility = Visibility.Visible;
                TrackHistory.IsChecked = historyDisplayer.HistoryTracker.IsTracking;
            }
            else
            {
                TrackHistory.Visibility = Visibility.Collapsed;
                SeenTotalMedia.Visibility = Visibility.Collapsed;
            }
            ApplyMinHeight();
            SetDisplayedTest();
        }

        private void SetDisplayedTest()
        {
            TitleDisplay.Text = displayer?.CurrentDisplayableName;
            var seenMedia = ((displayer as IHistoryTracking)?.HistoryTracker?.GetSeenWithExtensions(displayer?.DirectoryPicker?.AllowedExtensions) ?? 0).ToString();
            var totalMedia = (displayer?.DirectoryPicker?.TotalDisplayables ?? 0).ToString();
            SeenTotalMedia.Text = $"{seenMedia}/{totalMedia}";
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

        private void FullscreenToggler_Click(object sender, RoutedEventArgs e)
        {
            FullScreenToggle();
        }

        private void TrackHistory_Click(object sender, RoutedEventArgs e)
        {
            if (displayer is RandomMediaPlayer.HistoryTracking.IHistoryTracking historyDisplayer)
            {
                historyDisplayer.HistoryTracker.IsTracking = TrackHistory.IsChecked ?? false;
            }
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape && isFullScreen)
            {
                FullScreenToggle();
            }
        }

        private void FullScreenToggle()
        {
            if (!isFullScreen)
            {
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;
                isFullScreen = true;
            }
            else
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
                WindowState = WindowState.Normal;
                isFullScreen = false;
            }
        }

        private void Title_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TitleDisplay.Visibility = Visibility.Collapsed;
        }

        private async Task CheckForUpdatesAsync(bool interactive)
        {
            if (updateCheckTask != null && !updateCheckTask.IsCompleted)
            {
                await updateCheckTask;
                return;
            }
            if (await updateManager.IsUpdateAvailableAsync().ConfigureAwait(false))
            {
                var result = MessageBox.Show("There is a new update available, do you want to download it?", "Update available", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    if (await updateManager.RunUpdaterAsync().ConfigureAwait(false))
                    {
                        Dispatcher.Invoke(() => App.CurrentApp.Shutdown());
                    }
                    else
                    {
                        _ = MessageBox.Show("The new update couldn't be installer, please try again later.", "Update installation error", MessageBoxButton.OK, MessageBoxImage.Error);
                        updateManager.ClearCache();
                    }
                }
            }
            else if (interactive)
            {
                _ = MessageBox.Show("There are no updates available", "No new updates", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async void CheckForUpdates_Click(object sender, RoutedEventArgs e)
        {
            await CheckForUpdatesAsync(interactive: true);
        }

        private void ApplyMinHeight()
        {
            MenuColumn.UpdateLayout();
            MinHeight = MenuColumn.ActualHeight + SystemParameters.WindowCaptionHeight + 25; //I have no idea why this 25 is necessary here. That's UI for you I guess.
        }

        private void AboutMe_OnClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                $"Random Media Player, version: {App.Version}\nWould you like to see the GitHub page?",
                "About me", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "explorer.exe",
                    Arguments = "https://github.com/KowalskiPiotr98/RandomMediaPlayer",
                    UseShellExecute = true
                });
            }
        }
    }
}
