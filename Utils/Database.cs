﻿using GreenHat.Entitys;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;

namespace GreenHat.Utils
{
    internal static class Database
    {
        private static SqlSugarClient db;

        static Database()
        {
            string databasePath = $"{AppDomain.CurrentDomain.BaseDirectory}Config.db";
            db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = $"Data Source={databasePath};Pooling=True;Version=3;",
                DbType = DbType.Sqlite,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,
                MoreSettings = new ConnMoreSettings()
                {
                    IsAutoRemoveDataCache = true,
                    DefaultCacheDurationInSeconds = 60
                }
            });
            if (!File.Exists(databasePath))
            {
                db.DbMaintenance.CreateDatabase(databasePath);
                db.CodeFirst.InitTables<Black>();
                db.CodeFirst.InitTables<White>();
                db.CodeFirst.InitTables<Log>();
                db.CodeFirst.InitTables<Cloud>();
            }
            else db.Open();
        }

        public static List<Log> GetLogList(string type)
        {
            if (type == "全部") type = "";
            return db.CopyNew().Queryable<Log>().Where(it => it.Type.Contains(type)).OrderBy(it => SqlFunc.Desc(it.Time)).ToList();
        }

        public static List<Log> GetLogList(string type, DateTime startDate, DateTime endDate)
        {
            if (type == "全部") type = "";
            return db.CopyNew().Queryable<Log>().Where(it => it.Type.Contains(type) && it.Time >= startDate && it.Time <= endDate).OrderBy(it => SqlFunc.Desc(it.Time)).ToList();
        }

        public static bool AddLog(string type, string func, string desc, string detail = "")
        {
            return db.CopyNew().Insertable(new Log()
            {
                Time = DateTime.Now,
                Type = type,
                Func = func,
                Desc = desc,
                Detail = detail
            }).ExecuteCommand() > 0;
        }

        public static int CountLog()
        {
            return db.CopyNew().Queryable<Log>().Count();
        }

        public static void ClearLog()
        {
            db.CopyNew().Deleteable<Log>().ExecuteCommand();
        }

        public static List<White> GetWhiteList(string path = "")
        {
            return db.CopyNew().Queryable<White>().Where(it => it.Path.Contains(path)).OrderBy(it => SqlFunc.Desc(it.Time)).ToList();
        }

        public static bool AddWhite(string path)
        {
            AddLog("其他", "添加信任", $"文件：{path}");
            return db.Insertable(new White()
            {
                Time = DateTime.Now,
                Path = path
            }).ExecuteCommand() > 0;
        }

        public static bool RemoveWhite(List<int> ids)
        {
            return db.CopyNew().Deleteable<White>().In(ids).ExecuteCommand() > 0;
        }

        public static bool IsWhite(string path)
        {
            return db.CopyNew().Queryable<White>().Where(it => path.StartsWith(it.Path)).Count() > 0;
        }

        public static List<Black> GetBlackList(string path = "")
        {
            return db.Queryable<Black>().Where(it => it.Path.Contains(path)).OrderBy(it => SqlFunc.Desc(it.Time)).ToList();
        }

        public static bool AddBlack(string path, string type)
        {
            try
            {
                int id = db.CopyNew().Insertable(new Black()
                {
                    Time = DateTime.Now,
                    Path = path,
                    Type = type
                }).ExecuteReturnIdentity();
                string dir = $"{AppDomain.CurrentDomain.BaseDirectory}backup\\";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                AddLog("病毒防护", "添加隔离", $"文件：{path}");
                Tools.EncryptFile(path, $"{dir}{id}.bak", "GreenHat12345678");
                File.Delete(path);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool RestoreBlack(List<int> ids)
        {
            List<Black> blackList = db.CopyNew().Queryable<Black>().In(ids).ToList();
            string dir = $"{AppDomain.CurrentDomain.BaseDirectory}backup\\";
            foreach (Black item in blackList)
            {
                Tools.DecryptFile($"{dir}{item.Id}.bak", item.Path, "GreenHat12345678");
                File.Delete($"{dir}{item.Id}.bak");
            }
            return db.CopyNew().Deleteable<Black>().In(ids).ExecuteCommand() > 0;
        }

        public static bool RemoveBlack(List<int> ids)
        {
            List<Black> blackList = db.Queryable<Black>().In(ids).ToList();
            string dir = $"{AppDomain.CurrentDomain.BaseDirectory}backup\\";
            foreach (Black item in blackList)
            {
                File.Delete($"{dir}{item.Id}.bak");
            }
            return db.CopyNew().Deleteable<Black>().In(ids).ExecuteCommand() > 0;
        }

        public static bool IsBlack(string path)
        {
            return db.CopyNew().Queryable<Black>().Where(it => it.Path == path).Count() > 0;
        }

        public static int CountBlack()
        {
            return db.CopyNew().Queryable<Black>().Count();
        }

        public static List<Cloud> GetCloudList()
        {
            return db.CopyNew().Queryable<Cloud>().OrderBy(it => SqlFunc.Desc(it.Time)).ToList();
        }

        public static bool AddCloud(Cloud cloud)
        {
            return db.CopyNew().Insertable(cloud).ExecuteCommand() > 0;
        }

        public static int CountCloud()
        {
            return db.CopyNew().Queryable<Cloud>().Count();
        }

        public static void ClearCloud()
        {
            db.CopyNew().Deleteable<Cloud>().ExecuteCommand();
        }
    }
}
