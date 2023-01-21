using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using TagLib.Mpeg;

namespace mp3tag.ViewModels
{
    public class MainWindowViewModel : PropertyChangedBase
    {
        private string _workingDirectoryPath;
        private BindableCollection<Song> _songs;
        private readonly Regex Regex = new Regex(
           @"(?<date>[0-9]{4}-[0-9]{2})\\(?<cd>CD [0-9])?\\?(?<number>[0-9]{2}) - (?<artist>[a-zA-Zàééèèøêâäáùüïôûûîåööëó+ÉÉÈÏÖñÇçÁÀÜłΛ★^♀#0-9 \-(.)_&Ø;,\[\]’`!$'º°]+) - (?<title>[a-zA-Zàééèèøêâäùüïôûûîåööëó+ÉÉÈÏÖÇçÁÀÜłΛ★^♀#0-9 \-(.)_&Ø;,\[\]’`  !$'º°]+)\.mp3", RegexOptions.Compiled);

        public MainWindowViewModel()
        {
            WorkingDirectoryPath = @"D:\Matthias\Musique\Compilations";
        }

        public string WorkingDirectoryPath
        {
            get { return _workingDirectoryPath; }
            set
            {
                _workingDirectoryPath = value;
                NotifyOfPropertyChange(() => WorkingDirectoryPath);
            }
        }

        public BindableCollection<Song> Songs { get { return _songs; } set { _songs = value; NotifyOfPropertyChange(() => Songs); } }

        public void OpenWorkingDirectory()
        {
            var folderBrowserDialog = new FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                WorkingDirectoryPath = folderBrowserDialog.SelectedPath;
        }

        public bool CanLoadWorkingDirectory()
        {
            return !string.IsNullOrEmpty(WorkingDirectoryPath);
        }

        public void LoadWorkingDirectory()
        {
            Songs = GetAllSongs();
        }

        public bool CanSetMp3Tag()
        {
            return true;
        }

        public void SetMp3Tag()
        {
            var songsToSet = Songs.Where(s => !s.IsInError).ToList();
            for (int i = 0; i < songsToSet.Count; i++)
            {
                Debug.WriteLine($"Writing {songsToSet[i].Path} ({i}/{songsToSet.Count - 1})");
                var audioFile = (AudioFile)TagLib.File.Create(songsToSet[i].Path);
                audioFile.Tag.Performers = new[] { songsToSet[i].Artist };
                audioFile.Tag.Title = songsToSet[i].Title;


                if (string.IsNullOrWhiteSpace(songsToSet[i].Cd))
                {
                    audioFile.Tag.Album = $"{songsToSet[i].Date}";
                    audioFile.Tag.Disc = 1;
                }
                else
                {
                    audioFile.Tag.Album = $"{songsToSet[i].Date} - {songsToSet[i].Cd}";
                    audioFile.Tag.Disc = Convert.ToUInt32(songsToSet[i].Cd.Substring(3, 1));
                }

                audioFile.Tag.Track = Convert.ToUInt32(songsToSet[i].Number);
                audioFile.Save();
                Debug.WriteLine($"Written {songsToSet[i]} ({i}/{songsToSet.Count - 1})");
            }

            LoadWorkingDirectory();
        }

        public void SaveMp3FileToClipboard()
        {
            var stringBuilder = new StringBuilder();
            foreach (var song in Songs)
            {
                stringBuilder.AppendLine(song.Path);
            }
            System.Windows.Clipboard.SetText(stringBuilder.ToString());
        }

        private BindableCollection<Song> GetAllSongs()
        {
            var songs = new ConcurrentBag<Song>();
            var trackFiles = Directory.GetFiles(WorkingDirectoryPath, "*.mp3", SearchOption.AllDirectories);

            Parallel.ForEach(trackFiles, track =>
                 {
                     Debug.WriteLine($"Reading {track}");

                     var audioFile = (AudioFile)TagLib.File.Create(track);

                     var match = Regex.Match(track);
                     if (match.Success)
                     {
                         var picture = audioFile.Tag.Pictures.FirstOrDefault();

                         var song = new Song
                         {
                             Artist = match.Groups["artist"].Value,
                             Date = match.Groups["date"].Value,
                             Cd = match.Groups["cd"].Value,
                             Number = match.Groups["number"].Value,
                             Path = track,
                             Title = match.Groups["title"].Value,
                             TagAlbum = audioFile.Tag.Album,
                             TagArtist = audioFile.Tag.Performers.FirstOrDefault(),
                             TagCd = audioFile.Tag.Disc.ToString(),
                             TagNumber = audioFile.Tag.Track.ToString(),
                             TagTitle = audioFile.Tag.Title
                         };

                         if (picture != null)
                         {
                             try
                             {
                                 var base64 = Convert.ToBase64String(audioFile.Tag.Pictures[0].Data.Data);
                                 using var ms = new MemoryStream(audioFile.Tag.Pictures[0].Data.Data);
                                 var bitmapImage = new BitmapImage();
                                 bitmapImage.BeginInit();
                                 bitmapImage.StreamSource = ms;
                                 bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                                 bitmapImage.EndInit();
                                 bitmapImage.Freeze();
                                 song.TagThumbnail = bitmapImage;
                             }
                             catch (Exception)
                             { }
                         }

                         songs.Add(song);
                     }
                     else
                     {
                         songs.Add(new Song
                         {
                             Path = track,
                             IsInError = true
                         });
                     }
                     Debug.WriteLine($"Read {track})");
                 });

            return new BindableCollection<Song>(songs);
        }
    }
}