﻿namespace GreenHat.Languages
{
    class en_US : AntdUI.ILocalization
    {
        public string GetLocalizedString(string key)
        {
            switch (key)
            {
                case "绿帽子安全防护": return "GreenHat Security";
                case "绿帽子安全防护正在保护您的电脑安全": return "GreenHat Security is protecting your computer";
                case "主页": return "Home";
                case "查杀": return "Scan";
                case "日志": return "Logs";
                case "设置": return "Settings";
                case "关于": return "About";
                case "已保护": return "Protected";
                case "天": return "Days";
                case "文件云鉴定器": return "Cloud Scan";
                case "开始查杀": return "Start Scan";
                case "快速查杀": return "Quick Scan";
                case "全盘查杀": return "Full Scan";
                case "目录查杀": return "Folder Scan";
                case "文件查杀": return "File Scan";
                case "处理器占用": return "CPU Usage";
                case "内存占用": return "RAM Usage";
                case "硬盘占用": return "Disk Usage";
                case "官方主页": return "Website";
                case "检查更新": return "Updates";
                case "信任区": return "Trusted";
                case "隔离区": return "Quarantine";
                case "添加文件": return "Add File";
                case "添加目录": return "Add Folder";
                case "删除所选": return "Delete";
                case "恢复所选": return "Restore";
                case "文件路径": return "File Path";
                case "加入时间": return "Added Time";
                case "类型": return "Type";
                case "隔离时间": return "Quarantine Time";
                case "模型版本": return "Model Version";
                case "个": return " Items";
                case "效能模式": return "Slow";
                case "正常模式": return "Normal";
                case "性能模式": return "Fast";
                case "暂停查杀": return "Pause Scan";
                case "停止查杀": return "Stop Scan";
                case "继续查杀": return "Resume Scan";
                case "结束查杀": return "Stop Scan";
                case "已用时间": return "Time Used";
                case "查杀引擎": return "Engine";
                case "病毒类型": return "Type";
                case "状态": return "Status";
                case "病毒查杀": return "Virus Scan";
                case "已扫描": return "Scanned";
                case "威胁数量": return "Threats Found";
                case "查杀结束": return "Scan Completed";
                case "查看详情": return "View Details";
                case "开始日期": return "Start Date";
                case "结束日期": return "End Date";
                case "全部": return "All";
                case "病毒防护": return "Virus Protection";
                case "其他": return "Others";
                case "导出日志": return "Export Logs";
                case "清空日志": return "Clear Logs";
                case "日志条数": return "Log Entries";
                case "时间": return "Time";
                case "概要": return "Summary";
                case "类别": return "Category";
                case "功能": return "Features";
                case "进程防护": return "Process Protection";
                case "进程实时监控": return "Process Protection";
                case "文件防护": return "File Protection";
                case "文件实时监控": return "File Protection";
                case "引导防护": return "Boot Protection";
                case "开机启动": return "Launch at Startup";
                case "绿帽子机器学习引擎": return "GreenHat ML Engine";
                case "猎剑云引擎": return "Talonflame Cloud Engine";
                case "名称": return "Name";
                case "描述": return "Description";
                case "是否启用": return "Enabled";
                case "作者": return "Author";
                case "关于作者": return "About Author";
                case "微信": return "WeChat";
                case "邮箱": return "Email";
                case "GitHub地址": return "GitHub";
                case "Gitee地址": return "Gitee";
                case "技术栈": return "Tech Stack";
                case "官方网站": return "Website";
                case "安全文件": return "Safe";
                case "文件信息": return "File Info";
                case "文件大小": return "File Size";
                case "文件类型": return "File Type";
                case "创建日期": return "Created";
                case "文件摘要": return "File Summary";
                case "鉴定结果": return "Analysis Result";
                case "鉴定时间": return "Analyzed";
                case "文件名称": return "File Name";
                case "选择文件鉴定": return "Select File";
                case "清空鉴定记录": return "Clear History";
                case "病毒文件": return "Malware";
                case "未知文件": return "Unknown";
                case "猎剑文件鉴定云": return "Talonflame Cloud Scan";
                case "操作系统": return "OS";
                case "处理器": return "CPU";
                case "主板": return "Motherboard";
                case "内存": return "RAM";
                case "显卡": return "GPU";
                case "硬盘": return "Disk";
                case "显示器": return "Monitor";
                case "核": return "Cores";
                case "线程": return "Threads";
                case "打开绿帽子": return "Open GreenHat";
                case "退出绿帽子": return "Exit GreenHat";
                case "您确定要退出绿帽子安全防护吗？\n届时绿帽子将无法保护您的电脑安全，您的电脑存在被恶意程序攻击的风险。":
                    return "Exit GreenHat Security?\nYour computer will lose real-time protection.";
                case "仍然退出": return "Exit Anyway";
                case "继续保护": return "Keep Protecting";
                case "警告": return "Warning";
                case "检测到本地引擎丢失，防护功能将会失效！": return "Engine missing! Protection disabled!";
                case "恢复成功": return "Restored";
                case "请选择恢复的行": return "Select to restore";
                case "请选择删除的行": return "Select to delete";
                case "删除成功": return "Deleted";
                case "所有文件": return "All Files";
                case "添加成功": return "Added";
                case "分": return " Scores";
                case "云判定为": return "Cloud Result";
                case "等待鉴定": return "Pending Analysis";
                case "发现新的未知文件，是否上传鉴定？": return "Upload new file for analysis?";
                case "确定上传": return "Upload";
                case "取消上传": return "Cancel";
                case "鉴定失败，可能由于网络原因！": return "Analysis failed! Check network.";
                case "待处理": return "Pending";
                case "已隔离": return "Quarantined";
                case "隔离完毕": return "Quarantine Complete";
                case "删除查杀文件": return "Delete Scan Files";
                case "操作时间": return "Action Time";
                case "删除完毕": return "Deletion Complete";
                case "隔离所选": return "Quarantine";
                case "自定义查杀": return "Custom Scan";
                case "清空成功": return "Cleared";
                case "CSV文件": return "CSV File";
                case "导出成功": return "Exported";
                case "安全日志": return "Security Logs";
                case "开启": return "On";
                case "关闭": return "Off";
                case "防护设置": return "Protection Settings";
                case "查看、管理各项安全防护功能": return "Manage security features";
                case "暂无数据": return "No Data";
                case "加载中": return "Loading...";
                case "程序启动": return "Program Start";
                case "搜索": return "Search";
                case "恢复文件": return "Restore";
                case "立即更新": return "Update";
                case "找到": return "Found";
                case "个威胁": return " threats";
                case "用时": return "Time";
                case "实时监控拦截所有正在打开的可疑程序": return "Block suspicious processes";
                case "实时监控拦截所有正在写入的可疑文件": return "Block suspicious file writes";
                case "实时监控系统引导扇区是否被非法篡改": return "Monitor boot sector integrity";
                case "程序跟随系统开机启动": return "Start with Windows";
                case "绿帽子自研的恶意软件检测机学引擎（师承科洛，感谢猎剑云提供的样本）": return "AI engine for malware detection";
                case "鹰眼鉴定，秒速响应（云引擎查杀时才会启用）": return "Hunter Sword Cloud Engine (Cloud engine)";
                case "当前": return "Current";
                case "版本已是最新": return "The version is already up to date";
                case "当前最新版本为": return "The latest version is ";
                case "是否前去官网下载升级": return "Go to the official website to download the update";
                case "取消": return "Cancel";
                case "确定": return "OK";
                default: return null;
            }
        }
    }
}