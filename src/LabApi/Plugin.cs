using System;
using LabApi.Features.Console;

namespace Infinite_Ammo.LabApi
{
    public sealed class Plugin : global::LabApi.Loader.Features.Plugins.Plugin<Config>
    {
        private EventHandlers handlers;

        public override string Name => "Infinite_Ammo";
        public override string Description => "无限子弹";
        public override string Author => "Yiming";
        public override Version Version => new Version(2, 0, 1);
        public override Version RequiredApiVersion => new Version(1, 1, 7);

        public override void Enable()
        {
            if (handlers != null || Config == null || !Config.IsEnabled)
                return;

            handlers = new EventHandlers(Config);
            handlers.Subscribe();
            Logger.Info("Infinite_Ammo LabAPI 版已启用。");
            if (Config.Debug)
                Logger.Debug("使用切换物品 / 换弹事件补充备用弹药。");
        }

        public override void Disable()
        {
            handlers?.Unsubscribe();
            handlers = null;
        }
    }
}
