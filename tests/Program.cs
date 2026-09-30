using System;
using System.Collections.Generic;
using KeycardInventoryBypass;

internal static class Program
{
    private static int Main()
    {
        try
        {
            var settings = new AmmoSettings();
            var service = new AmmoService(settings);
            var player = new FakePlayer();
            service.Refill(player);
            Assert(player.Ammo.Count == 5, "All five reserve ammo types are supplied.");
            Assert(player.Ammo[AmmoKind.Ammo12Gauge] == 14, "Internal shotgun default is preserved.");
            foreach (var kind in new[] { AmmoKind.Nato762, AmmoKind.Nato556, AmmoKind.Ammo44Cal, AmmoKind.Nato9 })
                Assert(player.Ammo[kind] == 101, "Internal rifle/pistol defaults are preserved.");

            // Simulate reserve ammo consumed by a reload, then another refill trigger.
            player.Ammo[AmmoKind.Nato556] = 60;
            service.Refill(player);
            Assert(player.Ammo[AmmoKind.Nato556] == 101, "Reload triggers restore the reserve.");

            player.CanReceiveAmmo = false;
            player.Ammo[AmmoKind.Nato556] = 60;
            service.Refill(player);
            Assert(player.Ammo[AmmoKind.Nato556] == 60, "Ineligible players receive no refill.");
            service.Clear(player);
            Assert(player.Ammo.Count == 0, "Handcuffed/dying players can still have ammo cleared.");

            player.CanReceiveAmmo = true;
            settings.Ammo12Gauge = 0;
            settings.Nato556 = 250;
            service.Refill(player);
            Assert(player.Ammo[AmmoKind.Ammo12Gauge] == 0 && player.Ammo[AmmoKind.Nato556] == 250,
                "Configured quantities, including zero, are respected.");

            settings.IsEnabled = false;
            Assert(!service.IsEnabled, "Disabled service does not block ammo drops.");
            player.Ammo[AmmoKind.Nato556] = 7;
            service.Refill(player);
            service.Clear(player);
            Assert(player.Ammo[AmmoKind.Nato556] == 7, "Disabled plugin leaves inventory intact.");

            settings.IsEnabled = true;
            service.Refill(null);
            service.Clear(null);
            Console.WriteLine("PASS: defaults, reload replenishment, eligibility, death/cuff clearing, custom quantities, disable, null player.");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }

    private static void Assert(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }

    private sealed class FakePlayer : IAmmoPlayer
    {
        public bool CanReceiveAmmo { get; set; } = true;
        public Dictionary<AmmoKind, ushort> Ammo { get; } = new Dictionary<AmmoKind, ushort>();
        public void SetAmmo(AmmoKind kind, ushort amount) => Ammo[kind] = amount;
        public void ClearAmmo() => Ammo.Clear();
    }
}
