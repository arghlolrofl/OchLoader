using Microsoft.VisualStudio.TestTools.UnitTesting;
using OchLoader.Contracts;
using OchLoader.Model.Search;

namespace OchLoader.Test
{
  [TestClass]
  public class BasicFixture
  {
    [TestMethod]
    public void TestKinoxSearchIsWorking()
    {
      IContentSearch contentSearch = new KinoxContentSearch();
    }
  }
}
