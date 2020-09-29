using System;
using Avalonia;
using Avalonia.Dialogs;
using Avalonia.ReactiveUI;

namespace StorageSimulator
{
    class Program
    {
        public static AppBuilder BuildAvaloniaApp() => 
            AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .With(new X11PlatformOptions
                {
                    EnableMultiTouch = true,
                    UseDBusMenu = true
                })
                .With(new Win32PlatformOptions
                {
                    EnableMultitouch = true,
                    AllowEglInitialization = true
                })
                .UseSkia()
                .UseReactiveUI()
                .UseManagedSystemDialogs();

        static int Main(string[] args)
        {
            var builder = BuildAvaloniaApp();
            return builder.StartWithClassicDesktopLifetime(args);
        }
    }
}