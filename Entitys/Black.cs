using SqlSugar;
using System;

namespace GreenHat.Entitys
{
    internal class Black
    {
        private string path;
        private string type;
        private string time;

        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }
        public string Path { get => path; set => path = value; }
        public string Type { get => type; set => type = value; }
        public DateTime Time { get => DateTime.Parse(time); set => time = value.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffffff'Z'"); }
    }
}
