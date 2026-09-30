# 构建与验证

## 环境

- .NET SDK 与 .NET Framework 4.8 Developer Pack；共享逻辑测试使用 .NET 10 SDK。
- EXILED 版：EXILED 9.5.0 和匹配的游戏程序集。
- LabAPI 版：LabAPI 1.1.7 和配套专服 `SCPSL_Data/Managed` 文件。

游戏及框架 DLL 由本机提供，不提交到仓库，也不会复制到插件输出目录。不要混用不同游戏版本的程序集。

## 编译

默认路径见 [Directory.Build.props](../Directory.Build.props)：EXILED 使用上级工作区的 `packages/ExMod.Exiled.9.5.0/lib/net48` 和 `Server_NapCha_ui_API/bin/Debug`；LabAPI 使用本机 Steam 专服目录。其他电脑请显式指定：

```powershell
dotnet build src/Exiled/Infinite_Ammo.Exiled.csproj -c Release -p:ExiledReferencesDir="D:\References\Exiled-9.5.0" -p:ExiledGameManagedDir="D:\References\Game-For-Exiled"
dotnet build src/LabApi/Infinite_Ammo.LabApi.csproj -c Release -p:LabApiGameManagedDir="D:\SCPServer\SCPSL_Data\Managed"
```

`ExiledReferencesDir` 需包含 `Exiled.API.dll`、`Exiled.Events.dll`、`Assembly-CSharp-Publicized.dll`；`ExiledGameManagedDir` 需提供匹配的 `UnityEngine.CoreModule.dll`、`Mirror.dll` 等依赖。LabAPI Managed 目录需包含 `LabApi.dll`、`Assembly-CSharp.dll` 和配套依赖。

本机路径已配置时可以直接构建解决方案：

```powershell
dotnet build Infinite_Ammo.sln -c Release
```

输出分别位于 `src/Exiled/bin/Release/net48/Infinite_Ammo.Exiled.dll` 和 `src/LabApi/bin/Release/net48/Infinite_Ammo.LabApi.dll`。若编译器服务遇到输出权限问题，可添加 `-p:UseSharedCompilation=false`。

## 测试

```powershell
dotnet run --project tests/Infinite_Ammo.Tests.csproj -c Release
```

共享逻辑测试覆盖默认数量、换弹消耗后的恢复、不符合条件时不补弹、死亡/被铐清空、配置数量、禁用状态和空玩家。它不模拟服务器框架或游戏网络。

测试服检查：

1. 人类玩家切换物品、连续换弹，备用弹药恢复且弹匣正常工作。
2. SCP、旁观者不补弹；被铐后清空备用弹药，切换物品不能恢复。
3. 死亡时清空备用弹药，主动丢弃弹药被拒绝。
4. 禁用后恢复原版行为，卸载/重新启用无重复回调。
5. 与其他插件共同处理死亡/铐人事件时检查回调顺序。

死亡和铐人采用前置事件，只能尊重进入回调时已有的取消状态。后执行的插件仍可能取消已处理的事件。修改配置后请重启或重新加载插件。

## 代码组织

```text
src/Shared/   共享配置与弹药规则，直接编译进两份 DLL
src/Exiled/   EXILED 入口、事件、玩家适配
src/LabApi/   LabAPI 入口、事件、玩家适配
tests/       不依赖游戏的共享逻辑测试
```

补弹规则以内部最新版练枪服插件为基准，保留五种弹药的默认数量与事件触发时机。内部源目录不参与发布修改。
