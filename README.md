# KeycardInventoryBypass

作者：**Yiming**。SCP: Secret Laboratory 练枪服无限备用弹药插件，提供独立的 **EXILED** 和 **LabAPI** 版本。仓库沿用 `Infinite_Ammo` 名称，插件名称按内部要求统一为 `KeycardInventoryBypass`。

2.0.0 重构以内部项目 `幻梦银河练枪服插件/Class1.cs`、`Class2.cs` 为功能基准，替换仓库原来的定时补弹代码。内部源目录不作修改。

## 功能

- 切换手持物品、开始换弹时，将五种备用弹药设置为配置数量。
- 默认 12 号霰弹为 **14**，7.62、5.56、.44、9mm 均为 **101**。
- SCP、死亡/旁观角色和被铐玩家不获得补弹。
- 死亡前、被铐前清空备用弹药，禁止主动丢弃弹药。
- 已被其他插件取消的切换物品、换弹、死亡和铐人事件不会执行补弹/清空。
- 插件卸载时注销所有事件，避免重复回调。

“无限子弹”通过事件补充**备用弹药**实现；玩家仍需换弹，弹匣仍正常消耗。不定时补弹，不修改枪械射击机制。两版保留内部实现的触发时机，不新增出生或解铐补弹。已存在的备用弹药会被设置为配置数量，可能升高或降低。

## 两个版本

| 版本 | 编译依赖 | 输出文件 |
| --- | --- | --- |
| EXILED | EXILED 9.5.0 及匹配的游戏依赖（内部原项目版本） | `src/Exiled/bin/Release/net48/KeycardInventoryBypass.Exiled.dll` |
| LabAPI | 本机专服的 LabAPI 1.1.7 及其配套游戏 Managed 文件 | `src/LabApi/bin/Release/net48/KeycardInventoryBypass.LabApi.dll` |

两版均为 .NET Framework 4.8。构建通过证明 API/类型匹配，游戏更新后应使用对应服务端依赖重新编译并在测试服验证。

## 安装

1. 停服并移除旧的无限子弹 DLL。
2. 按服务器使用的框架选择**其中一版**，不要同时加载两版或旧版。
3. EXILED 版放入服务器的 `EXILED/Plugins`，配置前缀为 `keycard_inventory_bypass`。原 `无限子弹` 配置块不会自动迁移。
4. LabAPI 版放入服务器的 `LabAPI/plugins/<端口>`（或全局 `LabAPI/plugins/global`），由 LabAPI 自动生成插件配置。
5. 重启服务器，根据生成的配置文件调整数量。

配置项（YAML 字段格式以框架生成的文件为准）：

| 属性 | 默认值 | 说明 |
| --- | --- | --- |
| `IsEnabled` | `true` | 启用插件 |
| `Debug` | `false` | 输出启动调试日志 |
| `Ammo12Gauge` | `14` | 12 号霰弹 |
| `Nato762` | `101` | 7.62mm |
| `Nato556` | `101` | 5.56mm |
| `Ammo44Cal` | `101` | .44 |
| `Nato9` | `101` | 9mm |

数量属性使用 `ushort`。修改配置后重启服务器，或使用框架的插件重新加载功能。

## 构建

需要 .NET SDK 和 .NET Framework 4.8 Developer Pack。游戏、框架 DLL 由本机提供，不提交到仓库，也不复制到插件输出目录。

在此内部工作区内，EXILED 默认使用上级目录的 `packages/ExMod.Exiled.9.5.0/lib/net48` 与 `Server_NapCha_ui_API/bin/Debug`。LabAPI 默认使用 Steam 专服安装目录。其他电脑显式指定路径：

```powershell
dotnet build src/Exiled/KeycardInventoryBypass.Exiled.csproj -c Release -p:ExiledReferencesDir="D:\References\Exiled-9.5.0" -p:ExiledGameManagedDir="D:\References\Game-For-Exiled"
dotnet build src/LabApi/KeycardInventoryBypass.LabApi.csproj -c Release -p:LabApiGameManagedDir="D:\SCPServer\SCPSL_Data\Managed"
```

`ExiledReferencesDir` 需包含 `Exiled.API.dll`、`Exiled.Events.dll`、`Assembly-CSharp-Publicized.dll`；游戏依赖需包含匹配的 `UnityEngine.CoreModule.dll`、`Mirror.dll` 等依赖。LabAPI 的 Managed 目录需包含 `LabApi.dll`、`Assembly-CSharp.dll` 及配套游戏依赖。若编译器服务遇到输出权限问题，可加 `-p:UseSharedCompilation=false`。

整个解决方案：

```powershell
dotnet build KeycardInventoryBypass.sln -c Release
```

## 验证

共享逻辑测试不需要启动游戏，使用 .NET 10 SDK：

```powershell
dotnet run --project tests/KeycardInventoryBypass.Tests.csproj -c Release
```

覆盖默认数量、换弹后的恢复、不符合条件时不补弹、死亡/被铐清空、配置数量、禁用状态和空玩家。

测试服检查：

1. 人类玩家切换物品、连续换弹，检查备用弹药与弹匣正常工作。
2. SCP 和旁观者不补弹；被铐后备用弹药归零，切换物品不能恢复。
3. 死亡时清空备用弹药；主动丢弃弹药被拒绝。
4. 禁用插件后行为恢复原版；卸载/重新启用后无重复回调。
5. 与会取消死亡/铐人事件的插件一起运行时核对事件顺序。两个版本均采用前置事件，后执行的其他插件仍可能取消已经处理过的事件。

## 结构

```text
src/Shared/   共享配置和弹药规则，直接编译进两份 DLL
src/Exiled/   EXILED 入口、事件和玩家适配
src/LabApi/   LabAPI 入口、事件和玩家适配
tests/       不依赖游戏的共享逻辑测试
```

移除了内部原代码中的空 Verified/RespawningTeam 回调、废弃注释及 HintServiceMeow、PluginAPI 等无关依赖。

## 许可

沿用仓库的 [GPL-3.0 许可证](LICENSE)。LabAPI API 参考：[官方源码](https://github.com/northwood-studios/LabAPI)。
