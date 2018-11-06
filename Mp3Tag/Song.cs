using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using TagLib;

namespace Mp3Tag
{
  public class Song
  {
    public string Date { get; set; }
    public string Cd { get; set; }
    public string Number { get; set; }
    public string Artist { get; set; }
    public string Title { get; set; }
    public string Path { get; set; }
    public bool IsInError { get; set; }
    public string TagArtist { get;set; }
    public string TagAlbum { get; set; }
    public Bitmap TagThumbnail { get; set; }
    public string TagTitle { get; set; }
    public string TagNumber { get; set; }
    public string TagCd { get; set; }
  }
}
