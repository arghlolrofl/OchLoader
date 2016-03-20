using GalaSoft.MvvmLight;
using System;

namespace OchLoader.Message {
  public class ActivateViewMessage {
    private Type viewModelType;

    public Type ViewModelType {
      get { return viewModelType; }
      private set { viewModelType = value; }
    }

    private ViewModelBase viewModel;
    public ViewModelBase ViewModel {
      get { return viewModel; }
      set { viewModel = value; }
    }
    
    public ActivateViewMessage(Type t) {
      ViewModelType = t;
    }

    public ActivateViewMessage(ViewModelBase vm) {
      ViewModel = vm;
    }
  }
}
