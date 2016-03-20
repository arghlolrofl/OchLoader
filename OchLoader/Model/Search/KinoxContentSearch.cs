using System;
using System.Collections.Generic;
using OchLoader.Contracts;
using System.Text.RegularExpressions;
using Autofac;

namespace OchLoader.Model.Search {
  public class KinoxContentSearch : IContentSearch {
    readonly ILifetimeScope _scope;

    const string BaseUrl = "http://kinox.to";
    const string InitialSearchQueryTemplate = BaseUrl + "/Search.html?q={0}";
    const string FilterInitialSearchResults = @"""RsltTableStatic"".+?<tbody>(?<rows>.+?)</tbody>";
    const string SplitInitialSearchResults = @"<tr( class=""(even|odd)"")?>(.+?)</tr>";
    const string MatchRowInfo = @"src=""/gr/sys/lng/(?<langId>[\d]+?)\.png.+?src=""/cs/themes/.+?/types/(?<type>.+?)\.png.+?<a href=""(?<href>.+?)"".+?>(?<name>.+?)</a>.+?Year"">(?<year>[\d]+?)</";

    WebRequest _request;

    public KinoxContentSearch(ILifetimeScope scope, WebRequest request) {
      _scope = scope;
      _request = request;
    }

    public IList<IGlobalSearchResult> SearchFor(string searchString) {
      string searchUrl = String.Format(InitialSearchQueryTemplate, searchString.Replace(" ", "+"));
      _request.Create(searchUrl);

      string response = _request.GetResponse();
      Regex regex = new Regex(FilterInitialSearchResults, RegexOptions.Singleline);
      Match match = regex.Match(response);

      if (!match.Success) {
        return null;
      }

      response = match.Groups["rows"].Value;

      // Split rows in tbody
      regex = new Regex(SplitInitialSearchResults, RegexOptions.Singleline);
      string[] rows = regex.Split(response);

      if (rows.Length == 0)
        return null;

      IList<IGlobalSearchResult> results = ConvertHtmlToGlobalSearchResults(rows);

      return results;
    }

    private IList<IGlobalSearchResult> ConvertHtmlToGlobalSearchResults(string[] rows) {
      Regex regex = new Regex(MatchRowInfo, RegexOptions.Singleline | RegexOptions.Compiled);
      IList<IGlobalSearchResult> results = new List<IGlobalSearchResult>();

      foreach (string rowHtml in rows) {
        if (String.IsNullOrWhiteSpace(rowHtml))
          continue;

        Match match = regex.Match(rowHtml);
        if (!match.Success)
          continue;

        string languageId = match.Groups["langId"].Value;
        string type = match.Groups["type"].Value;
        string href = match.Groups["href"].Value;
        string name = match.Groups["name"].Value;
        string year = match.Groups["year"].Value;

        IGlobalSearchResult result = _scope.Resolve<IGlobalSearchResult>();
        result.RelativePath = href;
        result.Year = Convert.ToInt32(year);
        result.Name = name;
        
        switch (languageId) {
          case "1":
            result.Language = Contracts.Enums.MediaLanguage.German;
            break;
          case "2":
            result.Language = Contracts.Enums.MediaLanguage.English;
            break;
          case "15":
            result.Language = Contracts.Enums.MediaLanguage.EnglishGermanSubbed;
            break;
          default:
            break;
        }

        switch (type) {
          case "movie":
            result.Type = Contracts.Enums.MediaType.Movie;
            break;
          case "documentation":
            result.Type = Contracts.Enums.MediaType.Documentation;
            break;
          case "series":
            result.Type = Contracts.Enums.MediaType.Series;
            break;
          default:
            break;
        }

        results.Add(result);
      }

      return results;
    }
  }
}
