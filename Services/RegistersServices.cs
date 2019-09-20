using Plugin.RestClient;
using SurveyMonkey_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SurveyMonkey_Project.Services
{
    public class RegistersServices
    {
        private const string UrlApi = "http://192.168.43.172/WebApi/api/Registers/";
        RestClient<User> restClient = new RestClient<User>();
        public RegistersServices()
        {
            restClient.WebServiceUrlProperty = UrlApi;
        }
        public void Set_UrlApi(string restUrl)
        {
            restClient.WebServiceUrlProperty += restUrl;
        }
        public async Task<List<User>> GetRegistersAsync()
        {
            try
            {
                var ListRegisters = await restClient.GetAsync();
                return ListRegisters;
            }
            catch(Exception ex)
            {
                DependencyService.Get<SnackBar>().ShowSnackBar(ex.ToString());
                return new List<User>();
            }
        }
        public async Task<bool> PostRegistersAsync(User register)
        {
            try
            {
                var Result = await restClient.PostAsync(register);
                return Result;
            }
            catch(Exception ex)
            {
                DependencyService.Get<SnackBar>().ShowSnackBar(ex.ToString());
                return false;
            }
        }
        public async Task<bool> PutRegistersAsync(User register)
        {
            try
            {
                var Result = await restClient.PutAsync(register);
                return Result;
            }
            catch(Exception ex)
            {
                DependencyService.Get<SnackBar>().ShowSnackBar(ex.ToString());
                return false;
            }
        }
        public async Task<bool> DeleteRegistersAsync()
        {
            var Result = await restClient.DeleteAsync();
            return Result;
        }
        public async Task<List<User>> GetRegistersBykeywordAsync(string keyword)
        {
            try
            {
                var ListRegisters = await restClient.GetBykeywordAsync(keyword);
                return ListRegisters;
            }
            catch(Exception ex)
            {
                DependencyService.Get<SnackBar>().ShowSnackBar(ex.ToString());
                return new List<User>();
            }
        }
    }
}
