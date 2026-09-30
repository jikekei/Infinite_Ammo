namespace KeycardInventoryBypass
{
    internal enum AmmoKind
    {
        Ammo12Gauge,
        Nato762,
        Nato556,
        Ammo44Cal,
        Nato9,
    }

    internal interface IAmmoPlayer
    {
        bool CanReceiveAmmo { get; }
        void SetAmmo(AmmoKind kind, ushort amount);
        void ClearAmmo();
    }

    // Compiled into both plugins so neither DLL needs a separate shared dependency.
    internal sealed class AmmoService
    {
        private readonly AmmoSettings settings;

        public AmmoService(AmmoSettings settings)
        {
            this.settings = settings;
        }

        public bool IsEnabled => settings.IsEnabled;

        public void Refill(IAmmoPlayer player)
        {
            if (!IsEnabled || player == null || !player.CanReceiveAmmo)
                return;

            player.SetAmmo(AmmoKind.Ammo12Gauge, settings.Ammo12Gauge);
            player.SetAmmo(AmmoKind.Nato762, settings.Nato762);
            player.SetAmmo(AmmoKind.Nato556, settings.Nato556);
            player.SetAmmo(AmmoKind.Ammo44Cal, settings.Ammo44Cal);
            player.SetAmmo(AmmoKind.Nato9, settings.Nato9);
        }

        // Death and handcuffing must also clear ammo on players ineligible for refills.
        public void Clear(IAmmoPlayer player)
        {
            if (IsEnabled)
                player?.ClearAmmo();
        }
    }
}
