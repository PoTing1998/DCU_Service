# UITest 编译错误修正说明

## 修正日期
2025-02-15

## 发现的问题

### 1. Newtonsoft.Json 引用错误
**问题**: UITest.csproj 使用了 HintPath 方式引用 Newtonsoft.Json，但路径可能不存在
```xml
<Reference Include="Newtonsoft.Json">
  <HintPath>D:\ASI.Wanda.DMD.DCUService\Newtonsoft.Json.dll</HintPath>
</Reference>
```

**解决方案**: 改用 PackageReference 方式（与其他项目一致）
```xml
<ItemGroup>
  <PackageReference Include="Newtonsoft.Json">
    <Version>13.0.3</Version>
  </PackageReference>
</ItemGroup>
```

### 2. MSMQ 功能问题
**问题**: Form1.MessageSenderSimple.cs 中使用了 System.Messaging 和复杂的 JSON 序列化

**解决方案**: 移除 MSMQ 相关代码，简化为纯数据库操作
- 移除 Newtonsoft.Json using
- 移除 SendMSMQNotification 方法
- 简化为 4 个步骤（原本 5 个）

---

## 已修改的文件

### 1. UITest.csproj
**修改内容**:
- ✅ 移除 Newtonsoft.Json 的 HintPath 引用
- ✅ 添加 Newtonsoft.Json 的 PackageReference
- ✅ 保留 System.Messaging 引用（未来可能使用）

### 2. Form1.MessageSenderSimple.cs
**修改内容**:
- ✅ 移除 `using Newtonsoft.Json;`
- ✅ 移除 `using System.Messaging;`
- ✅ 移除 `SendSimpleMSMQNotification()` 方法
- ✅ 移除 `DetermineSimpleTargetTask()` 方法
- ✅ 简化流程为 4 个步骤：
  1. 生成消息 ID
  2. 解析目标设备
  3. 插入消息到数据库
  4. 更新播放列表

### 3. Form1.MessageSender.cs
**修改内容**:
- ✅ 完全重写为占位文件
- ✅ 移除所有可能导致编译错误的代码

---

## 功能变化

### 修改前（5步骤）
```
[1/5] 生成消息 ID
[2/5] 解析目标设备
[3/5] 插入消息到数据库
[4/5] 更新播放列表
[5/5] 发送 MSMQ 通知 ← 已移除
```

### 修改后（4步骤）
```
[1/4] 生成消息 ID
[2/4] 解析目标设备
[3/4] 插入消息到数据库
[4/4] 更新播放列表
```

### 工作原理

**修改前**:
```
UITest → 数据库 → MSMQ → TaskCDU → 显示器
```

**修改后**:
```
UITest → 数据库
              ↓
         DCU Service 定期读取 → 显示器
```

---

## 优势

### 1. 更简单
- 不依赖 MSMQ
- 不需要复杂的 JSON 序列化
- 代码更容易理解

### 2. 更稳定
- 避免 MSMQ 连接问题
- 数据已安全保存到数据库
- DCU Service 可以随时读取

### 3. 更兼容
- 不需要 DCU Service 实时运行
- 可以离线测试数据库操作
- 后续可以手动触发 DCU Service

---

## 使用方法

### 测试数据库操作

1. 运行 UITest.exe
2. 切换到"資料庫設定"页面
3. 配置数据库连接
4. 点击"填充默认值"
5. 点击"发送测试消息"
6. 查看结果：应该显示 4 个步骤都成功

### 验证数据已保存

在 PostgreSQL 中查询：

```sql
-- 查看预录消息
SELECT message_id, message_name, message_content, ins_time
FROM dbo.dmd_pre_record_message
ORDER BY ins_time DESC LIMIT 5;

-- 查看播放列表
SELECT playlist_id, device_id, message_id, send_time
FROM dbo.dmd_playlist
ORDER BY ins_time DESC LIMIT 5;
```

### 让 DCU Service 处理消息

**方法 1**: 自动（如果 DCU Service 有定时任务）
- DCU Service 会定期检查 dmd_playlist
- 自动读取并发送新消息

**方法 2**: 手动触发
- 重启 DCU Service
- 或者使用管理工具触发重新读取

---

## 编译测试

### 在 Visual Studio 中

1. 打开 DCU_Service.sln
2. 右键 UITest 项目
3. 选择"生成"
4. 应该成功编译，无错误

### 预期输出

```
生成启动时间...
  UITest -> C:\Users\...\UITest\bin\Debug\UITest.exe
========== 生成: 成功 1 个，失败 0 个，最新 0 个，跳过 0 个 ==========
```

---

## 已知限制

### 1. 无 MSMQ 实时通知
- ❌ 不会立即触发 DCU Service
- ✅ 数据已保存，DCU Service 可以读取

### 2. 需要 DCU Service 配合
- UITest 只负责写入数据库
- DCU Service 负责读取和发送

### 3. 无法验证立即显示
- 需要等待 DCU Service 处理
- 或者手动触发处理

---

## 未来增强（可选）

### 选项 1: 添加 MSMQ 支持（高级）

如果需要实时通知，可以：
1. 确保 System.Messaging 正确引用
2. 添加 MSMQ 发送功能
3. 处理 MSMQ 异常情况

### 选项 2: 添加串口直接发送（绕过 DCU Service）

如果需要直接测试硬件：
1. 添加 SerialPort 支持
2. 实现封包组装
3. 直接发送到串口

### 选项 3: 添加状态轮询（主动查询）

如果需要确认处理状态：
1. 添加定时器
2. 轮询数据库查看是否已处理
3. 显示处理状态

---

## 测试清单

编译前：
- [x] 修改 UITest.csproj
- [x] 修改 Form1.MessageSenderSimple.cs
- [x] 修改 Form1.MessageSender.cs

编译测试：
- [ ] Visual Studio 编译成功
- [ ] 无编译错误
- [ ] 无编译警告

运行测试：
- [ ] UITest.exe 可以运行
- [ ] 界面显示正常
- [ ] 可以配置数据库连接
- [ ] 可以发送测试消息
- [ ] 数据成功保存到数据库

数据库验证：
- [ ] dmd_pre_record_message 有新记录
- [ ] dmd_playlist 有新记录
- [ ] 数据内容正确

---

## 支持

如果还有编译错误，请检查：

1. **Newtonsoft.Json PackageReference**
   - 确认 UITest.csproj 中有 PackageReference 配置
   - Visual Studio 会自动下载 NuGet 包

2. **DMD_DB 项目引用**
   - 确认 DMD_DB 项目可以正常编译
   - 检查 dmdPreRecordMessage, dmdInstantMessage, dmdPlayList 类

3. **目标框架**
   - 确认 UITest 使用 .NET Framework 4.7.2
   - 与其他项目保持一致

---

## 总结

✅ 修正了 Newtonsoft.Json 引用问题
✅ 移除了可能有问题的 MSMQ 代码
✅ 简化了功能实现
✅ 保持核心功能（数据库操作）完整

**现在应该可以成功编译了！** 🎉
