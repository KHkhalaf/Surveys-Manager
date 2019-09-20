using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using SurveyMonkey_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Surveys_Manager.Models
{
    public class CheckNetwork
    {
        public static bool Check_Connectivity()
        {
            List<ConnectionType> ConnectionTypes = CrossConnectivity.Current.ConnectionTypes.ToList();

            if (ConnectionTypes != null || ConnectionTypes.Count != 0)
            {
                if (ConnectionTypes.Contains(ConnectionType.WiFi))
                {
                    return true;
                }
                else
                {
                    var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                    player.Load("NotificationUnconnected.mp3");
                    player.Play();

                    DependencyService.Get<SnackBar>().ShowSnackBar(Lang.Resource.notConnect);
                    return false;
                }
            }
            else
            {
                var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                player.Load("NotificationUnconnected.mp3");
                player.Play();

                DependencyService.Get<SnackBar>().ShowSnackBar(Lang.Resource.notConnect);
                return false;
            }
        }
    }
}
