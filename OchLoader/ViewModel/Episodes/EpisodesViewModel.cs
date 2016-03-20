using Autofac;
using GalaSoft.MvvmLight;
using OchLoader.Contracts;
using OchLoader.ViewModel.Search;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using OchLoader.Message;
using OchLoader.Model;
using System.Windows.Data;
using System.Collections;

namespace OchLoader.ViewModel.Episodes {
  public class EpisodesViewModel : ViewModelBase {
    const string KinoxMirrorByEpisode = @"http://kinox.to/aGET/MirrorByEpisode/?Addr={0}&SeriesID={1}&Season={2}&Episode={3}";

    private readonly ILifetimeScope _scope;
    private readonly SearchViewModel _parent;
    private readonly WebClient webClient = new WebClient();

    // Series Info
    const string PatternSeriesInfo = @"<select.+?""SeasonSelection"" rel=""\?Addr=(?<addr>.+?)&amp;SeriesID=(?<seriesId>[\d]+?)"".+?(?<seasons><option value=""1"".+?)</select>";
    readonly Regex regexSeriesInfo = new Regex(PatternSeriesInfo, RegexOptions.Singleline | RegexOptions.Compiled);

    // Series Info
    const string PatternEpisodeInfo = @"<option value=""(?<s>\d+?)"" rel=""(?<e>.+?)""";
    readonly Regex regexEpisodeInfo = new Regex(PatternEpisodeInfo, RegexOptions.Singleline | RegexOptions.Compiled);

    // MirrorInfo
    const string PatternMirrorInfo = @"<li id=""Hoster_(?<hosterId>\d+?)"".+?rel=.+?class=""Named"">(?<hosterName>.+?)</div>.+?/b>:.+?/(?<count>.+?)<br/>";
    readonly Regex regexMirrorInfo = new Regex(PatternMirrorInfo, RegexOptions.Singleline | RegexOptions.Compiled);

    private ISeason selectedSeason;
    public ISeason SelectedSeason {
      get { return selectedSeason; }
      set { selectedSeason = value; RaisePropertyChanged(); }
    }

    private int selectedEpisode;
    public int SelectedEpisode {
      get { return selectedEpisode; }
      set { selectedEpisode = value; RaisePropertyChanged(); }
    }

    private Series series;
    public Series Series {
      get { return series; }
      set { series = value; RaisePropertyChanged(); }
    }

    private ListCollectionView mirrors;
    public ListCollectionView Mirrors {
      get { return mirrors; }
      set {
        mirrors = value;
        mirrors?.GroupDescriptions.Add(new PropertyGroupDescription(nameof(SelectedMirror.HosterName)));
        RaisePropertyChanged();
      }
    }

    private Mirror selectedMirror;
    public Mirror SelectedMirror {
      get { return selectedMirror; }
      set {
        selectedMirror = value;
        RaisePropertyChanged();
      }
    }

    private ICommand backToParentViewCommand;
    public ICommand BackToParentViewCommand {
      get { return backToParentViewCommand ?? (backToParentViewCommand = new RelayCommand(BackToParentViewCommand_OnExecute)); }
    }

    public EpisodesViewModel(ILifetimeScope scope, SearchViewModel searchViewModel) {
      _scope = scope;
      _parent = searchViewModel;

      PropertyChanged += EpisodesViewModel_OnPropertyChanged;

      GET_EpisodeInfo();
    }

    private void EpisodesViewModel_OnPropertyChanged(object sender, PropertyChangedEventArgs e) {
      switch (e.PropertyName) {
        case nameof(Series.Seasons):
          if (Series.Seasons != null)
            SelectedSeason = Series.Seasons.FirstOrDefault();
          break;
        case nameof(SelectedSeason):
          if (SelectedSeason != null)
            SelectedEpisode = SelectedSeason.Episodes.FirstOrDefault();
          break;
        case nameof(SelectedEpisode):
          if (SelectedEpisode > 0)
            GET_AllMirrors();
          break;
        case nameof(SelectedMirror):
          SelectedMirror?.FetchReferrer();
          RaisePropertyChanged(nameof(SelectedMirror.Referrer));
          RaisePropertyChanged(nameof(SelectedMirror.Availability));
          if (SelectedMirror != null) {
            Mirrors.EditItem(SelectedMirror);
            Mirrors.CommitEdit();
          }
          break;
        default:
          break;
      }
    }

    private void BackToParentViewCommand_OnExecute() {
      MessengerInstance.Send(new ActivateViewMessage(_parent));
    }

    private void GET_EpisodeInfo() {
      string response = webClient.DownloadString(_parent.SelectedSearchResult.Url);

      Match match = regexSeriesInfo.Match(response);
      if (!match.Success)
        return;

      string seasons = match.Groups["seasons"].Value;
      string addr = match.Groups["addr"].Value;
      string seriesId = match.Groups["seriesId"].Value;

      IList<ISeason> seasonList = ParseNumberOfEpisodes(seasons);

      Series = _scope.Resolve<Series>();
      Series.Addr = addr;
      Series.SeriesId = Convert.ToInt32(seriesId);
      Series.Seasons = new ObservableCollection<ISeason>(seasonList);
      RaisePropertyChanged(nameof(Series.Seasons));
    }

    private IList<ISeason> ParseNumberOfEpisodes(string seasons) {
      IList<ISeason> seasonList = new List<ISeason>();
      MatchCollection matches = regexEpisodeInfo.Matches(seasons);

      foreach (Match match in matches) {
        if (!match.Success)
          continue;

        ISeason season = _scope.Resolve<ISeason>();
        season.Number = Convert.ToInt32(match.Groups["s"].Value);
        string[] episodes = match.Groups["e"].Value.Split(',');
        foreach (string episode in episodes)
          season.Episodes.Add(Convert.ToInt32(episode));

        seasonList.Add(season);
      }

      return seasonList;
    }

    private void GET_AllMirrors() {
      IList mirrors = new List<Mirror>();

      string url = String.Format(KinoxMirrorByEpisode, Series.Addr, Series.SeriesId, SelectedSeason.Number, SelectedEpisode);
      string response = webClient.DownloadString(url);

      MatchCollection matches = regexMirrorInfo.Matches(response);
      foreach (Match match in matches) {
        string hosterId = match.Groups["hosterId"].Value;
        string hosterName = match.Groups["hosterName"].Value;
        string mirrorCount = match.Groups["count"].Value;

        int count = 0;
        if (!Int32.TryParse(mirrorCount, out count))
          continue;

        for (int x = 1; x <= count; x++) {
          Mirror mirror = new Mirror(Series, SelectedSeason.Number, SelectedEpisode);
          mirror.HosterId = hosterId;
          mirror.HosterName = hosterName;
          mirror.Number = x;

          mirrors.Add(mirror);
        }
      }

      Mirrors = new ListCollectionView(mirrors);
    }
  }
}
