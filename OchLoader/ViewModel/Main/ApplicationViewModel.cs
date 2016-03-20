using Autofac;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using OchLoader.Message;

namespace OchLoader.ViewModel.Main {
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
  public class ApplicationViewModel : ViewModelBase {
    private ILifetimeScope _scope;

    private ViewModelBase focusedViewModel;
    public ViewModelBase FocusedViewModel {
      get { return focusedViewModel; }
      private set {
        focusedViewModel = value;
        RaisePropertyChanged();
      }
    }

    /// <summary>
    /// Initializes a new instance of the ApplicationViewModel class.
    /// </summary>
    public ApplicationViewModel(ILifetimeScope scope) {
      _scope = scope;

      Messenger.Default.Register<ActivateViewMessage>(this, HandleViewRequest);
    }

    private void HandleViewRequest(ActivateViewMessage msg) {
      if (msg.ViewModel == null)
        FocusedViewModel = (ViewModelBase)_scope.Resolve(msg.ViewModelType);
      else
        FocusedViewModel = msg.ViewModel;
    }
  }
}