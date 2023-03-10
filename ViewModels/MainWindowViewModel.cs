using book_manager.Views.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.ObjectModel;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

using book_manager.Services;

namespace book_manager.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _applicationTitle = String.Empty;

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationItems = new();

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationFooter = new();

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new();

        public MainWindowViewModel(INavigationService navigationService)
        {
            if (!_isInitialized)
                InitializeViewModel();

            //binding callback
            App.GetService<UserService>().flush_user += flush_panel;
            App.GetService<BookService>();
            App.GetService<UserInfoService>();
        }

        #region book_attributes

        [ObservableProperty]
        private string status_log = "登录";

        ObservableCollection<INavigationControl> items_normal = new ObservableCollection<INavigationControl>
        {
            new NavigationItem()
                {
                    Content = "Home",
                    PageTag = "dashboard",
                    Icon = SymbolRegular.Home24,
                    PageType = typeof(Views.Pages.DashboardPage)
                },
            new NavigationItem()
                {
                    Content = "BookShelf",
                    PageTag = "book",
                    Icon = SymbolRegular.Book24,
                    PageType = typeof(Views.Pages.BookViewPage)
                },
            new NavigationItem()
                {
                    Content = "orders",
                    PageTag = "order",
                    Icon = SymbolRegular.Book24,
                    PageType = typeof(Views.Pages.BorrowInfoPage)
                }

        };

         ObservableCollection<INavigationControl> items_bookmg = new ObservableCollection<INavigationControl>
         {
            new NavigationItem()
                {
                    Content = "Home",
                    PageTag = "dashboard",
                    Icon = SymbolRegular.Home24,
                    PageType = typeof(Views.Pages.DashboardPage)
                },
            new NavigationItem()
                {
                    Content = "items",
                    PageTag = "item",
                    Icon = SymbolRegular.Book24,
                    PageType = typeof(Views.Pages.ItemPage)
                },
            new NavigationItem()
                {
                    Content = "confim",
                    PageTag = "confim",
                    Icon = SymbolRegular.Book24,
                    PageType = typeof(Views.Pages.ConfimPage)
                },
            
         };

         ObservableCollection<INavigationControl> items_usermg = new ObservableCollection<INavigationControl>
         {
            new NavigationItem()
                {
                    Content = "Home",
                    PageTag = "dashboard",
                    Icon = SymbolRegular.Home24,
                    PageType = typeof(Views.Pages.DashboardPage)
                },
            new NavigationItem()
                {
                    Content = "User",
                    PageTag = "user",
                    Icon = SymbolRegular.Person24,
                    PageType = typeof(Views.Pages.UserPage)
                }
         };


        #endregion

        #region view_generate

        private void InitializeViewModel()
        {
            ApplicationTitle = "book_manager";

            NavigationItems = new ObservableCollection<INavigationControl>
            {
                new NavigationItem()
                {
                    Content = "Home",
                    PageTag = "dashboard",
                    Icon = SymbolRegular.Home24,
                    PageType = typeof(Views.Pages.DashboardPage)
                },
                
            };

            NavigationFooter = new ObservableCollection<INavigationControl>
            {
                new NavigationItem()
                {
                    Content = "Settings",
                    PageTag = "settings",
                    Icon = SymbolRegular.Settings24,
                    PageType = typeof(Views.Pages.SettingsPage)
                }
            };

            TrayMenuItems = new ObservableCollection<MenuItem>
            {
                new MenuItem
                {
                    Header = "test",
                    Tag = "tray_home"
                }
            };

            _isInitialized = true;
            
        }

        #endregion

        #region login_method

        [RelayCommand]
        private void OnshowLoginWindow(){
            App.GetService<UserLoginWindow>().Show();
        }

        private void flush_panel(Models.User? user){
            Status_log = user?.Account ?? "登录";

            //flush_panel
            if(user != null)
                switch(user.accountType){
                    case Models.User.userType.normal:
                        NavigationItems = items_normal;
                        break;
                    case Models.User.userType.book_manager:
                        NavigationItems = items_bookmg;
                        break;
                    case Models.User.userType.system_manager:
                        NavigationItems = items_usermg;
                        break;
                }
            App.GetService<INavigationWindow>().Navigate(typeof(Views.Pages.DashboardPage));
        }

        #endregion

    }
}
