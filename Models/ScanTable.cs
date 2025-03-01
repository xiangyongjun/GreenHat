using AntdUI;

namespace GreenHat.Models
{
    public class ScanTable : NotifyProperty
    {
        private bool selected = false;
        private string path;
        private string engine;
        private string type;
        private CellLink detail;
        private CellTag state;

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

        public string Engine
        {
            get { return engine; }
            set
            {
                if (engine == value) return;
                engine = value;
                OnPropertyChanged(nameof(Engine));
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

        public CellLink Detail
        {
            get { return detail; }
            set
            {
                if (detail == value) return;
                detail = value;
                OnPropertyChanged(nameof(Detail));
            }
        }

        public CellTag State
        {
            get { return state; }
            set
            {
                if (state == value) return;
                state = value;
                OnPropertyChanged(nameof(State));
            }
        }
    }
}
