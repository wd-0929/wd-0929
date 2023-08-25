using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectLoginBLL
{
    public class MainLoginSetting
    {
        /// <summary>
        /// 访问地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 正常host
        /// </summary>
        public string OriginalString { get; set; }
        public string OriginalAbsolutePath { get; set; }
        /// <summary>
        /// 登录站点的host
        /// </summary>
        public string LoginHost { get; set; }
        public string LoginAbsolutePath { get; set; }
        /// <summary>
        /// 登录界面的登录id
        /// </summary>
        public string LoginId { get; set; }
        /// <summary>
        /// 登录界面的登录密码
        /// </summary>
        public string LoginPassword { get; set; }
        /// <summary>
        /// 登录的账户
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 登录的密码
        /// </summary>
        public string AccountPassword { get; set; }
    }
}
