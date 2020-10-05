using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore_webapi.ViewModels
{
    public class ResponseStatusViewModel
    {
        public bool Result { get; set; }
        public List<string> Messages { get; set; }

        public ResponseStatusViewModel()
        {
            Messages = new List<string>();
        }
    }
}
