using PCLStorage;
using SurveyMonkey_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Surveys_Manager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : MasterDetailPage
    {
        private IFolder folder;

        private List<MasterPageItem> menuList { get; set; }
        public Home()
        {
            InitializeComponent();

            menuList = new List<MasterPageItem>();

            var page1 = new MasterPageItem() { Title = Lang.Resource.titlemaster1, Icon = "home.png", TargetType = typeof(Surveys) };
            var page2 = new MasterPageItem() { Title = Lang.Resource.titlemaster2, Icon = "user.png", TargetType = typeof(Account) };
            var page3 = new MasterPageItem() { Title = Lang.Resource.titlemaster3, Icon = "group.png", TargetType = typeof(Aboutus) };
            var page4 = new MasterPageItem() { Title = Lang.Resource.titlemaster4, Icon = "up.png", TargetType = typeof(SharedSurveys) };
            var page5 = new MasterPageItem() { Title = Lang.Resource.titlemaster5, Icon = "down.png", TargetType = typeof(ReceivingPage) };
            var page6 = new MasterPageItem() { Title = Lang.Resource.titlemaster6, Icon = "lock.png", TargetType = typeof(Login) };

            menuList.Add(page1);
            menuList.Add(page4);
            menuList.Add(page5);
            menuList.Add(page2);
            menuList.Add(page3);
            menuList.Add(page6);

            navigationDrawerList.ItemsSource = menuList;

            this.BindingContext = new
            {
                Header = "",
                Image = "back_List.png",
                Footer = Lang.Resource.footer
            };

            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Surveys)));
            
        }
        protected override bool OnBackButtonPressed()
        {
            string survey = Detail.Navigation.NavigationStack[0].ToString().Substring(Detail.Navigation.NavigationStack[0].ToString().Length - 7);
            if (IsPresented)
            {
                IsPresented = false;
                return true;
            }
            else if(Detail.Navigation.NavigationStack.Count == 1 && !Detail.Navigation.NavigationStack[0].ToString().Contains("Login") && !Detail.Navigation.NavigationStack[0].ToString().Contains(".Surveys"))
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Surveys)));
                return true;
            }
            else if (Detail.Navigation.NavigationStack[Detail.Navigation.NavigationStack.Count-1].ToString().Contains("Login"))
            {
                Detail = new ClosePage();
                var closer = DependencyService.Get<ICloseApplication>();
                closer?.closeApplication();
                return false;
            }
            else if (Detail.Navigation.NavigationStack.Count > 1) {
                base.OnBackButtonPressed();
                return true;
            }
            else if (Detail.Navigation.NavigationStack.Count == 1 && survey.Contains("Surveys"))
            {
                if(base.OnBackButtonPressed())
                     return true;
                else { 
                     Detail = new ClosePage();
                     return false;
                }
            }
            Detail = new ClosePage();
            return false;
        }
        private async void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            folder = FileSystem.Current.LocalStorage;
            ExistenceCheckResult Resultfolder = await folder.CheckExistsAsync("folderUser");

            if (Resultfolder != ExistenceCheckResult.FolderExists)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Surveys)));
                IsPresented = false;
                return;
            }
            var item = (MasterPageItem)e.SelectedItem;
            if (item.Title.Equals("تسجيل الخروج") || item.Title.Equals("Sign out"))
            {
                IsPresented = false;
                var Res = await DisplayAlert(Lang.Resource.titlemaster6,Lang.Resource.dangerSignOut, Lang.Resource.btnOkMessage, Lang.Resource.btnCancelMessage);
                if (!Res)
                    return;

                // Delete Folder (File Account)
                folder = FileSystem.Current.LocalStorage;
                Resultfolder = await folder.CheckExistsAsync("folderUser");
                if (Resultfolder == ExistenceCheckResult.FolderExists)
                {
                    try
                    {
                        folder = await folder.GetFolderAsync("folderUser");
                        await folder.DeleteAsync();
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Surveys)));
                    }
                    catch (Exception ex)
                    {
                        DependencyService.Get<SnackBar>().ShowSnackBar(ex.ToString());
                    }
                }
                // Delete Folder (File Surveys)
                folder = FileSystem.Current.LocalStorage;
                Resultfolder = await folder.CheckExistsAsync("foldersurveys");
                if (Resultfolder == ExistenceCheckResult.FolderExists)
                {
                    try
                    {
                        folder = await folder.GetFolderAsync("foldersurveys");
                        await folder.DeleteAsync();
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Surveys)));
                    }
                    catch (Exception ex)
                    {
                        DependencyService.Get<SnackBar>().ShowSnackBar(ex.ToString());
                    }
                }
                // Delete Folder (file Received Surveys)
                folder = FileSystem.Current.LocalStorage;
                Resultfolder = await folder.CheckExistsAsync("Receivedsurveys");
                if (Resultfolder == ExistenceCheckResult.FolderExists)
                {
                    try
                    {
                        folder = await folder.GetFolderAsync("Receivedsurveys");
                        await folder.DeleteAsync();
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Surveys)));
                    }
                    catch (Exception ex)
                    {
                        DependencyService.Get<SnackBar>().ShowSnackBar(ex.ToString());
                    }
                }
                // Delete Folder (file Shared Surveys)
                folder = FileSystem.Current.LocalStorage;
                Resultfolder = await folder.CheckExistsAsync("SharedSurveys");
                if (Resultfolder == ExistenceCheckResult.FolderExists)
                {
                    try
                    {
                        folder = await folder.GetFolderAsync("SharedSurveys");
                        await folder.DeleteAsync();
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Surveys)));
                    }
                    catch (Exception ex)
                    {
                        DependencyService.Get<SnackBar>().ShowSnackBar(ex.ToString());
                    }
                }
                return;
            }
            Type page = item.TargetType;
            Detail = new NavigationPage((Page)Activator.CreateInstance(page));
            IsPresented = false;
        }

    }
}
