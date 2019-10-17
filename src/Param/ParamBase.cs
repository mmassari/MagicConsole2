using System;
using System.Collections.Generic;

namespace MagicConsole
{
    public abstract class ParamBase
    {
        public string Title { get; set; }
        public List<string> AliasList { get; set; }
        public string Description { get; set; }
        public ParamBase()
        {
            Title = string.Empty;
            Description = string.Empty;
            AliasList = new List<string>();
        }
        public ParamBase(string title, string description, params string[] alias) : this()
        {
            Title = title;
            Description = description;
            AliasList = new List<string>(alias);
        }

    }
}