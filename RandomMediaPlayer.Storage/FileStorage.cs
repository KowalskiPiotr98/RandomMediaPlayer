using System.IO;

namespace RandomMediaPlayer.Storage
{
    public static class FileStorage
    {
        public static string FileStoragePath
        {
            get
            {
#if DEBUG
                return
                    $"{System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData)}\\RandomMediaPlayer-DEBUG";
#else
                return
                    $"{System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData)}\\RandomMediaPlayer";
#endif
            }
        }

        public static void EnsureFileStructureIsPresent()
        {
            EnsureDirectoryExists(FileStoragePath);
        }

        private static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}