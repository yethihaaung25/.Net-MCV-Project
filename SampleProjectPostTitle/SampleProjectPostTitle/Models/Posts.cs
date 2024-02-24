using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleProjectPostTitle.Models
{
    public class Posts
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Location { get; set; }
        public int Price { get; set; }
        public int Rate { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
    }
}