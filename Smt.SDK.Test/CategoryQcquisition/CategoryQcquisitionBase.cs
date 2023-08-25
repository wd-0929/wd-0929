using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebLoginBLL;

namespace Smt.SDK.Test.CategoryQcquisition
{
    public class CategoryQcquisitionBase
    {
        public MainLoginSetting Setting { get; set; }
        public WebClient webClient { get; set; }
        public Dictionary<long, string> CategoryDatas { get; set; }
    }
}
