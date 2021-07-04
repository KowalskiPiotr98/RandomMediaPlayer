using RandomMediaPlayer.Core.Displayables;
using System.Collections.Generic;
using System.Linq;
using RandomMediaPlayer.HistoryTracking;

namespace RandomMediaPlayer.Core.Directory
{
    public abstract class DirectoryPicker : IDirectoryPicker
    {
        private bool isEmpty;
        public bool IsEmpty { get => isEmpty; }
        public string[] AllowedExtensions { get; protected set; }
        public System.Uri Directory { get => directory; }
        public int TotalDisplayables => displayables.Count;
        public string BasePath => Directory.AbsolutePath;
        protected System.Uri directory;
        protected List<IDisplayable> displayables;

        protected DirectoryPicker(System.Uri directory)
        {
            AllowedExtensions = System.Array.Empty<string>();
            this.directory = directory;
        }

        public void ReadDisplayables()
        {
            var files = System.IO.Directory.GetFiles(directory.LocalPath).Where(name => AllowedExtensions.Contains(name.Split('.').Last().ToLower())).ToList();
            var tempDisplayables = new List<IDisplayable>(files.Count);
            isEmpty = true;
            foreach (var file in files)
            {
                tempDisplayables.Add(CreateDisplayableFromLocalPath(file));
                isEmpty = false;
            }
            displayables = tempDisplayables;
        }

        public IEnumerable<IDisplayable> GetDisplayables()
        {
            DisplayablesReadCheck();
            return displayables;
        }

        public IDisplayable GetRandomDisplayable()
        {
            DisplayablesReadCheck();
            var random = new System.Random();
            return displayables.ElementAtOrDefault(random.Next(0, displayables.Count));
        }
        public IDisplayable GetRandomDisplayable(HistoryTracker history)
        {
            if (!history.IsTracking)
            {
                return GetRandomDisplayable();
            }
            DisplayablesReadCheck();
            if (!displayables.Any())
            {
                return null;
            }
            var limitedDisplayables = history.LimitCollectionToNotInHistory(displayables, s => s.Source);
            IDisplayable displayable;
            var limitedDisplayablesList = limitedDisplayables.ToList();
            if (!limitedDisplayablesList.Any())
            {
                history.Clear();
                displayable = GetRandomDisplayable();
            }
            else
            {
                var random = new System.Random();
                displayable = limitedDisplayablesList.ElementAtOrDefault(random.Next(0, limitedDisplayablesList.Count));
            }
            history.AddToHistory(displayable?.Source);
            return displayable;
        }

        private void DisplayablesReadCheck()
        {
            if (displayables is null)
            {
                ReadDisplayables();
            }
        }

        protected abstract IDisplayable CreateDisplayableFromLocalPath(string path);
    }
}
