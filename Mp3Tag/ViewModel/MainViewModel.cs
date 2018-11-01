using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Mp3Tag.ViewModel
{
  public class MainViewModel : ViewModelBase
  {
    private string _workingDirectoryPath;

    public MainViewModel()
    {
      TOtoCommand = new RelayCommand(OpenFileDialog);
    }

    private void OpenFileDialog()
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      if (openFileDialog.ShowDialog() == true)
        WorkingDirectoryPath = File.ReadAllText(openFileDialog.FileName);
    }

    public string WorkingDirectoryPath
    {
      get { return _workingDirectoryPath; }
      set { _workingDirectoryPath = value;
        RaisePropertyChanged();
      }
    }
    public ICommand TOtoCommand { get; set; }

    public List<Song> GetAllSongs()
    {
      var songs = new List<Song>();
      //var trackFiles = Directory.GetFiles(@"C:\Users\mme\Desktop\dev-dir", "*.mp3", SearchOption.AllDirectories);
      var trackFiles = Directory.GetFiles(@"D:\Matthias\Musique\Compilations", "*.mp3", SearchOption.AllDirectories);
      var regex = new Regex(@"(?<date>[0-9]{4}-[0-9]{2})\\(?<cd>CD [0-9])\\(?<number>[0-9]{2}) - (?<artist>[a-zA-Z0-9-(.) ]+) - (?<title>[a-zA-Z0-9 (.)']+)\.mp3");
      foreach (var file in trackFiles)
      {
        var match = regex.Match(file);
        if (match.Success)
        {
          songs.Add(new Song
          {
            Artist = match.Groups["artist"].Value,
            Album = match.Groups["date"].Value,
            Folder = match.Groups["cd"].Value,
            Number = match.Groups["number"].Value,
            Path = file,
            Title = match.Groups["title"].Value
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