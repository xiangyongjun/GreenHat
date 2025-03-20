using Microsoft.ML.Data;
using System.Collections.Generic;

namespace GreenHat.utils
{
    public class PEData
    {
        [LoadColumn(0)] public bool Label { get; set; } // 标签列（是否为恶意文件）
        [LoadColumn(1)] public float Probability { get; set; } // 恶意概率预测值
        [LoadColumn(2)] public float FileEntropy { get; set; } // 文件熵值
        [LoadColumn(3)] public float TextSection { get; set; } // .text段存在标志
        [LoadColumn(4)] public float TextSizeRatio { get; set; } // .text段大小占比
        [LoadColumn(5)] public float DataSection { get; set; } // .data段存在标志
        [LoadColumn(6)] public float DataSizeRatio { get; set; } // .data段大小占比
        [LoadColumn(7)] public float RsrcSection { get; set; } // .rsrc段存在标志
        [LoadColumn(8)] public float RsrcSizeRatio { get; set; } // .rsrc段大小占比
        [LoadColumn(9)] public float IsSigned { get; set; } // 是否包含数字签名
        [LoadColumn(10)] public float IsExe { get; set; } // 是否为可执行文件
        [LoadColumn(11)] public float IsDll { get; set; } // 是否为动态链接库
        [LoadColumn(12)] public float IsDriver { get; set; } // 是否为驱动程序文件
        [LoadColumn(13)] public float Is64Bit { get; set; } // 是否为64位程序
        [LoadColumn(14)] public float IsDebug { get; set; } // 是否为调试版本
        [LoadColumn(15)] public float IsDotNet { get; set; } // 是否为DotNet程序
        [LoadColumn(16)] public float SpecialBuildLength { get; set; } // 特殊构建字符串长度
        [LoadColumn(17)] public float PrivateBuildLength { get; set; } // 私有构建字符串长度
        [LoadColumn(18)] public float ValidSigned { get; set; } // 签名有效性验证结果
        [LoadColumn(19)] public float TrustSigned { get; set; } // 签名可信度验证
        [LoadColumn(20)] public float ExceptionCount { get; set; } // 异常处理程序数量
        [LoadColumn(21)] public float HasDirSigned { get; set; } // 是否包含目录签名
        [LoadColumn(22)] public float Machine { get; set; } // 目标机器架构类型
        [LoadColumn(23)] public float NumberOfSections { get; set; } // PE节区数量
        [LoadColumn(24)] public float TimeDateStamp { get; set; } // 文件时间戳
        [LoadColumn(25)] public float PointerToSymbolTable { get; set; } // 符号表指针偏移
        [LoadColumn(26)] public float NumberOfSymbols { get; set; } // 符号表条目数量
        [LoadColumn(27)] public float SizeOfOptionalHeader { get; set; } // 可选头大小
        [LoadColumn(28)] public float Characteristics { get; set; } // 文件特性标志
        [LoadColumn(29)] public float Magic { get; set; } // PE魔数（32/64位标识）
        [LoadColumn(30)] public float MajorLinkerVersion { get; set; } // 主链接器版本号
        [LoadColumn(31)] public float MinorLinkerVersion { get; set; } // 次链接器版本号
        [LoadColumn(32)] public float SizeOfCode { get; set; } // 代码段总大小
        [LoadColumn(33)] public float SizeOfInitializedData { get; set; } // 已初始化数据大小
        [LoadColumn(34)] public float SizeOfUninitializedData { get; set; } // 未初始化数据大小
        [LoadColumn(35)] public float AddressOfEntryPoint { get; set; } // 程序入口点地址
        [LoadColumn(36)] public float BaseOfCode { get; set; } // 代码段基址
        [LoadColumn(37)] public float ImageBase { get; set; } // 内存映像基址
        [LoadColumn(38)] public float SectionAlignment { get; set; } // 内存节区对齐值
        [LoadColumn(39)] public float FileAlignment { get; set; } // 文件节区对齐值
        [LoadColumn(40)] public float MajorOperatingSystemVersion { get; set; } // 主OS版本要求
        [LoadColumn(41)] public float MinorOperatingSystemVersion { get; set; } // 次OS版本要求
        [LoadColumn(42)] public float MajorImageVersion { get; set; } // 主映像版本号
        [LoadColumn(43)] public float MinorImageVersion { get; set; } // 次映像版本号
        [LoadColumn(44)] public float MajorSubsystemVersion { get; set; } // 主子系统版本
        [LoadColumn(45)] public float MinorSubsystemVersion { get; set; } // 次子系统版本
        [LoadColumn(46)] public float SizeOfImage { get; set; } // 内存映像总大小
        [LoadColumn(47)] public float SizeOfHeaders { get; set; } // PE头总大小
        [LoadColumn(48)] public float CheckSum { get; set; } // 校验和值
        [LoadColumn(49)] public float Subsystem { get; set; } // 子系统类型（GUI/CUI等）
        [LoadColumn(50)] public float DllCharacteristics { get; set; } // DLL特性标志
        [LoadColumn(51)] public float SizeOfStackReserve { get; set; } // 保留堆栈空间
        [LoadColumn(52)] public float SizeOfStackCommit { get; set; } // 初始提交堆栈
        [LoadColumn(53)] public float SizeOfHeapReserve { get; set; } // 保留堆空间
        [LoadColumn(54)] public float SizeOfHeapCommit { get; set; } // 初始提交堆空间
        [LoadColumn(55)] public float LoaderFlags { get; set; } // 加载器标志位
        [LoadColumn(56)] public float NumberOfRvaAndSizes { get; set; } // RVA表项数量
        [LoadColumn(57)] public float ApiCount { get; set; } // 导入API函数总数
        [LoadColumn(58)] public float ExportCount { get; set; } // 导出函数数量
        [LoadColumn(59)] public float FileDescriptionLength { get; set; } // 文件描述长度
        [LoadColumn(60)] public float FileVersionLength { get; set; } // 文件版本信息长度
        [LoadColumn(61)] public float ProductNameLength { get; set; } // 产品名称长度
        [LoadColumn(62)] public float ProductVersionLength { get; set; } // 产品版本长度
        [LoadColumn(63)] public float CompanyNameLength { get; set; } // 公司名称长度
        [LoadColumn(64)] public float LegalCopyrightLength { get; set; } // 版权信息长度
        [LoadColumn(65)] public float CommentsLength { get; set; } // 注释信息长度
        [LoadColumn(66)] public float InternalNameLength { get; set; } // 内部名称长度
        [LoadColumn(67)] public float LegalTrademarksLength { get; set; } // 商标信息长度
        [LoadColumn(68)] public float ExecutableSections { get; set; } // 可执行节区数量
        [LoadColumn(69)] public float WritableSections { get; set; } // 可写节区数量
        [LoadColumn(70)] public float ReadableSections { get; set; } // 可读节区数量
        [LoadColumn(71)] public float SectionCount { get; set; } // 总节区数量
        [LoadColumn(72)] public float SectionException { get; set; } // 异常节区特征
        [LoadColumn(73)] public float DebugCount { get; set; } // 调试信息数量
        [LoadColumn(74)] public float IsPatched { get; set; } // 是否存在补丁
        [LoadColumn(75)] public float IsPrivateBuild { get; set; } // 是否为私有构建
        [LoadColumn(76)] public float IsPreRelease { get; set; } // 是否为预发布版本
        [LoadColumn(77)] public float IsSpecialBuild { get; set; } // 是否为特殊构建
        [LoadColumn(78)] public float OriginalFilenameLength { get; set; } // 原始文件名长度
        [LoadColumn(79)] public float LanguageLength { get; set; } // 语言信息长度
        [LoadColumn(80)] public float IconCount { get; set; } // 图标资源数量
        [LoadColumn(81)] public float GroupIconCount { get; set; } // 图标组数量
        [LoadColumn(82)] public float IsAdmin { get; set; } // 是否需要管理员权限
        [LoadColumn(83)] public float HasNonStandardSection { get; set; } // 包含非标准节区
        [LoadColumn(84)] public float HighEntropySections { get; set; } // 高熵节区特征
        [LoadColumn(85)] public float HasTlsCallbacks { get; set; } // 包含TLS回调
        [LoadColumn(86)] public float HasInvalidTimestamp { get; set; } // 时间戳异常
        [LoadColumn(87)] public float HasRelocationDirectory { get; set; } // 包含重定位目录
        [LoadColumn(88)] public float IsInstall { get; set; } // 安装程序特征
        [LoadColumn(89)] public float HasReadFile { get; set; } // 包含文件读取API
        [LoadColumn(90)] public float HasWriteFile { get; set; } // 包含文件写入API
        [LoadColumn(91)] public float HasDeleteFile { get; set; } // 包含文件删除API
        [LoadColumn(92)] public float HasReadReg { get; set; } // 包含注册表读取API
        [LoadColumn(93)] public float HasWriteReg { get; set; } // 包含注册表写入API
        [LoadColumn(94)] public float HasDeleteReg { get; set; } // 包含注册表删除API
        [LoadColumn(95)] public float HasProcessFunc { get; set; } // 包含进程操作API
        [LoadColumn(96)] public float HasInternetFunc { get; set; } // 包含网络访问API
        [LoadColumn(97)] public float HasLoadLibraryFunc { get; set; } // 包含动态加载库API
        [LoadColumn(98)] public float HasMemoryFunc { get; set; } // 包含内存操作API
        [LoadColumn(99)] public float HasSensitiveFunc { get; set; } // 包含敏感API调用
        [LoadColumn(100)] public float HasHookFunc { get; set; } // 包含钩子函数API
        [NoColumn] public HashSet<string> ImportFuncs { get; set; } // 导入函数集合
    }

    // 定义预测结果类
    public class MalwarePrediction
    {
        [ColumnName("PredictedLabel")]
        public bool IsMalware { get; set; }

        public float Probability { get; set; }
    }
}