using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Students_Site.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}