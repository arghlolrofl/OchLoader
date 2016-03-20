using System;
using System.Collections.Generic;

namespace OchLoader.Contracts
{
  public interface IContentSearch
  {
    IList<IGlobalSearchResult> SearchFor(string searchString);
  }
}
