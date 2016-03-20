using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OchLoader.Contracts {
  public interface ISeason {
    int Number { get; set; }
    IList<int> Episodes { get; }
  }
}
