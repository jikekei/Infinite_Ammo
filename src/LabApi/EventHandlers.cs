using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;

namespace KeycardInventoryBypass.LabApi
{
    internal sealed class EventHandlers
    {
        private readonly AmmoService service;

        public EventHandlers(Config config) => service = new AmmoService(config);

        public void Subscribe()
        {
            PlayerEvents.ChangingItem += OnChangingItem;
            PlayerEvents.ReloadingWeapon += OnReloadingWeapon;
            PlayerEvents.Dying += OnDying;
            PlayerEvents.Cuffing += OnCuffing;
            PlayerEvents.DroppingAmmo += OnDroppingAmmo;
        }

        public void Unsubscribe()
        {
            PlayerEvents.ChangingItem -= OnChangingItem;
            PlayerEvents.ReloadingWeapon -= OnReloadingWeapon;
            PlayerEvents.Dying -= OnDying;
            PlayerEvents.Cuffing -= OnCuffing;
            PlayerEvents.DroppingAmmo -= OnDroppingAmmo;
        }

        private void OnChangingItem(PlayerChangingItemEventArgs ev)
        {
            if (ev.IsAllowed)
                service.Refill(new AmmoPlayer(ev.Player));
        }

        private void OnReloadingWeapon(PlayerReloadingWeaponEventArgs ev)
        {
            if (ev.IsAllowed)
                service.Refill(new AmmoPlayer(ev.Player));
        }

        private void OnDying(PlayerDyingEventArgs ev)
        {
            if (ev.IsAllowed)
                service.Clear(new AmmoPlayer(ev.Player));
        }

        private void OnCuffing(PlayerCuffingEventArgs ev)
        {
            if (ev.IsAllowed)
                service.Clear(new AmmoPlayer(ev.Target));
        }

        private void OnDroppingAmmo(PlayerDroppingAmmoEventArgs ev)
        {
            if (service.IsEnabled)
                ev.IsAllowed = false;
        }
    }
}
