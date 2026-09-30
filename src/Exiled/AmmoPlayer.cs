using Exiled.API.Enums;
using Exiled.API.Features;
using PlayerRoles;

namespace Infinite_Ammo.Exiled
{
    internal sealed class AmmoPlayer : IAmmoPlayer
    {
        private readonly Player player;

        public AmmoPlayer(Player player) => this.player = player;

        public bool CanReceiveAmmo => player != null && player.ReferenceHub != null &&
            player.Role != null && !player.IsCuffed &&
            player.Role.Team != Team.SCPs && player.Role.Team != Team.Dead;

        public void SetAmmo(AmmoKind kind, ushort amount)
        {
            AmmoType type;
            switch (kind)
            {
                case AmmoKind.Ammo12Gauge: type = AmmoType.Ammo12Gauge; break;
                case AmmoKind.Nato762: type = AmmoType.Nato762; break;
                case AmmoKind.Nato556: type = AmmoType.Nato556; break;
                case AmmoKind.Ammo44Cal: type = AmmoType.Ammo44Cal; break;
                case AmmoKind.Nato9: type = AmmoType.Nato9; break;
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
