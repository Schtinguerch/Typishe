using System;
using System.Collections.Generic;

namespace Typishe.Input
{
    public class KeyboardLayout
    {
        public string Name { get; set; }
        public string Author { get; set; }

        public List<string> Contacts { get; set; }
        public string[,] Keys { get; set; } //Should be 61*4 keys
    }
}
