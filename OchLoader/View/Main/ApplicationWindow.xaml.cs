using System.Windows;
using OchLoader.ViewModel.Main;
using MahApps.Metro.Controls;

namespace OchLoader.View.Main
{
  /// <summary>
  /// Interaction logic for ApplicationWindow.xaml
  /// </summary>
  public partial class ApplicationWindow : MetroWindow
  {
    public ApplicationWindow(ApplicationViewModel viewModel)
    {
      InitializeComponent();
      DataContext = viewModel;
    }
  }
}
