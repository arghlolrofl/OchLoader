using System.Windows;
using OchLoader.ViewModel.Main;

namespace OchLoader.View.Main
{
  /// <summary>
  /// Interaction logic for ApplicationWindow.xaml
  /// </summary>
  public partial class ApplicationWindow : Window
  {
    public ApplicationWindow(ApplicationViewModel viewModel)
    {
      InitializeComponent();
      DataContext = viewModel;
    }
  }
}
