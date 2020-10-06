using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace StorageSimulator.Views
{
    public class LogListView: UserControl
    {
        public LogListView()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}