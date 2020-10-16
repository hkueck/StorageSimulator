using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace StorageSimulator.Views
{
    public class StoreListView: UserControl
    {
        public StoreListView()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}