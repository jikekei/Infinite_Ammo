using Exiled.Events.EventArgs.Player;
using PlayerEvents = Exiled.Events.Handlers.Player;

namespace KeycardInventoryBypass.Exiled
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
            PlayerEvents.Handcuffing += OnHandcuffing;
            PlayerEvents.DroppingAmmo += OnDroppingAmmo;
        }

        public void Unsubscribe()
        {
            PlayerEvents.ChangingItem -= OnChangingItem;
            PlayerEvents.ReloadingWeapon -= OnReloadingWeapon;
            PlayerEvents.Dying -= OnDying;
            PlayerEvents.Handcuffing -= OnHandcuffing;
            PlayerEvents.DroppingAmmo -= OnDroppingAmmo;
        }

        private void OnChangingItem(ChangingItemEventArgs ev)
        {
            if (ev.IsAllowed)
                service.Refill(new AmmoPlayer(ev.Player));
        }

        private void OnReloadingWeapon(ReloadingWeaponEventArgs ev)
        {
            if (ev.IsAllowed)
                service.Refill(new AmmoPlayer(ev.Player));
        }

        private void OnDying(DyingEventArgs ev)
        {
            if (ev.IsAllowed)
                service.Clear(new AmmoPlayer(ev.Player));
        }

        private void OnHandcuffing(HandcuffingEventArgs ev)
        {
            if (ev.IsAllowed)
                service.Clear(new AmmoPlayer(ev.Target));
        }

        private void OnDroppingAmmo(DroppingAmmoEventArgs ev)
        {
            if (service.IsEnabled)
                ev.IsAllowed = false;
        }
    }
}
