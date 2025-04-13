using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenHat.Utils
{
    internal static class InterceptQueue
    {
        private static Queue<InterceptForm> queue = new Queue<InterceptForm>();
        private static HashSet<string> set = new HashSet<string>();
        private static InterceptForm curForm;

        public static void Add(InterceptForm interceptForm)
        {
            if (!set.Add(interceptForm.GetPath())) return;
            if (curForm == null)
            {
                curForm = interceptForm;
                Task.Run(() => interceptForm.ShowDialog());
            }
            else queue.Enqueue(interceptForm);
        }

        public static void Next()
        {
            if (curForm != null) set.Remove(curForm.GetPath());
            InterceptForm interceptForm = queue.Count > 0 ? queue.Dequeue() : null;
            curForm = interceptForm;
            if (interceptForm == null) return;
            set.Remove(interceptForm.GetPath());
            Task.Run(() => interceptForm.ShowDialog());
        }

        public static void Clear()
        {
            set.Clear();
        }
    }
}
