using System.ComponentModel;

namespace Infinite_Ammo
{
    public class AmmoSettings
    {
        [Description("是否启用无限备用弹药。")]
        public bool IsEnabled { get; set; } = true;

        [Description("是否启用调试日志。")]
        public bool Debug { get; set; } = false;

        [Description("切换物品或换弹时设置的 12 号霰弹备用数量。")]
        public ushort Ammo12Gauge { get; set; } = 14;

        public ushort Nato762 { get; set; } = 101;
        public ushort Nato556 { get; set; } = 101;
        public ushort Ammo44Cal { get; set; } = 101;
        public ushort Nato9 { get; set; } = 101;
    }
}
