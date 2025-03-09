using Microsoft.ML.Data;
using System.Collections.Generic;

namespace GreenHat.utils
{
    public class PEData
    {
        [LoadColumn(0)] public bool Label { get; set; } // 标签列
        [LoadColumn(1)] public float Probability { get; set; } // 概率
        [LoadColumn(2)] public float FileEntropy { get; set; } // 文件熵
        [LoadColumn(3)] public float TextSection { get; set; } // 文本段
        [LoadColumn(4)] public float TextSizeRatio { get; set; } // 文本段大小比例
        [LoadColumn(5)] public float DataSection { get; set; } // 数据段
        [LoadColumn(6)] public float DataSizeRatio { get; set; } // 数据段大小比例
        [LoadColumn(7)] public float RsrcSection { get; set; } // 资源段
        [LoadColumn(8)] public float RsrcSizeRatio { get; set; } // 资源段大小比例
        [LoadColumn(9)] public float IsSigned { get; set; } // 是否签名
        [LoadColumn(10)] public float IsExe { get; set; } // 是否为 EXE 文件
        [LoadColumn(11)] public float IsDll { get; set; } // 是否为 DLL 文件
        [LoadColumn(12)] public float IsDriver { get; set; } // 是否为驱动程序
        [LoadColumn(13)] public float Is64Bit { get; set; } // 是否为 64 位
        [LoadColumn(14)] public float IsDebug { get; set; } // 是否为调试版本
        [LoadColumn(15)] public float SpecialBuildLength { get; set; } // 特殊构建长度
        [LoadColumn(16)] public float PrivateBuildLength { get; set; } // 私有构建长度
        [LoadColumn(17)] public float ValidSigned { get; set; } // 签名是否有效
        [LoadColumn(18)] public float TrustSigned { get; set; } // 签名是否可信
        [LoadColumn(19)] public float ExceptionCount { get; set; } // 异常计数
        [LoadColumn(20)] public float HasDirSigned { get; set; } // 是否有目录签名
        [LoadColumn(21)] public float Machine { get; set; } // 机器类型
        [LoadColumn(22)] public float NumberOfSections { get; set; } // 节数量
        [LoadColumn(23)] public float TimeDateStamp { get; set; } // 时间戳
        [LoadColumn(24)] public float PointerToSymbolTable { get; set; } // 符号表指针
        [LoadColumn(25)] public float NumberOfSymbols { get; set; } // 符号数量
        [LoadColumn(26)] public float SizeOfOptionalHeader { get; set; } // 可选头大小
        [LoadColumn(27)] public float Characteristics { get; set; } // 特性
        [LoadColumn(28)] public float Magic { get; set; } // 魔数
        [LoadColumn(29)] public float MajorLinkerVersion { get; set; } // 主链接器版本
        [LoadColumn(30)] public float MinorLinkerVersion { get; set; } // 次链接器版本
        [LoadColumn(31)] public float SizeOfCode { get; set; } // 代码大小
        [LoadColumn(32)] public float SizeOfInitializedData { get; set; } // 初始化数据大小
        [LoadColumn(33)] public float SizeOfUninitializedData { get; set; } // 未初始化数据大小
        [LoadColumn(34)] public float AddressOfEntryPoint { get; set; } // 入口点地址
        [LoadColumn(35)] public float BaseOfCode { get; set; } // 代码基址
        [LoadColumn(36)] public float ImageBase { get; set; } // 映像基址
        [LoadColumn(37)] public float SectionAlignment { get; set; } // 段对齐
        [LoadColumn(38)] public float FileAlignment { get; set; } // 文件对齐
        [LoadColumn(39)] public float MajorOperatingSystemVersion { get; set; } // 主操作系统版本
        [LoadColumn(40)] public float MinorOperatingSystemVersion { get; set; } // 次操作系统版本
        [LoadColumn(41)] public float MajorImageVersion { get; set; } // 主映像版本
        [LoadColumn(42)] public float MinorImageVersion { get; set; } // 次映像版本
        [LoadColumn(43)] public float MajorSubsystemVersion { get; set; } // 主子系统版本
        [LoadColumn(44)] public float MinorSubsystemVersion { get; set; } // 次子系统版本
        [LoadColumn(45)] public float SizeOfImage { get; set; } // 映像大小
        [LoadColumn(46)] public float SizeOfHeaders { get; set; } // 头大小
        [LoadColumn(47)] public float CheckSum { get; set; } // 校验和
        [LoadColumn(48)] public float Subsystem { get; set; } // 子系统
        [LoadColumn(49)] public float DllCharacteristics { get; set; } // DLL 特性
        [LoadColumn(50)] public float SizeOfStackReserve { get; set; } // 堆栈保留大小
        [LoadColumn(51)] public float SizeOfStackCommit { get; set; } // 堆栈提交大小
        [LoadColumn(52)] public float SizeOfHeapReserve { get; set; } // 堆保留大小
        [LoadColumn(53)] public float SizeOfHeapCommit { get; set; } // 堆提交大小
        [LoadColumn(54)] public float LoaderFlags { get; set; } // 加载器标志
        [LoadColumn(55)] public float NumberOfRvaAndSizes { get; set; } // RVA 和大小数量
        [LoadColumn(56)] public float ApiCount { get; set; } // API 计数
        [LoadColumn(57)] public float ExportCount { get; set; } // 导出计数
        [LoadColumn(58)] public float FileDescriptionLength { get; set; } // 文件描述长度
        [LoadColumn(59)] public float FileVersionLength { get; set; } // 文件版本长度
        [LoadColumn(60)] public float ProductNameLength { get; set; } // 产品名称长度
        [LoadColumn(61)] public float ProductVersionLength { get; set; } // 产品版本长度
        [LoadColumn(62)] public float CompanyNameLength { get; set; } // 公司名称长度
        [LoadColumn(63)] public float LegalCopyrightLength { get; set; } // 法律版权长度
        [LoadColumn(64)] public float CommentsLength { get; set; } // 注释长度
        [LoadColumn(65)] public float InternalNameLength { get; set; } // 内部名称长度
        [LoadColumn(66)] public float LegalTrademarksLength { get; set; } // 法律商标长度
        [LoadColumn(67)] public float ExecutableSections { get; set; } // 可执行段
        [LoadColumn(68)] public float WritableSections { get; set; } // 可写段
        [LoadColumn(69)] public float SectionCount { get; set; } // 段计数
        [LoadColumn(70)] public float SectionException { get; set; } // 段异常
        [NoColumn] public HashSet<string> ImportFuncs { get; set; } // 导入函数
    }

    // 定义预测结果类
    public class MalwarePrediction
    {
        [ColumnName("PredictedLabel")]
        public bool IsMalware { get; set; }

        public float Probability { get; set; }
    }
}