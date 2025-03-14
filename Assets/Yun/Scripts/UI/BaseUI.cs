using Yun.Scripts.Core;
using Yun.Scripts.Cores;

namespace Yun.Scripts.UI
{
    public abstract class BaseUI : YunBehaviour
    {
        public abstract void Show();
        public abstract void Hide();

        public UIName.Name UIName { get; set; }
    }
}
