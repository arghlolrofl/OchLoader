/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:OchLoader"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using Autofac;
using OchLoader.ViewModel.Main;
using OchLoader.ViewModel.Start;

namespace OchLoader.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        static readonly ILifetimeScope _scope;

        static ViewModelLocator()
        {
            if (_scope == null)
                _scope = App.Container.BeginLifetimeScope();
        }

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {

        }

        public ApplicationViewModel ApplicationViewModel
        {
            get
            {
                return _scope.Resolve<ApplicationViewModel>();
            }
        }

        public StartViewModel StartViewModel
        {
            get
            {
                return _scope.Resolve<StartViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}