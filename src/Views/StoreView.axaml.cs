using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace StorageSimulator.Views
{
    public class StoreView : UserControl
    {
        public StoreView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}