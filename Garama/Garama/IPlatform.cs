using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Garama
{
    public interface IPlatform
    {
        IPublicClientApplication GetIdentityClient(string applicationId);
    }
}
