using SqlSugar;
using System;

namespace GreenHat.Entitys
{
    internal class Cloud
    {
        private string icon;
        private string name;
        private string path;
        private string type;
        private int score;
        private string tag;
        private string md5;
        private string sha256;
        private string time;
        private string createTime;
        private int fileSize;

        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }
        public string Icon { get => icon; set => icon = value; }
        public string Name { get => name; set => name = value; }
        public string Path { get => path; set => path = value; }
        public string Type { get => type; set => type = value; }
        public int Score { get => score; set => score = value; }
        public string Tag { get => tag; set => tag = value; }
        public string Md5 { get => md5; set => md5 = value; }
        public string Sha256 { get => sha256; set => sha256 = value; }
        public DateTime Time { get => DateTime.Parse(time); set => time = value.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffffff'Z'"); }
        public DateTime CreateTime { get => DateTime.Parse(createTime); set => createTime = value.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffffff'Z'"); }
        public int FileSize { get => fileSize; set => fileSize = value; }
    }
}
