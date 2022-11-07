using Microsoft.Datasync.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Garama.Models
{
    public class ToDoItem : DatasyncClientData
    {
        public string Text { get; set; }
        public bool Complete { get; set; }
    }
}
