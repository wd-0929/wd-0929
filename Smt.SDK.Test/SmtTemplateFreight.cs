using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Smt.SDK.Test
{
    public class SmtTemplateFreight
    {
        private string filePath = "D:\\自己的项目\\wd-0929\\Smt.SDK.Test\\Excel";
        private SmtTemplateFreight() 
        {
             DirectoryInfo root = new DirectoryInfo(filePath);
             FileInfo files = root.GetFiles().FirstOrDefault();
        }
        public static SmtTemplateFreight Create()
        {
            return new SmtTemplateFreight();
        }
    }
}
