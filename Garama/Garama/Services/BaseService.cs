using System;
using System.Collections.Generic;
using System.Text;

namespace Garama.Services
{
    public class BaseService
    {

        public void LogError(Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
