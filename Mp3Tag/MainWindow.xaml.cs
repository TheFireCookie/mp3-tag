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

  // Regex V1
  // [0-9]{4}-[0-9]{2}\\CD [0-9]\\[0-9]{2} - [a-zA-Z0-9-(.)]+ - [a-zA-Z0-9 (.)]+

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
