using PCLStorage;
using SurveyMonkey_Project.Models;
using SurveyMonkey_Project.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Surveys_Manager.Models
{
    public class ControlFile
    {
        private IFile file;
        private IFolder folder;

        public IFolder folderProperty
        {
            get { return folder; }
            set { folder = value; }
        }

        public IFile fileProperty
        {
            get { return file; }
            set { file = value; }
        }

        public async void Create_Folder(string foldername)
        {
            folder = FileSystem.Current.LocalStorage;
            folder = await folder.CreateFolderAsync(foldername, CreationCollisionOption.ReplaceExisting);
        }

        public async void Create_File(string filename)
        {
            file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
        }

        public async Task<bool> IsFolderIsExistAsync(string folderName)
        {
            ExistenceCheckResult folderExist = await folder.CheckExistsAsync(folderName);
            if (folderExist == ExistenceCheckResult.FolderExists)
                return true;
            return false;
        }
        public async Task<bool> IsFileIsExistAsync(string fileName)
        {
            ExistenceCheckResult fileExist = await folder.CheckExistsAsync(fileName);
            if (fileExist == ExistenceCheckResult.FileExists)
                return true;
            return false;
        }

        public async void WriteinsideFile(string filename, string content = "")
        {
            await file.WriteAllTextAsync(content);
        }

        public async Task<string> readfromFile(string filename)
        {
            string content = "";
            file = await folder.GetFileAsync(filename);
            content = await file.ReadAllTextAsync();
            return content;
        }
        public async Task<string> GetUserName()
        {
            User register = null;
            try
            {
                folder = FileSystem.Current.LocalStorage;
                folder = await folder.CreateFolderAsync("folderUser", CreationCollisionOption.OpenIfExists);
                file = await folder.CreateFileAsync("fileUser", CreationCollisionOption.OpenIfExists);

                string AccountUser = await file.ReadAllTextAsync();
                register = Serializable_Account.deserialize(AccountUser).ElementAt(0);
            }
            catch(Exception ex)
            {
                DependencyService.Get<SnackBar>().ShowSnackBar("Error ... cann't access to device storage");
            }
            return register.UserName;
        }
        public async Task UploadResponsedSurveys()
        {
            folder = FileSystem.Current.LocalStorage;
            if (!CheckNetwork.Check_Connectivity())
                return;
            List<Survey> surveys = new List<Survey>();
            ExistenceCheckResult result = await folder.CheckExistsAsync("ShareResponsedSurveys");
            if (result == ExistenceCheckResult.FolderExists)
            {
                folder = await folder.CreateFolderAsync("ShareResponsedSurveys", CreationCollisionOption.OpenIfExists);
                file = await folder.CreateFileAsync("ShareResponsedSurveys", CreationCollisionOption.OpenIfExists);

                string Content = await file.ReadAllTextAsync();
                if(String.IsNullOrEmpty(Content) || String.IsNullOrWhiteSpace(Content))
                    return;
                surveys = Serializable_Survey.deserialize(Content).ToList();
                foreach (Survey S in surveys)
                {
                    SurveysServices S_S = new SurveysServices();
                    S_S.Set_UrlApi("ShareResponsedSurveys/");
                    await S_S.PostSurveysAsync(S);
                }
                file = await folder.GetFileAsync("ShareResponsedSurveys");
                await file.DeleteAsync();
            }
        }
    }
}
