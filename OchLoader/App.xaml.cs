using System.Reflection;
using System.Windows;
using Autofac;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using OchLoader.Contracts;
using OchLoader.Message;
using OchLoader.Model.Search;
using OchLoader.View.Main;
using OchLoader.ViewModel.Start;

namespace OchLoader
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    private static IContainer container;

    private ILifetimeScope _scope;

    public static IContainer Container
    {
      get
      {
        if (container == null)
          Bootstrap();

        return container;
      }
    }

    /// <summary>
    /// Creates the IoC-Container for the application
    /// </summary>
    private static void Bootstrap()
    {
      ContainerBuilder builder = new ContainerBuilder();

      //if (ViewModelBase.IsInDesignModeStatic)
      //{
      //    // Create design time view services and models
      //    SimpleIoc.Default.Register<IDataService, DesignDataService>();
      //}
      //else
      //{
      //    // Create run time view services and models
      //    SimpleIoc.Default.Register<IDataService, DataService>();
      //}

      // Views
      builder.RegisterType<ApplicationWindow>().SingleInstance();

      // ViewModels
      builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
        .Where((t) => t.IsSubclassOf(typeof(ViewModelBase)))
        .SingleInstance();

      // Entities
      builder.RegisterType<KinoxContentSearch>().As<IContentSearch>();

      // Others

      container = builder.Build();
    }

    /// <summary>
    /// Initializes the application
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Application_Startup(object sender, StartupEventArgs e)
    {
      _scope = Container.BeginLifetimeScope();
      _scope.Resolve<ApplicationWindow>().Show();

      var m = new ActivateViewMessage<StartViewModel>();

      Messenger.Default.Send(new ActivateViewMessage<StartViewModel>());
    }
  }
}
