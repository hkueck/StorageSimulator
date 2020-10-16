using Avalonia;
using Avalonia.Markup.Xaml;
using Prism.DryIoc;
using Prism.Ioc;
using StorageSimulator.Core.Interfaces;
using StorageSimulator.Core.Model;
using StorageSimulator.Core.UseCases;
using StorageSimulator.Infrastructure;
using StorageSimulator.ViewModels;
using StorageSimulator.Views;

namespace StorageSimulator
{
    public class App : PrismApplication
    {
        public static AppBuilder BuildAvaloniaApp() =>
            AppBuilder
                .Configure<App>()
                .UsePlatformDetect();

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            base.Initialize();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IStorageSystem, StorageSystem>();
            containerRegistry.Register<IWatchRequestUseCase, WatchRequestUseCase>();
            containerRegistry.Register<IWatchRequestService, WatchRequestService>();
            containerRegistry.Register<ISendResponseUseCase, SendResponseUseCase>();
            containerRegistry.Register<IAnalyseRequestUseCase, AnalyseRequestUseCase>();
            containerRegistry.Register<IStorageSimulatorConfig, StorageSimulatorConfig>();
            containerRegistry.Register<IMovementRequestListViewModel, MovementRequestListViewModel>();
            containerRegistry.Register<IMovementRequestViewModel, MovementRequestViewModel>();
            containerRegistry.Register<ILogViewModel, LogViewModel>();
            containerRegistry.Register<ILogListViewModel, LogListViewModel>();
        }

        protected override IAvaloniaObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }
    }
}