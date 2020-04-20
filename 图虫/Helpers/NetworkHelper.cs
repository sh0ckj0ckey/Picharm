using System;
using Windows.Networking.Connectivity;

namespace 图虫.Helpers
{
    public class NetworkHelper
    {
        public static bool NetworkAvailable = false;

        public static bool CheckNetwork()
        {
            ConnectionProfile profile = null;
            try
            {
                profile = NetworkInformation.GetInternetConnectionProfile();
            }
            catch { }
            if (profile == null)
            {
                //断网
            }
            else
            {
                NetworkAvailable = true;
            }
            return NetworkAvailable;
        }
    }
}
