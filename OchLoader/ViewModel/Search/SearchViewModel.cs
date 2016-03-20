using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Input;
using System;
using Autofac;
using OchLoader.Contracts;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using OchLoader.Message;
using OchLoader.ViewModel.Episodes;

namespace OchLoader.ViewModel.Search {
  public class SearchViewModel : ViewModelBase {
    private readonly ILifetimeScope _scope;
    
    public bool CanSwitchToSubView {
      get { return SelectedSearchResult != null; }
    }

    private string searchString;
    public string SearchString {
      get { return searchString; }
      set { searchString = value; RaisePropertyChanged(); }
    }

    private IGlobalSearchResult selectedSearchResult;
    public IGlobalSearchResult SelectedSearchResult {
      get { return selectedSearchResult; }
      set {
        selectedSearchResult = value;
        RaisePropertyChanged();
        RaisePropertyChanged(nameof(CanSwitchToSubView));
      }
    }

    private ObservableCollection<IGlobalSearchResult> listOfSearchResults;
    public ObservableCollection<IGlobalSearchResult> ListOfSearchResults {
      get { return listOfSearchResults; }
      set { listOfSearchResults = value; RaisePropertyChanged(); }
    }

    private ICommand initialSearchCommand;
    public ICommand InitialSearchCommand {
      get {
        return initialSearchCommand ?? (initialSearchCommand = new RelayCommand(InitialSearchCommand_OnExecute));
      }
    }

    private ICommand switchToEpisodesViewCommand;
    public ICommand SwitchToEpisodesViewCommand {
      get { return switchToEpisodesViewCommand ?? (switchToEpisodesViewCommand = new RelayCommand(SwitchToEpisodesViewCommand_OnExecute)); }
    }

    public SearchViewModel(ILifetimeScope scope) {
      _scope = scope;

      PropertyChanged += StartViewModel_OnPropertyChanged;
    }

    private void SwitchToEpisodesViewCommand_OnExecute() {
      MessengerInstance.Send(new ActivateViewMessage(typeof(EpisodesViewModel)));
    }

    private void InitialSearchCommand_OnExecute() {
      IContentSearch search = _scope.Resolve<IContentSearch>();
      IList<IGlobalSearchResult> results = search.SearchFor(SearchString);

      ListOfSearchResults = new ObservableCollection<IGlobalSearchResult>(results);
    }

    private void StartViewModel_OnPropertyChanged(object sender, PropertyChangedEventArgs e) {
      switch (e.PropertyName) {
        default:
          break;
      }
    }
  }
}
