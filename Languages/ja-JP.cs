namespace GreenHat.Languages
{
    class ja_JP : AntdUI.ILocalization
    {
        public string GetLocalizedString(string key)
        {
            switch (key)
            {
                case "绿帽子安全防护": return "GreenHat Security";
                case "绿帽子安全防护正在保护您的电脑安全": return "PC保護中";
                case "主页": return "ホーム";
                case "查杀": return "スキャン";
                case "日志": return "ログ";
                case "设置": return "設定";
                case "关于": return "について";
                case "已保护": return "保護済み";
                case "天": return "日";
                case "文件云鉴定器": return "クラウド鑑定";
                case "开始查杀": return "開始";
                case "快速查杀": return "クイック";
                case "全盘查杀": return "フル";
                case "目录查杀": return "フォルダ";
                case "文件查杀": return "ファイル";
                case "处理器占用": return "CPU%";
                case "内存占用": return "メモリ%";
                case "硬盘占用": return "ディスク%";
                case "官方主页": return "公式サイト";
                case "检查更新": return "更新確認";
                case "信任区": return "信頼リスト";
                case "隔离区": return "隔離リスト";
                case "添加文件": return "ファイル追加";
                case "添加目录": return "フォルダ追加";
                case "删除所选": return "選択削除";
                case "恢复所選": return "選択復元";
                case "文件路径": return "パス";
                case "加入时间": return "追加日時";
                case "类型": return "タイプ";
                case "隔离时间": return "隔離日時";
                case "模型版本": return "モデルバージョン";
                case "个": return "件";
                case "效能模式": return "省エネ";
                case "正常模式": return "通常";
                case "性能模式": return "高性能";
                case "暂停查杀": return "一時停止";
                case "停止查杀": return "ていし";
                case "继续查杀": return "再開";
                case "结束查杀": return "終了";
                case "已用时间": return "経過時間";
                case "查杀引擎": return "エンジン";
                case "病毒类型": return "ウイルス種別";
                case "状态": return "状態";
                case "病毒查杀": return "ウイルス検出";
                case "已扫描": return "スキャン済み";
                case "威胁数量": return "脅威数";
                case "查杀结束": return "完了";
                case "查看详情": return "詳細";
                case "开始日期": return "開始日";
                case "结束日期": return "終了日";
                case "全部": return "全件";
                case "病毒防护": return "ウイルス対策";
                case "其他": return "その他";
                case "导出日志": return "エクスポート";
                case "清空日志": return "クリア";
                case "日志条数": return "件数";
                case "时间": return "日時";
                case "概要": return "概要";
                case "类别": return "カテゴリ";
                case "功能": return "機能";
                case "进程防护": return "プロセス保護";
                case "进程实时监控": return "プロセス保護";
                case "文件防护": return "ファイル保護";
                case "文件实时监控": return "ファイル保護";
                case "引导防护": return "ブート保護";
                case "开机启动": return "起動時実行";
                case "绿帽子机器学习引擎": return "GreenHat 機械学習AI";
                case "猎剑云引擎": return "Talonflame クラウド";
                case "名称": return "名称";
                case "描述": return "説明";
                case "是否启用": return "有効";
                case "作者": return "作者";
                case "关于作者": return "作者情報";
                case "微信": return "WeChat";
                case "邮箱": return "メール";
                case "GitHub地址": return "GitHub";
                case "Gitee地址": return "Gitee";
                case "技术栈": return "技術構成";
                case "官方网站": return "公式サイト";
                case "安全文件": return "安全ファイル";
                case "文件信息": return "ファイル情報";
                case "文件大小": return "サイズ";
                case "文件类型": return "形式";
                case "创建日期": return "作成日";
                case "文件摘要": return "サマリ";
                case "鉴定结果": return "判定結果";
                case "鉴定时间": return "判定日時";
                case "文件名称": return "ファイル名";
                case "选择文件鉴定": return "ファイル選択";
                case "清空鉴定记录": return "履歴クリア";
                case "病毒文件": return "ウイルス";
                case "未知文件": return "不明";
                case "猎剑文件鉴定云": return "Talonflame クラウド鑑定";
                case "操作系统": return "OS";
                case "处理器": return "CPU";
                case "主板": return "マザー";
                case "内存": return "メモリ";
                case "显卡": return "GPU";
                case "硬盘": return "ストレージ";
                case "显示器": return "ディスプレイ";
                case "核": return "コア";
                case "线程": return "スレッド";
                case "打开绿帽子": return "起動";
                case "退出绿帽子": return "終了";
                case "您确定要退出绿帽子安全防护吗？\n届时绿帽子将无法保护您的电脑安全，您的电脑存在被恶意程序攻击的风险。":
                    return "終了しますか？\nPCが攻撃される可能性あり";
                case "仍然退出": return "強制終了";
                case "继续保护": return "保護継続";
                case "警告": return "警告";
                case "检测到本地引擎丢失，防护功能将会失效！": return "エンジン異常！";
                case "恢复成功": return "復元成功";
                case "请选择恢复的行": return "対象選択";
                case "请选择删除的行": return "削除対象";
                case "删除成功": return "削除成功";
                case "所有文件": return "全ファイル";
                case "添加成功": return "追加成功";
                case "分": return "分";
                case "云判定为": return "クラウド判定";
                case "等待鉴定": return "待機中";
                case "发现新的未知文件，是否上传鉴定？": return "新規ファイルを判定？";
                case "确定上传": return "アップロード";
                case "取消上传": return "キャンセル";
                case "鉴定失败，可能由于网络原因！": return "ネットワークエラー";
                case "待处理": return "未処理";
                case "已隔离": return "隔離済み";
                case "隔离完毕": return "隔離完了";
                case "删除查杀文件": return "ファイル削除";
                case "操作时间": return "操作日時";
                case "删除完毕": return "削除完了";
                case "隔离所选": return "選択隔離";
                case "自定义查杀": return "カスタム";
                case "清空成功": return "クリア成功";
                case "CSV文件": return "CSV";
                case "导出成功": return "エクスポート成功";
                case "安全日志": return "セキュリティログ";
                case "开启": return "ON";
                case "关闭": return "OFF";
                case "防护设置": return "保護設定";
                case "查看、管理各项安全防护功能": return "機能管理";
                case "暂无数据": return "データなし";
                case "加载中": return "読み込み中";
                case "程序启动": return "起動完了";
                case "搜索": return "検索";
                case "恢复文件": return "復元";
                case "立即更新": return "すぐに更新";
                case "找到": return "見つける";
                case "个威胁": return " の脅威を";
                case "用时": return "時間";
                case "实时监控拦截所有正在打开的可疑程序": return "不審プロセス監視";
                case "实时监控拦截所有正在写入的可疑文件": return "不審ファイル監視";
                case "实时监控系统引导扇区是否被非法篡改": return "ブートセクタ監視";
                case "程序跟随系统开机启动": return "自動起動";
                case "绿帽子自研的恶意软件检测机学引擎（师承科洛，感谢猎剑云提供的样本）":
                    return "独自AIエンジン";
                case "鹰眼鉴定，秒速响应（云引擎查杀时才会启用）":
                    return "クラウド高速判定";
                case "当前":
                    return "現在";
                case "版本已是最新":
                    return "バージョンは最新です";
                case "当前最新版本为":
                    return "現在の最新バージョンは";
                case "是否前去官网下载升级":
                    return "公式サイトでアップデートしますか";
                case "取消":
                    return "キャンセル";
                case "确定":
                    return "OK";
                default: return null;
            }
        }
    }
}