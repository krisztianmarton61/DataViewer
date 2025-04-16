using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataExplorerModels
{
    public class Album
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public User User { get; set; } = new User();
        public PhotosPage Photos { get; set; } = new PhotosPage();
    }
}


