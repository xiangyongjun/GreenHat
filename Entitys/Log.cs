using SqlSugar;
using System;

namespace GreenHat.Entitys
{
    internal class Log
    {
        private string time;
        private string type;
        private string func;
        private string desc;
        private string detail;

        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }
        public DateTime Time { get => DateTime.Parse(time); set => time = value.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffffff'Z'"); }
        public string Type { get => type; set => type = value; }
        public string Func { get => func; set => func = value; }
        public string Desc { get => desc; set => desc = value; }
        public string Detail { get => detail; set => detail = value; }
    }
}
