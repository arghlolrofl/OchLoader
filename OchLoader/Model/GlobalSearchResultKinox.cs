using OchLoader.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OchLoader.Contracts.Enums;

namespace OchLoader.Model {
  public class GlobalSearchResultKinox : IGlobalSearchResult {
    const string BaseUrl = "http://kinox.to";

    public MediaLanguage Language { get; set; }
    public string Name { get; set; }
    public string RelativePath { get; set; }
    public MediaType Type { get; set; }
    public int Year { get; set; }
    public string Url {
      get {
        return BaseUrl + RelativePath;
      }
    }
  }
}
