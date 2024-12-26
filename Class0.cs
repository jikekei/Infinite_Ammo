using Exiled.Events.EventArgs.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using System.Collections;
using PluginAPI.Enums;
using Exiled.Events.Commands.PluginManager;
using PluginAPI.Events;
using MEC;
using CommandSystem.Commands.RemoteAdmin.ServerEvent;
using PlayerRoles;
using Exiled.API.Interfaces;
using System.ComponentModel;

namespace Class0
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
    }

    public class Main : Plugin<Config>
    {
        public FileName0 FileName1;

        public override string Name => "无限子弹";
        public override string Author => "疑什么名字？[3037240065]";
        public override Version Version => new Version(1, 0, 3);
        public override string Prefix => "无限子弹";

        public override void OnEnabled()
        {
            FileName1 = new FileName0();
            Exiled.Events.Handlers.Server.RoundStarted += FileName1.游戏开始;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= FileName1.游戏开始;
            FileName1 = null;
            base.OnDisabled();
        }
    }
    public class FileName0
    {
        public static void GiveAllAmmo(Player player)
        {
            if (player == null || player.IsCuffed == true)
                return;
            player.SetAmmo(Exiled.API.Enums.AmmoType.Ammo12Gauge, 14);
            player.SetAmmo(Exiled.API.Enums.AmmoType.Nato762, 100);
            player.SetAmmo(Exiled.API.Enums.AmmoType.Nato556, 100);
            player.SetAmmo(Exiled.API.Enums.AmmoType.Ammo44Cal, 100);
            player.SetAmmo(Exiled.API.Enums.AmmoType.Nato9, 100);
        }
        public void 游戏开始()
        {
            Timing.RunCoroutine(CleanFloorPlugin());
        }
        private static IEnumerator<float> CleanFloorPlugin()
        {
            yield return Timing.WaitForSeconds(3);
            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(3);
                var scpTeammates0 = Player.List.Where(p => (p.Role.Team != Team.SCPs && p.Role.Team != Team.Dead)).ToList();
                foreach (Player player in scpTeammates0)
                {
                    GiveAllAmmo(player);
                }
            }
        }

    }
}
