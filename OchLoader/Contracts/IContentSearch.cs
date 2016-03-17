using System;
using System.Collections.Generic;

namespace OchLoader.Contracts
{
  public interface IContentSearch
  {
    IList<Uri> SearchFor(string searchString);
  }
}
