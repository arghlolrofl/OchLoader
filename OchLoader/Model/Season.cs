using OchLoader.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OchLoader.Model {
  public class Season : ISeason {
    readonly IList<int> episodes;
    public IList<int> Episodes {
      get {
        return episodes;
      }
    }

    private int number;
    public int Number {
      get { return number; }
      set { number = value; }
    }

    public Season() {
      episodes = new List<int>();
    }

    public override string ToString() {
      return Number.ToString();
    }
  }
}
