using System;


namespace Garama.Domain.Entities
{
    public class ToDoItem
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public bool Complete { get; set; }

    }
}

