# 更新日志

## 2.0.1 - 2026-09-30

### 名称与安装

- 两版插件名称恢复为 **Infinite_Ammo**，作者统一为 **Yiming**。
- DLL、工程文件、解决方案和代码命名空间统一使用 `Infinite_Ammo`。
- EXILED 使用当前实现的 `无限子弹` 配置前缀；若旧配置使用 `keycard_inventory_bypass`，请迁移到新生成的配置块。
- 升级时移除旧的 `KeycardInventoryBypass` DLL，只安装 EXILED 或 LabAPI 其中一版。

### 项目介绍

- 按 README 技能重排纯 Markdown 介绍：版本下载、安装、规则、配置与验证边界。
- 使用文字和表格展示事件触发与默认弹药数量，不添加 SVG 或其他图片。
- 将详细构建及测试服检查说明移到 `docs/DEVELOPMENT.md`。

### 验证与下载

- 保留 v2.0.0 的事件补弹逻辑，EXILED 9.5.0 / LabAPI 1.1.7 编译目标不变。
- 两版 Release 构建及共享逻辑测试通过；尚未进行游戏内验证。
- `Infinite_Ammo.Exiled.dll`：EXILED 版。
- `Infinite_Ammo.LabApi.dll`：LabAPI 版。

## 2.0.0 - 2026-09-30

### 升级说明

- EXILED 配置前缀改为 `keycard_inventory_bypass`，原 `无限子弹` 配置不会自动迁移。
- 使用内部最新版的事件补弹逻辑替换旧版定时补弹：切换物品、换弹时恢复备用弹药。弹匣仍正常消耗，仍需换弹。
- 安装前移除旧插件，并且只加载 EXILED 或 LabAPI 其中一版。

### 新功能

- 新增独立 LabAPI 版本，使用本机专服的 LabAPI 1.1.7 及配套游戏依赖编译。
- EXILED 版本使用内部原项目的 EXILED 9.5.0 及匹配游戏依赖编译。
- 提供五种备用弹药数量配置，默认霰弹 14，其余四种弹药 101。
- 保留内部逻辑：SCP、死亡/旁观者和被铐玩家不补弹；死亡前和被铐前清空备用弹药；禁止主动丢弃弹药。

### 重构与修复

- 分离插件入口、事件处理和玩家适配，两版共享同一份弹药规则。
- 移除空事件回调及无关的 HintServiceMeow、PluginAPI 等依赖。
- 对称注销事件，防止插件卸载后残留回调。
- 尊重进入回调时已被取消的事件；插件禁用时不修改库存。

### 验证

- 两版 Release 构建均通过，零警告、零错误。
- 共享逻辑测试通过，覆盖默认数量、补弹、资格限制、清空、配置、禁用和空玩家。
- 尚未进行游戏内验证；更新游戏后需使用对应依赖重新编译。与其他插件共同处理死亡/铐人前置事件时，请在测试服检查事件顺序。

### 下载

- `KeycardInventoryBypass.Exiled.dll`：放入 EXILED 插件目录。
- `KeycardInventoryBypass.LabApi.dll`：放入 LabAPI 插件目录。
- 只安装适合服务器框架的那一份 DLL，无需额外共享 DLL。
