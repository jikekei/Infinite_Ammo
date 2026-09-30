using LabApi.Features.Wrappers;
using PlayerRoles;

namespace KeycardInventoryBypass.LabApi
{
    internal sealed class AmmoPlayer : IAmmoPlayer
    {
        private readonly Player player;

        public AmmoPlayer(Player player) => this.player = player;

        public bool CanReceiveAmmo => player != null && player.ReferenceHub != null &&
            !player.IsDisarmed && player.Team != Team.SCPs && player.Team != Team.Dead;

        public void SetAmmo(AmmoKind kind, ushort amount)
        {
            ItemType type;
            switch (kind)
            {
                case AmmoKind.Ammo12Gauge: type = ItemType.Ammo12gauge; break;
                case AmmoKind.Nato762: type = ItemType.Ammo762x39; break;
                case AmmoKind.Nato556: type = ItemType.Ammo556x45; break;
                case AmmoKind.Ammo44Cal: type = ItemType.Ammo44cal; break;
                case AmmoKind.Nato9: type = ItemType.Ammo9x19; break;
                default: throw new System.ArgumentOutOfRangeException(nameof(kind));
            }
            player.SetAmmo(type, amount);
        }

        public void ClearAmmo()
        {
            if (player != null && player.ReferenceHub != null)
                player.ClearAmmo();
        }
    }
}
