using AntdUI;

namespace GreenHat.Models
{
    public class CloudTable : NotifyProperty
    {
        private int id;
        private CellImage icon;
        private string name;
        private CellTag type;
        private string time;

        public int Id
        {
            get { return id; }
            set
            {
                if (id == value) return;
                id = value;
                OnPropertyChanged(nameof(Id));
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

        public CellTag Type
        {
            get { return type; }
            set
            {
                if (type == value) return;
                type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public string Time
        {
            get { return time; }
            set
            {
                if (time == value) return;
                time = value;
                OnPropertyChanged(nameof(Time));
            }
        }
    }
}
