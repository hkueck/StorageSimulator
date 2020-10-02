using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace StorageSimulator.Views
{
    public class MovementRequestView: UserControl
    {

        public MovementRequestView()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}