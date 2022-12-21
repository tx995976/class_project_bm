﻿using book_manager.Models;
using book_manager.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;


namespace book_manager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        // The.NET Generic Host provides dependency injection, configuration, logging, and other services.
        // https://docs.microsoft.com/dotnet/core/extensions/generic-host
        // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
        // https://docs.microsoft.com/dotnet/core/extensions/configuration
        // https://docs.microsoft.com/dotnet/core/extensions/logging
        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(c => { c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)); })
            .ConfigureServices((context, services) =>
            {
                // App Host
                services.AddHostedService<ApplicationHostService>();
                
                // Page resolver service
                services.AddSingleton<IPageService, PageService>();

                // Theme manipulation
                services.AddSingleton<IThemeService, ThemeService>();

                // TaskBar manipulation
                services.AddSingleton<ITaskBarService, TaskBarService>();

                // Service containing navigation, same as INavigationWindow... but without window
                services.AddSingleton<INavigationService, NavigationService>();

                //
                services.AddSingleton<UserService>();
                services.AddSingleton<BookService>();
                services.AddSingleton<UserInfoService>();

                // Main window with navigation
                services.AddScoped<INavigationWindow, Views.Windows.MainWindow>();
                services.AddScoped<ViewModels.MainWindowViewModel>();

                //other windows
                services.AddScoped<Views.Windows.UserLoginWindow>();
                services.AddScoped<ViewModels.UserLoginViewModel>();
                services.AddScoped<Views.Windows.BookdetailWindow>();
                services.AddScoped<ViewModels.BookdetailViewModel>();
                services.AddScoped<Views.Windows.ItemAddWindow>();
                services.AddScoped<ViewModels.ItemAddViewModel>();
                services.AddScoped<Views.Windows.TimeSelectWindow>();
                services.AddScoped<ViewModels.TimeSelectViewModel>();

                // Views and ViewModels
                services.AddScoped<Views.Pages.DashboardPage>();
                services.AddScoped<ViewModels.DashboardViewModel>();
                services.AddScoped<Views.Pages.DataPage>();
                services.AddScoped<ViewModels.DataViewModel>();
                services.AddScoped<Views.Pages.SettingsPage>();
                services.AddScoped<ViewModels.SettingsViewModel>();
                services.AddScoped<Views.Pages.BookViewPage>();
                services.AddScoped<ViewModels.BookViewViewModel>();
                services.AddScoped<Views.Pages.ItemPage>();
                services.AddScoped<ViewModels.ItemViewModel>();
                services.AddScoped<Views.Pages.ConfimPage>();
                services.AddScoped<ViewModels.ConfimViewModel>();
                services.AddScoped<Views.Pages.BorrowInfoPage>();
                services.AddScoped<ViewModels.BorrowInfoViewModel>();
                services.AddScoped<Views.Pages.UserPage>(); 
                services.AddScoped<ViewModels.UserViewModel>();


                // Configuration
                services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));
            }).Build();

        /// <summary>
        /// Gets registered service.
        /// </summary>
        /// <typeparam name="T">Type of the service to get.</typeparam>
        /// <returns>Instance of the service or <see langword="null"/>.</returns>
        public static T GetService<T>()
            where T : class
        {
            return _host.Services.GetService(typeof(T)) as T;
        }

        /// <summary>
        /// Occurs when the application is loading.
        /// </summary>
        private async void OnStartup(object sender, StartupEventArgs e)
        {
            await _host.StartAsync();
        }

        /// <summary>
        /// Occurs when the application is closing.
        /// </summary>
        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();

            _host.Dispose();
        }

        /// <summary>
        /// Occurs when an exception is thrown by an application but not handled.
        /// </summary>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
        }
    }
}