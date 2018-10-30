using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Mp3Tag
{
  public class Song
  {
    public string Album { get; set; }
    public string Folder { get; set; }
    public string Number { get; set; }
    public string Artist { get; set; }
    public string Title { get; set; }
    public string Path { get; set; }
    public string Thumbnail { get; set; }
  }

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
      var files = Directory.GetFiles(@"C:\Users\mme\Desktop\dev-dir", "*.mp3", SearchOption.AllDirectories);
      return null;
    }

    public void Toto()
    {
      //TagLib.
    }
  }
}
