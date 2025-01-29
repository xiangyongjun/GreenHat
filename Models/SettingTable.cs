using AntdUI;

namespace GreenHat.Models
{
    public class SettingTable : NotifyProperty
    {
        private int id;
        private CellImage icon;
        private string name;
        private string desc;
        private bool enabled;

        public int Id
        {
            get { return id; }
            set
            {
                if (id == value) return;
                id = value;
            }
        }

        public CellImage Icon
        {
            get { return icon; }
            set
            {
                if (icon == value) return;
                icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }

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

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (enabled == value) return;
                enabled = value;
                OnPropertyChanged(nameof(Enabled));
            }
        }
    }
}
