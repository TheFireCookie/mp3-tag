using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Input;
using TagLib;
using TagLib.Mpeg;

namespace Mp3Tag.ViewModel
{
  public class MainViewModel : ViewModelBase
  {
    private string _workingDirectoryPath;
    private ObservableCollection<Song> _songs;

    public MainViewModel()
    {
      OpenWorkingDirectoryCommand = new RelayCommand(OpenWorkingDirectory);
      LoadWorkingDirectoryCommand = new RelayCommand(LoadWorkingDirectory, !string.IsNullOrEmpty(WorkingDirectoryPath));

      WorkingDirectoryPath = @"C:\Users\mme\Desktop\dev-dir";
      LoadWorkingDirectory();
    }

    private void OpenWorkingDirectory()
    {
      FolderBrowserDialog openFileDialog = new FolderBrowserDialog();
      if (openFileDialog.ShowDialog() == DialogResult.OK)
        WorkingDirectoryPath = openFileDialog.SelectedPath;
    }

    private void LoadWorkingDirectory()
    {
      Songs = GetAllSongs();
    }

    public string WorkingDirectoryPath
    {
      get { return _workingDirectoryPath; }
      set
      {
        _workingDirectoryPath = value;
        RaisePropertyChanged();
      }
    }

    public ObservableCollection<Song> Songs
    {
      get { return _songs; }
      set
      {
        _songs = value;
        RaisePropertyChanged();
      }
    }

    public ICommand OpenWorkingDirectoryCommand { get; set; }
    public ICommand LoadWorkingDirectoryCommand { get; set; }

    public ObservableCollection<Song> GetAllSongs()
    {
      var songs = new ObservableCollection<Song>();
      var trackFiles = Directory.GetFiles(WorkingDirectoryPath, "*.mp3", SearchOption.AllDirectories);
      //var trackFiles = Directory.GetFiles(@"D:\Matthias\Musique\Compilations", "*.mp3", SearchOption.AllDirectories);
      var regex = new Regex(@"(?<date>[0-9]{4}-[0-9]{2})\\(?<cd>CD [0-9])\\(?<number>[0-9]{2}) - (?<artist>[a-zA-Z0-9-(.) ]+) - (?<title>[a-zA-Z0-9 (.)']+)\.mp3");
      foreach (var file in trackFiles)
      {
        AudioFile audioFile = (AudioFile)TagLib.File.Create(file);
        audioFile.Tag.Pictures = new IPicture[] { new Picture(@"C:\Users\mme\Desktop\téléchargement.png"), };

        var img = new Bitmap(@"C:\Users\mme\Desktop\téléchargement.png");


        var match = regex.Match(file);
        if (match.Success)
        {
          songs.Add(new Song
          {
            Artist = match.Groups["artist"].Value,
            Date = match.Groups["date"].Value,
            Cd = match.Groups["cd"].Value,
            Number = match.Groups["number"].Value,
            Path = file,
            Title = match.Groups["title"].Value,
            TagAlbum = audioFile.Tag.Album,
            TagArtist = audioFile.Tag.Performers.FirstOrDefault(),
            TagCd = audioFile.Tag.Disc.ToString(),
            TagNumber = audioFile.Tag.Track.ToString(),
            TagThumbnail = img,
            TagTitle = audioFile.Tag.Title
          });
        }
        else
        {
          songs.Add(new Song
          {
            Path = file,
            IsInError = true
          });
        }
      }

      return songs;
    }
  }
}