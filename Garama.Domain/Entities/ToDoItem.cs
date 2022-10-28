using Microsoft.AspNetCore.Datasync.EFCore;
using System;


namespace Garama.Domain.Entities
{
    public class ToDoItem : EntityTableData
    {
        public string Text { get; set; }
        public bool Complete { get; set; }

    }
}

