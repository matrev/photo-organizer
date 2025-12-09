using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using photo_organizer.ViewModels;
using photo_organizer.Views;

using Microsoft.Extensions.DependencyInjection;
using photo_organizer.Services;
using System;

namespace photo_organizer;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }
    

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            
            var services = new ServiceCollection();
            
            // Register MainWindow first so it can be injected into FolderService
            services.AddSingleton<MainWindow>();

            services.AddSingleton<IFolderService>(x => new FolderService(x.GetRequiredService<MainWindow>()));
            services.AddSingleton<IPhotoOrganizerService, PhotoOrganizerService>();
            

            var serviceProvider = services.BuildServiceProvider();

            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.DataContext = new MainWindowViewModel(
                serviceProvider.GetRequiredService<IPhotoOrganizerService>(),
                serviceProvider.GetRequiredService<IFolderService>());

            desktop.MainWindow = mainWindow;
            Services = serviceProvider;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
    
    public IServiceProvider? Services { get; private set; }
}