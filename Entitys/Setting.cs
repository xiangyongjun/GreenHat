using SqlSugar;

namespace GreenHat.Entitys
{
    internal class Setting
    {
        private string icon;
        private string name;
        private string desc;
        private bool enabled;

        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }
        public string Icon { get => icon; set => icon = value; }
        public string Name { get => name; set => name = value; }
        public string Desc { get => desc; set => desc = value; }
        public bool Enabled { get => enabled; set => enabled = value; }
    }
}
