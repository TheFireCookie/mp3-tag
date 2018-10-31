using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
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
    public bool IsInError { get; set; }
  }
}
