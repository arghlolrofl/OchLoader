using System;

namespace OchLoader.Message
{
  public class ActivateViewMessage
  {
    private Type viewModelType;

    public Type ViewModelType
    {
      get { return viewModelType; }
      private set { viewModelType = value; }
    }

    public ActivateViewMessage(Type t)
    {
      ViewModelType = t;
    }
  }
}
