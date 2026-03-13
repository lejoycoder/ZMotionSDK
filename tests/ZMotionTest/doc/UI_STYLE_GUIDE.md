# ZMotionTest UI 风格指南

## 1. 设计原则
- 工业监控优先：状态可读性高于装饰性，关键状态一眼可见。
- 信息分层明确：控制区、监控区、结果区固定分区，避免混排。
- 操作可预期：同类按钮在全项目保持一致的颜色、位置和交互反馈。
- 语义化视觉：颜色、图标、文本三者共同表达状态，不依赖单一通道。

## 2. 视觉令牌
### 2.1 颜色令牌
- 背景：`ZMColorBg`（主背景）、`ZMColorPanel`（面板）、`ZMColorPanelSoft`（次级面板）。
- 主色：`ZMColorPrimary`、`ZMColorPrimaryLight`。
- 文本：`ZMColorText`（主文本）、`ZMColorTextMuted`（辅助文本）。
- 状态：`ZMColorSuccess`、`ZMColorWarning`、`ZMColorDanger`。

### 2.2 间距令牌
- 页面外边距：`16`。
- 面板间距：`12/16`。
- 组件内边距：输入控件 `8~10`，按钮 `12x6`，卡片 `12~16`。

### 2.3 圆角与边框
- 基础圆角：`8`。
- 卡片圆角：`10`。
- 徽章/Chip 圆角：`14`。
- 边框主色：`#FF314766`。

### 2.4 字体令牌
- 字体族：`MaterialDesignFont=Segoe UI`。
- 标题：`Headline4/5/6`。
- 副标题：`Subtitle1`。
- 注释：`Caption`。

## 3. 状态语义规范
- `Success`：动作完成、连接成功、状态正常。
- `Warning`：可恢复风险、参数边界、需确认动作。
- `Error`：动作失败、连接中断、急停与危险指令。
- `Info`：默认提示、过程信息、日志反馈。

主窗口连接状态必须通过 `UiConnectionStateModel` 输出：
- 文案：`已连接/未连接`
- 图标：`PackIconKind.Connection/CloseNetworkOutline`
- 颜色：绿色/红色语义刷

## 4. 控件规范
- 按钮：
  - 主操作使用 `MaterialDesignRaisedButton`
  - 次操作使用 `MaterialDesignOutlinedButton`
  - 导航/辅助操作使用 `MaterialDesignFlatButton`
- 输入：`TextBox`、`ComboBox` 统一使用 `MaterialDesignFloatingHint*` 样式键。
- 容器：统一使用 `materialDesign:Card` 承载模块；顶部/底部横条使用 `materialDesign:ColorZone`。
- 状态徽章：统一使用 `materialDesign:Chip` 或 `Border + 语义色`。
- 表格：`DataGrid` 统一深色工业风表头、水平分割线、统一 Cell Padding。

## 5. 页面布局模板
- 主窗口模板：
  - 顶部：标题区 + 当前页标识。
  - 左侧：导航区（固定宽度）。
  - 中央：内容区（Frame）。
  - 底部：状态区（连接状态、反馈、时间）。
- 业务页模板：
  - 控制区：参数输入 + 触发按钮。
  - 监控区：实时状态、指标卡片。
  - 结果区：DataGrid、日志、返回消息。

## 6. 交互与反馈规范
- 危险操作（断开、急停、停止）必须使用危险色；文案明确动作对象。
- 命令执行后必须有状态反馈（成功/失败/错误原因）。
- 导航必须有选中态，当前页标题与导航联动更新。
- 长耗时动作建议显示“执行中”提示，完成后写入状态栏。

## 7. 可访问性与可读性
- 文本与背景对比度保持高对比（尤其状态文本）。
- 关键信息不低于 12px；主标题不低于 16px。
- 仅用颜色表达状态时，必须补充文本或图标。
- 表格列标题语义清晰，单位明确（ms、mm、units）。

## 8. Do / Don't
### Do
- 使用统一资源键，不在页面内写死颜色。
- 保持模块标题、按钮层级、间距节奏一致。
- 将状态定义集中到 ViewModel 状态模型，而非散落 Brush 字段。

### Don't
- 不新增页面级“临时样式”覆盖全局样式。
- 不在不同页面对同类操作使用不同颜色语义。
- 不在关键操作按钮上使用含糊文案（如“执行”“处理”）。

## 9. 迁移检查清单
- [ ] 页面已使用统一 `materialDesign` 官方命名空间（`http://materialdesigninxaml.net/winfx/xaml/themes`）。
- [ ] 已依赖并启用 MaterialDesignThemes 包与默认主题资源。
- [ ] 关键样式键均来自 `Styles/FluentIndustrialTheme.xaml`。
- [ ] 主窗口导航选中态正常，`CurrentPageTitle` 随导航变更。
- [ ] 连接状态通过 `UiConnectionStateModel` 输出并联动状态栏。
- [ ] 危险操作按钮语义色统一且行为正确。
- [ ] `dotnet build tests/ZMotionTest/ZMotionTest.csproj` 通过。
