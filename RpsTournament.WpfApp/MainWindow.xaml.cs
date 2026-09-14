using RpsTournament.Core;
using System.Windows;

namespace RpsTournament.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            PlayerMoveComboBox.ItemsSource = Enum.GetValues<Move>();
        }

    }
}