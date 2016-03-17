using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OchLoader.Contracts;
using OchLoader.Model.Search;

namespace OchLoader.Test
{
  [TestClass]
  public class BasicFixture
  {
    const string testSearchString1 = "abc123";

    [TestMethod]
    public void TestContentSearchReturnsResult()
    {
      IContentSearch contentSearch = new KinoxContentSearch();
      IList<Uri> searchResults = contentSearch.SearchFor(testSearchString1);

      Assert.IsTrue(searchResults.Any());
    }
  }
}
