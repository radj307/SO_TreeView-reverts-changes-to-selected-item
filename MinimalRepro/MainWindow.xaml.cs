using System.Windows;

namespace MinimalRepro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            VM = new(this);

            InitializeComponent();

            VM.PostInitializeComponent();
        }

        public MainWindowVM VM { get; }
    }
}
