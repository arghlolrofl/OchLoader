using System.Windows.Controls;
using OchLoader.ViewModel.Start;

namespace OchLoader.View.Start
{
  /// <summary>
  /// Interaction logic for StartView.xaml
  /// </summary>
  public partial class StartView : UserControl
  {
    public StartView(StartViewModel viewModel)
    {
      InitializeComponent();
      DataContext = viewModel;
    }
  }
}
