using AntdUI;

namespace GreenHat.Models
{
    public class LogTable : NotifyProperty
    {
        private string time;
        private string type;
        private string func;
        private string desc;

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

        public string Func
        {
            get { return func; }
            set
            {
                if (func == value) return;
                func = value;
                OnPropertyChanged(nameof(Func));
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