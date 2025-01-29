using AntdUI;

namespace GreenHat.Models
{
    public class BlackTable : NotifyProperty
    {
        private bool selected = false;
        private int id;
        private string path;
        private string type;
        private string time;

        public bool Selected
        {
            get { return selected; }
            set
            {
                if (selected == value) return;
                selected = value;
                OnPropertyChanged(nameof(Selected));
            }
        }

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

        public string Path
        {
            get { return path; }
            set
            {
                if (path == value) return;
                path = value;
                OnPropertyChanged(nameof(Path));
            }
        }

        public string Type
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
