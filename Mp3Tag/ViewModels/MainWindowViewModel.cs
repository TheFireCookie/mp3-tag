using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Caliburn.Micro;
using TagLib.Mpeg;

namespace mp3tag.ViewModels
{
  public class MainWindowViewModel : PropertyChangedBase
  {
    private string _workingDirectoryPath;
    private BindableCollection<Song> _songs;
    private readonly Regex Regex = new Regex(
       @"(?<date>[0-9]{4}-[0-9]{2})\\(?<cd>CD [0-9])?\\?(?<number>[0-9]{2}) - (?<artist>[a-zA-Zàéèøêâäùüïôûîåöëó+ÉÈÁΛ★#0-9 \-(.)_&Ø;,\[\]’!$'º°]+) - (?<title>[a-zA-Zàéèøêâäùüïôûîåöëó+ÉÈÁΛ★#0-9 \-(.)_&Ø;,\[\]’!$'º°]+)\.mp3", RegexOptions.Compiled);

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
      return false;
    }

    public void SetMp3Tag()
    {
      foreach (var song in Songs.Where(s => !s.IsInError).ToList())
      {
        var audioFile = (AudioFile)TagLib.File.Create(song.Path);
        audioFile.Tag.Performers = new[] { song.Artist };
        audioFile.Tag.Title = song.Title;
        audioFile.Tag.Disc = Convert.ToUInt32(song.Cd.Substring(3, 1));
        audioFile.Tag.Album = $"{song.Date} - {song.Cd}";
        audioFile.Tag.Track = Convert.ToUInt32(song.Number);
        audioFile.Save();
      }

      LoadWorkingDirectory();
    }

    private BindableCollection<Song> GetAllSongs()
    {
      var songs = new BindableCollection<Song>();
      var trackFiles = Directory.GetFiles(WorkingDirectoryPath, "*.mp3", SearchOption.AllDirectories);

      for (int i = 0; i < trackFiles.Length; i++)
      {
        Debug.WriteLine($"Processing {trackFiles[i]} ({i}/{trackFiles.Length - 1})");
        var audioFile = (AudioFile)TagLib.File.Create(trackFiles[i]);

        var match = Regex.Match(trackFiles[i]);
        if (match.Success)
        {
          songs.Add(new Song
          {
            Artist = match.Groups["artist"].Value,
            Date = match.Groups["date"].Value,
            Cd = match.Groups["cd"].Value,
            Number = match.Groups["number"].Value,
            Path = trackFiles[i],
            Title = match.Groups["title"].Value,
            TagAlbum = audioFile.Tag.Album,
            TagArtist = audioFile.Tag.Performers.FirstOrDefault(),
            TagCd = audioFile.Tag.Disc.ToString(),
            TagNumber = audioFile.Tag.Track.ToString(),
            TagThumbnail = @"C:\Users\mme\Desktop\dev-dir\thumbnail.jpg",
            TagTitle = audioFile.Tag.Title
          });
        }
        else
        {
          songs.Add(new Song
          {
            Path = trackFiles[i],
            IsInError = true
          });
        }
        Debug.WriteLine($"Processed {trackFiles[i]} ({i}/{trackFiles.Length - 1})");
      }

      //Parallel.ForEach(trackFiles, (file) =>
      //{
      //  Debug.WriteLine($"Processing {file}");
      //  var audioFile = (AudioFile) TagLib.File.Create(file);

      //  //var img = new Bitmap(@"C:\Users\mme\Desktop\dev-dir\thumbnail.jpg");

      //  var match = regex.Match(file);
      //  if (match.Success)
      //  {
      //    songs.Add(new Song
      //    {
      //      Artist = match.Groups["artist"].Value,
      //      Date = match.Groups["date"].Value,
      //      Cd = match.Groups["cd"].Value,
      //      Number = match.Groups["number"].Value,
      //      Path = file,
      //      Title = match.Groups["title"].Value,
      //      TagAlbum = audioFile.Tag.Album,
      //      TagArtist = audioFile.Tag.Performers.FirstOrDefault(),
      //      TagCd = audioFile.Tag.Disc.ToString(),
      //      TagNumber = audioFile.Tag.Track.ToString(),
      //      TagThumbnail = @"C:\Users\mme\Desktop\dev-dir\thumbnail.jpg",
      //      TagTitle = audioFile.Tag.Title
      //    });
      //  }
      //  else
      //  {
      //    songs.Add(new Song
      //    {
      //      Path = file,
      //      IsInError = true
      //    });
      //  }
      //  Debug.WriteLine($"Processed {file}");
      //});

      return songs;
    }
  }
}