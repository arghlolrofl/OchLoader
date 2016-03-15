using System.Reflection;
using System.Windows;
using Autofac;
using OchLoader.View.Main;

namespace OchLoader
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    private ILifetimeScope _scope;

    private static IContainer container;

    public static IContainer Container
    {
      get
      {
        if (container == null)
          Bootstrap();

        return container;
      }

      private set { container = value; }
    }


    private static void Bootstrap()
    {
      ContainerBuilder builder = new ContainerBuilder();

      // Views
      builder.RegisterType<ApplicationWindow>().SingleInstance();

      // ViewModels
      builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
        .Where((t) => t.IsSubclassOf(typeof(GalaSoft.MvvmLight.ViewModelBase)))
        .SingleInstance();

      // Entities

      // Others

      container = builder.Build();
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
      _scope = Container.BeginLifetimeScope();
      _scope.Resolve<ApplicationWindow>().Show();

      //Messenger.Default.Send();
    }
  }
}
