
using System.Configuration;

namespace WheelTest.DAL.Common
{
    public abstract class BaseDal
    {
        protected readonly DapperUtil _dapperUtil;
        protected readonly UserInfo _userInfo;
        protected readonly string _dbConnectionString;

        public BaseDal(UserInfo user)
        {
            _userInfo = user;
            _dbConnectionString = ConfigurationManager.ConnectionStrings["SqlServerDb"].ConnectionString;
            _dapperUtil = new DapperUtil(_dbConnectionString);
        }
    }
}
