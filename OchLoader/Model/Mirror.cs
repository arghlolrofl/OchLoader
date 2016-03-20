using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OchLoader.Model {
  public class Mirror {
    public enum ReferrerAvailability { Unknown, Up, Down }

    const string UrlTemplate = @"http://kinox.to/aGET/Mirror/{0}&Hoster={1}&Mirror={2}&Season={3}&Episode={4}";

    static readonly WebClient _webClient = new WebClient();
    Series _series;
    int _seasonNumber, _episodeNumber;

    public int HosterId { get; set; }
    public string HosterName { get; set; }
    public int Number { get; set; }
    public string Url { get {
        return String.Format(UrlTemplate, _series.Addr, HosterId, Number, _seasonNumber, _episodeNumber);
      }
    }
    public string Referrer { get; private set; }
    public ReferrerAvailability Availability { get; set; }

    public Mirror(Series series, int seasonNumber, int episodeNumber) {
      _series = series;
      _seasonNumber = seasonNumber;
      _episodeNumber = episodeNumber;
    }

    public void FetchReferrer() {
      if (!String.IsNullOrEmpty(Referrer))
        return;
            
      string jsonResponse = _webClient.DownloadString(Url).Replace("\\\\", "\\").Replace("\\\"", "\"");

      Regex regex = new Regex(@"""Stream"":""<a href=""(?<referrerUrl>.+?)""");
      Match match = regex.Match(jsonResponse);
      if (match.Success) {
        Referrer = match.Groups["referrerUrl"].Value.Replace("\\/", "/");
        CheckAvailability();
      }
    }

    private void CheckAvailability() {
      string response = _webClient.DownloadString(Referrer);
      if (response.Contains("Datei nicht gefunden"))
        Availability = ReferrerAvailability.Down;
      else
        Availability = ReferrerAvailability.Up;
    }

    public override string ToString() {
      return HosterId + " | " + HosterName;
    }
  }
}
