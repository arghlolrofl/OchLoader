using System.Reflection;
using System.Windows;
using Autofac;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using OchLoader.Contracts;
using OchLoader.Message;
using OchLoader.Model.Search;
using OchLoader.View.Main;
using OchLoader.Model;
using OchLoader.View.Search;
using OchLoader.ViewModel.Search;
using OchLoader.View.Episodes;

namespace OchLoader {
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application {
    private static IContainer container;

    private ILifetimeScope _scope;

    public static IContainer Container {
      get {
        if (container == null)
          Bootstrap();

        return container;
      }
    }

    /// <summary>
    /// Creates the IoC-Container for the application
    /// </summary>
    private static void Bootstrap() {
      ContainerBuilder builder = new ContainerBuilder();

      // Views
      builder.RegisterType<ApplicationWindow>().SingleInstance();
      builder.RegisterType<SearchView>().SingleInstance();
      builder.RegisterType<EpisodesView>().SingleInstance();

      // ViewModels
      builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
             .Where((t) => t.IsSubclassOf(typeof(ViewModelBase)))
             .SingleInstance();

      // Entities
      builder.RegisterType<KinoxContentSearch>().As<IContentSearch>();
      builder.RegisterType<GlobalSearchResultKinox>().As<IGlobalSearchResult>();
      builder.RegisterType<Series>();
      builder.RegisterType<Season>().As<ISeason>();

      // Others
      builder.RegisterType<WebRequest>();

      container = builder.Build();
    }

    /// <summary>
    /// Initializes the application
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Application_Startup(object sender, StartupEventArgs e) {
      _scope = Container.BeginLifetimeScope();
      _scope.Resolve<ApplicationWindow>().Show();

      Messenger.Default.Send(new ActivateViewMessage(typeof(SearchViewModel)));
    }
  }
}
