using Autofac;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using OchLoader.Message;
using OchLoader.ViewModel.Start;

namespace OchLoader.ViewModel.Main
{
  /// <summary>
  /// This class contains properties that the main View can data bind to.
  /// <para>
  /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
  /// </para>
  /// <para>
  /// You can also use Blend to data bind with the tool's support.
  /// </para>
  /// <para>
  /// See http://www.galasoft.ch/mvvm
  /// </para>
  /// </summary>
  public class ApplicationViewModel : ViewModelBase
  {
    private ILifetimeScope _scope;

    public ViewModelBase FocusedViewModel { get; private set; }

    /// <summary>
    /// Initializes a new instance of the ApplicationViewModel class.
    /// </summary>
    public ApplicationViewModel()
    {
      _scope = App.Container.BeginLifetimeScope();

      //if (IsInDesignMode)
      //{
      //    // Code runs in Blend --> create design time data.
      //}
      //else
      //{
      //    // Code runs "for real"
      //}

      Messenger.Default.Register<ActivateViewMessage>(this, HandleViewRequest);
    }

    private void HandleViewRequest(ActivateViewMessage msg)
    {
      //MethodInfo methodInfo = this.GetType().GetMethod("Cast").MakeGenericMethod(msg.ViewModelType);

      object viewModelObject = _scope.Resolve(msg.ViewModelType);

      //FocusedViewModel = (ViewModelBase)viewModelObject;

      switch (msg.ViewModelType.Name)
      {
        case nameof(StartViewModel):
          FocusedViewModel = (StartViewModel)viewModelObject;
          break;
        default:
          break;
      }
    }

    private static T Cast<T>(object obj)
    {
      return (T)obj;
    }
  }
}