using System;
using Exiled.API.Features;

namespace KeycardInventoryBypass.Exiled
{
    public sealed class Plugin : Plugin<Config>
    {
        private EventHandlers handlers;

        public override string Name => "KeycardInventoryBypass";
        public override string Author => "Yming";
        public override string Prefix => "无限子弹";
        public override Version Version => new Version(2, 0, 0);
        public override Version RequiredExiledVersion => new Version(9, 5, 0);

        public override void OnEnabled()
        {
            if (handlers != null)
                return;

            handlers = new EventHandlers(Config);
            handlers.Subscribe();
            Log.Info("无限子弹 EXILED 版已启用。");
            if (Config.Debug)
                Log.Debug("使用切换物品 / 换弹事件补充备用弹药。");
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            handlers?.Unsubscribe();
            handlers = null;
            base.OnDisabled();
        }
    }
}
