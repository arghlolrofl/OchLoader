using OchLoader.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OchLoader.Model {
  public class Series {
    private int seriesId;
    public int SeriesId {
      get { return seriesId; }
      set { seriesId = value; }
    }

    private string addr;
    public string Addr {
      get { return addr; }
      set { addr = value; }
    }

    private ObservableCollection<ISeason> seasons = new ObservableCollection<ISeason>();
    public ObservableCollection<ISeason> Seasons {
      get { return seasons; }
      set { seasons = value; }
    }


  }
}
