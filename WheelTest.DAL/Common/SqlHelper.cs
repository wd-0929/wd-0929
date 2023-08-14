using System.Data.SqlClient;

namespace WheelTest.DAL.Common
{
    public static class SqlHelper
    {
        public static T GetValue<T>(this SqlDataReader dataReader, string name, T defaultValue)
           where T : struct
        {
            int ordinal = dataReader.GetOrdinal(name);
            if (dataReader.IsDBNull(ordinal))
            {
                return defaultValue;
            }
            else
            {
                return (T)dataReader.GetValue(ordinal);
            }
        }

        public static T? GetValue<T>(this SqlDataReader dataReader, string name)
            where T : struct
        {
            int ordinal = dataReader.GetOrdinal(name);
            if (dataReader.IsDBNull(ordinal))
            {
                return null;
            }
            else
            {
                return (T)dataReader.GetValue(ordinal);
            }
        }

        public static string GetString(this SqlDataReader dataReader, string name)
        {
            int ordinal = dataReader.GetOrdinal(name);
            if (dataReader.IsDBNull(ordinal))
            {
                return null;
            }
            else
            {
                return dataReader.GetString(ordinal);
            }
        }
    }
}
