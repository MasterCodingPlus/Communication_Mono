using System;
using System.Collections.Generic;
using SqlSugar;

namespace DbTools
{
    public class SqlSugarClientPool
    {
        public static SqlSugarClient getSqlSugarClient(string conStr)
        {
            var da = new ConnectionConfig();
            da.ConnectionString = conStr; //MySQL连接字符串
            da.DbType = DbType.MySql;
            da.InitKeyType = InitKeyType.Attribute;
            da.IsAutoCloseConnection = true;
            // da.SlaveConnectionConfigs = new List<SlaveConnectionConfig>()
            // {
            //     new SlaveConnectionConfig()
            //     {
            //         HitRate = 10, ConnectionString = "server=121.196.223.184;Database=zituback;Uid=test;Pwd=test"
            //     },
            //     new SlaveConnectionConfig()
            //     {
            //         HitRate = 10, ConnectionString = "server=121.196.223.184;Database=zituback;Uid=test;Pwd=test"
            //     }
            // };
            var dd = new SqlSugarClient(da);
            return dd;
        }
    }
}