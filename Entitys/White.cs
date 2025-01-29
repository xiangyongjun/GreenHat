using SqlSugar;
using System;

namespace GreenHat.Entitys
{
    internal class White
    {
        private string path;
        private string time;

        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }
        public string Path { get => path; set => path = value; }
        public DateTime Time { get => DateTime.Parse(time); set => time = value.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffffff'Z'"); }
    }
}
