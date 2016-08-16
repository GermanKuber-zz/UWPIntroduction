using System;
using System.Collections.Generic;

namespace ReadApp
{
    public class Notice
    {
        public List<string> Tags { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public int Id { get; set; }
        public string Date { get; set; }
        public string Image { get; set; }
    }
}