using System.Windows;
using System.Windows.Input;

namespace Pathfinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CanvasHandler canvasHandler { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            canvasHandler = new CanvasHandler(MyCanvas);
        }

        private void MyCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            canvasHandler.ExecuteToolAction(e.GetPosition(this.MyCanvas));
        }

        private void ClearCanvasButton_Click(object sender, RoutedEventArgs e)
        {
            canvasHandler.Clear();
        }
    }
}