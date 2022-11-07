using System;
using System.Collections.Generic;
using System.Text;

namespace Garama
{
    public static class Constants
    {
        /// <summary>
        /// The base URI for the Datasync service.
        /// </summary>
        public static string ServiceUri = "https://demo-datasync-quickstart.azurewebsites.net";

        /// <summary>
        /// The application (client) ID for the native app within Azure Active Directory
        /// </summary>
        public static string ApplicationId = "38788bad-85c5-486f-af98-3a2848ec44df";

        /// <summary>
        /// The list of scopes to request
        /// </summary>
        public static string[] Scopes = new[]
        {
          "api://ec4ba805-3a95-4f65-aba9-ced43a39cad0/access_as_user"
        };

        public static IPlatform PlatformService { get; set; }
    }
}
