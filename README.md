# Infinite_Ammo

**[下载最新版本](https://github.com/jikekei/Infinite_Ammo/releases/latest)** · [更新日志](CHANGELOG.zh.md) · [构建与验证](docs/DEVELOPMENT.md)

## 选一版，装进服务器

| 框架 | 下载 | 编译版本 |
| --- | --- | --- |
| EXILED | [下载 DLL](https://github.com/jikekei/Infinite_Ammo/releases/latest/download/Infinite_Ammo.Exiled.dll) | 9.5.0 |
| LabAPI | [下载 DLL](https://github.com/jikekei/Infinite_Ammo/releases/latest/download/Infinite_Ammo.LabApi.dll) | 1.1.7 |

1. 停服，移除旧版无限子弹插件，包括 `KeycardInventoryBypass` DLL。
2. 下载与你的框架对应的 **一份 DLL**。
3. EXILED 放入 `EXILED/Plugins`；LabAPI 放入 `LabAPI/plugins/<端口>` 或 `LabAPI/plugins/global`。
4. 重启服务器。使用存活、未被铐的人类角色切换物品或换弹，检查备用弹药恢复。

两版均面向 .NET Framework 4.8，已用表中框架及配套游戏程序集编译。游戏更新后，请使用匹配依赖重新编译并在测试服验证。不要同时加载两版或旧版。

## 补弹规则

| 时机或状态 | 行为 |
| --- | --- |
| 切换手持物品 / 开始换弹 | 将五种备用弹药设置为配置值 |
| SCP / 死亡与旁观角色 / 被铐 | 不获得补弹 |
| 死亡前 / 被铐前 | 清空备用弹药 |
| 主动丢弃弹药 | 拒绝丢弃 |

默认数量：**12 号霰弹 14；7.62、5.56、.44、9mm 各 101**。

存活且未被铐的人类玩家 → 切换物品或开始换弹 → 备用弹药恢复为配置值。

已有库存会被设置为配置值，可能增加或减少。补弹依靠事件触发，不定时轮询，也不额外在出生或解铐时补弹。

## 按练枪服需求调整

首次加载后编辑框架生成的配置。EXILED 配置前缀为 `无限子弹`；LabAPI 使用插件自己的配置文件。YAML 字段格式以生成文件为准。

| 配置属性 | 默认值 | 用途 |
| --- | --- | --- |
| `IsEnabled` | `true` | 启用插件 |
| `Debug` | `false` | 启动调试日志 |
| `Ammo12Gauge` | `14` | 12 号霰弹备用数量 |
| `Nato762` | `101` | 7.62mm 备用数量 |
| `Nato556` | `101` | 5.56mm 备用数量 |
| `Ammo44Cal` | `101` | .44 备用数量 |
| `Nato9` | `101` | 9mm 备用数量 |

修改后重启服务器或重新加载插件。如果旧配置使用 `keycard_inventory_bypass` 前缀，请将配置迁移到新生成的 `无限子弹` 配置块。

## 验证与边界

两版 Release 构建和共享逻辑测试均已通过。测试覆盖默认数量、补弹、资格限制、清空、配置、禁用和空玩家；**尚未进行游戏内验证**。

死亡、铐人采用前置事件。插件尊重进入回调时已有的取消状态，后执行的其他插件仍可能取消已处理的事件。与其他插件配合时，请在测试服检查回调顺序。完整检查步骤见 [构建与验证](docs/DEVELOPMENT.md)。

## 开发

配置本机依赖路径后：

```powershell
dotnet build Infinite_Ammo.sln -c Release
dotnet run --project tests/Infinite_Ammo.Tests.csproj -c Release
```

共享测试使用 .NET 10 SDK。依赖路径、输出位置和源码结构见 [开发文档](docs/DEVELOPMENT.md)。

## 许可

[GPL-3.0](LICENSE) · 作者 **Yiming**。
