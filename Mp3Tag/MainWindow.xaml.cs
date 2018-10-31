using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace Mp3Tag
{

  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
      GetAllSongs();
    }

    public List<Song> GetAllSongs()
    {
      var files = Directory.GetFiles(@"D:\Matthias\Musique\Compilations", "*.mp3", SearchOption.AllDirectories);
      var toto = new Regex(@"(?<date>[0-9]{4}-[0-9]{2})\\(?<cd>CD [0-9])\\(?<number>[0-9]{2}) - (?<artist>[a-zA-Z0-9-(.)]+) - (?<title>[a-zA-Z0-9 (.)']+)\.mp3");
      foreach(var file in files)
      {
        var regexResponses = toto.Match(file);
      }
      return null;
    }

    public void Toto()
    {
      //TagLib.
    }
  }
}
