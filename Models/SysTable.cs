using AntdUI;

namespace GreenHat.Models
{
    public class SysTable : NotifyProperty
    {
        private string name;
        private string desc;

        public string Name
        {
            get { return name; }
            set
            {
                if (name == value) return;
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Desc
        {
            get { return desc; }
            set
            {
                if (desc == value) return;
                desc = value;
                OnPropertyChanged(nameof(Desc));
            }
        }
    }
}
