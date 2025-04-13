﻿namespace GreenHat.Languages
{
    class ru_RU : AntdUI.ILocalization
    {
        public string GetLocalizedString(string key)
        {
            switch (key)
            {
                case "绿帽子安全防护": return "GreenHat Security";
                case "绿帽子安全防护正在保护您的电脑安全": return "Защита GreenHat активна";
                case "主页": return "Home";
                case "查杀": return "Scan";
                case "日志": return "Logs";
                case "设置": return "Settings";
                case "关于": return "About";
                case "已保护": return "Защищено";
                case "天": return "дн.";
                case "文件云鉴定器": return "Cloud проверка";
                case "开始查杀": return "Начать";
                case "快速查杀": return "Быстрая";
                case "全盘查杀": return "Полная";
                case "目录查杀": return "Каталог";
                case "文件查杀": return "Файл";
                case "处理器占用": return "CPU";
                case "内存占用": return "RAM";
                case "硬盘占用": return "Диск";
                case "官方主页": return "Сайт";
                case "检查更新": return "Обновить";
                case "信任区": return "Доверенные";
                case "隔离区": return "Карантин";
                case "添加文件": return "Добавить файл";
                case "添加目录": return "Добавить папку";
                case "删除所选": return "Удалить";
                case "恢复所选": return "Восстановить";
                case "文件路径": return "Путь";
                case "加入时间": return "Дата добавления";
                case "类型": return "Тип";
                case "隔离时间": return "Дата изоляции";
                case "模型版本": return "Версия модели";
                case "个": return " шт.";
                case "效能模式": return "Эконом";
                case "正常模式": return "Стандарт";
                case "性能模式": return "Максимум";
                case "暂停查杀": return "Пауза";
                case "停止查杀": return "Остановить";
                case "继续查杀": return "Продолжить";
                case "结束查杀": return "Стоп";
                case "已用时间": return "Время";
                case "查杀引擎": return "Антивирус";
                case "病毒类型": return "Тип угрозы";
                case "状态": return "Статус";
                case "病毒查杀": return "Антивирус";
                case "已扫描": return "Проверено";
                case "威胁数量": return "Угрозы";
                case "查杀结束": return "Завершено";
                case "查看详情": return "Детали";
                case "开始日期": return "Начало";
                case "结束日期": return "Конец";
                case "全部": return "Все";
                case "病毒防护": return "Антивирус";
                case "其他": return "Другое";
                case "导出日志": return "Экспорт";
                case "清空日志": return "Очистить";
                case "日志条数": return "Записей";
                case "时间": return "Время";
                case "概要": return "Итог";
                case "类别": return "Категория";
                case "功能": return "Функция";
                case "进程防护": return "Процессы";
                case "进程实时监控": return "Процессы";
                case "文件防护": return "Файлы";
                case "文件实时监控": return "Файлы";
                case "引导防护": return "Загрузка";
                case "开机启动": return "Автозагрузка";
                case "绿帽子机器学习引擎": return "AI-анализ";
                case "猎剑云引擎": return "Talonflame Cloud Engine";
                case "名称": return "Название";
                case "描述": return "Описание";
                case "是否启用": return "Включено";
                case "作者": return "Автор";
                case "关于作者": return "О разработчике";
                case "微信": return "WeChat";
                case "邮箱": return "Email";
                case "GitHub地址": return "GitHub";
                case "Gitee地址": return "Gitee";
                case "技术栈": return "Технологии";
                case "官方网站": return "Сайт";
                case "安全文件": return "Безопасные";
                case "文件信息": return "Информация";
                case "文件大小": return "Размер";
                case "文件类型": return "Тип";
                case "创建日期": return "Создан";
                case "文件摘要": return "Хэш";
                case "鉴定结果": return "Вердикт";
                case "鉴定时间": return "Дата проверки";
                case "文件名称": return "Имя файла";
                case "选择文件鉴定": return "Выбрать файл";
                case "清空鉴定记录": return "Очистить историю";
                case "病毒文件": return "Вирусы";
                case "未知文件": return "Неизвестные";
                case "猎剑文件鉴定云": return "Talonflame Cloud проверка";
                case "操作系统": return "ОС";
                case "处理器": return "CPU";
                case "主板": return "Материнская плата";
                case "内存": return "RAM";
                case "显卡": return "GPU";
                case "硬盘": return "Диск";
                case "显示器": return "Монитор";
                case "核": return "Ядер";
                case "线程": return "Потоков";
                case "打开绿帽子": return "Запустить";
                case "退出绿帽子": return "Выход";
                case "您确定要退出绿帽子安全防护吗？\n届时绿帽子将无法保护您的电脑安全，您的电脑存在被恶意程序攻击的风险。": return "Вы уверены? Без защиты возможны атаки!";
                case "仍然退出": return "Выйти";
                case "继续保护": return "Остаться";
                case "警告": return "Внимание!";
                case "检测到本地引擎丢失，防护功能将会失效！": return "Движок поврежден!";
                case "恢复成功": return "Восстановлено";
                case "请选择恢复的行": return "Выберите для восстановления";
                case "请选择删除的行": return "Выберите для удаления";
                case "删除成功": return "Удалено";
                case "所有文件": return "Все файлы";
                case "添加成功": return "Добавлено";
                case "请选择要删除的行": return "Выберите для удаления";
                case "分": return " части";
                case "云判定为": return "Cloud: ";
                case "等待鉴定": return "В очереди";
                case "发现新的未知文件，是否上传鉴定？": return "Отправить на анализ?";
                case "确定上传": return "Да";
                case "取消上传": return "Нет";
                case "鉴定失败，可能由于网络原因！": return "Ошибка сети!";
                case "待处理": return "Ожидание";
                case "已隔离": return "В карантине";
                case "隔离完毕": return "Изолировано";
                case "删除查杀文件": return "Удалить файлы";
                case "操作时间": return "Время операции";
                case "删除完毕": return "Удалено";
                case "隔离所选": return "Изолировать";
                case "自定义查杀": return "Больше";
                case "清空成功": return "Очищено";
                case "CSV文件": return "CSV";
                case "导出成功": return "Экспортировано";
                case "安全日志": return "Журнал безопасности";
                case "开启": return "Вкл.";
                case "关闭": return "Выкл.";
                case "防护设置": return "Настройки защиты";
                case "查看、管理各项安全防护功能": return "Управление защитой";
                case "暂无数据": return "Нет данных";
                case "加载中": return "Загрузка...";
                case "程序启动": return "Запуск";
                case "搜索": return "Поиск";
                case "恢复文件": return "Восстановить файл";
                case "立即更新": return "Обновление";
                case "找到": return "Найдено";
                case "个威胁": return "угроз";
                case "用时": return "Это займет";
                case "实时监控拦截所有正在打开的可疑程序": return "Блокировка запуска";
                case "实时监控拦截所有正在写入的可疑文件": return "Блокировка записи";
                case "实时监控系统引导扇区是否被非法篡改": return "Защита загрузчика";
                case "程序跟随系统开机启动": return "Автозагрузка с Windows";
                case "绿帽子自研的恶意软件检测机学引擎（师承科洛，感谢猎剑云提供的样本）": return "AI-детектирование угроз";
                case "鹰眼鉴定，秒速响应（云引擎查杀时才会启用）": return "Мгновенный облачный анализ";
                case "当前": return "Текущий";
                case "版本已是最新": return "Версия уже актуальная";
                case "当前最新版本为": return "Последняя версия";
                case "是否前去官网下载升级": return "Перейти на официальный сайт для обновления";
                case "取消": return "Отмена";
                case "确定": return "ОК";
                case "打开文件所在目录": return "Открыть каталог расположения файла";
                case "添加信任": return "Добавить в доверенные";
                case "加入隔离": return "Поместить в карантин";
                case "删除文件": return "Удалить файл";
                case "右键菜单": return "Контекстное меню";
                case "添加文件右键菜单，一键快速查杀": return "Добавить контекстное меню для файлов, быстрое сканирование одним щелчком";
                case "已删除": return "Удалено";
                case "已信任": return "Доверено";
                case "已处理": return "Обработано";
                case "一键导出选中文件": return "Экспорт выбранных файлов";
                case "导出完毕": return "Экспорт завершен";
                case "使用文件云鉴定": return "Оценка через облачный файл";
                case "脚本查杀引擎": return "Движок для обнаружения и удаления скриптов";
                case "绿帽子自研的脚本查杀引擎（使用沙盒分析行为进行查杀）": return "Собственный движок GreenHat для обнаружения скриптов (анализ поведения в песочнице для выявления угроз)";
                default: return null;
            }
        }
    }
}