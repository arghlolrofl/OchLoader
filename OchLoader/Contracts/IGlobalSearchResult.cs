using OchLoader.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OchLoader.Contracts {
  public interface IGlobalSearchResult {
    int Year { get; set; }
    string Name { get; set; }
    string RelativePath { get; set; }
    string Url { get; }
    MediaType Type { get; set; }
    MediaLanguage Language { get; set; }
  }
}
