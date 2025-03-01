using Microsoft.ML.Data;

namespace GreenHat.utils
{
    public class PEData
    {
        [LoadColumn(0)] public bool Label { get; set; }
        [LoadColumn(1)] public float Probability { get; set; }
        [LoadColumn(2)] public float FileEntropy { get; set; }
        [LoadColumn(3)] public float FileSize { get; set; }
        [LoadColumn(4)] public float TextSection { get; set; }
        [LoadColumn(5)] public float TextSizeRatio { get; set; }
        [LoadColumn(6)] public float DataSection { get; set; }
        [LoadColumn(7)] public float DataSizeRatio { get; set; }
        [LoadColumn(8)] public float RsrcSection { get; set; }
        [LoadColumn(9)] public float RsrcSizeRatio { get; set; }
        [LoadColumn(10)] public float IsSigned { get; set; }
        [LoadColumn(11)] public float IsExe { get; set; }
        [LoadColumn(12)] public float IsDll { get; set; }
        [LoadColumn(13)] public float IsDriver { get; set; }
        [LoadColumn(14)] public float IsDotNet { get; set; }
        [LoadColumn(15)] public float Is64Bit { get; set; }
        [LoadColumn(16)] public float IsDebug { get; set; }
        [LoadColumn(17)] public float SpecialBuildLength { get; set; }
        [LoadColumn(18)] public float PrivateBuildLength { get; set; }
        [LoadColumn(19)] public float ValidSigned { get; set; }
        [LoadColumn(20)] public float TrustSigned { get; set; }
        [LoadColumn(21)] public float ExceptionCount { get; set; }
        [LoadColumn(22)] public float HasDirSigned { get; set; }
        [LoadColumn(23)] public float Machine { get; set; }
        [LoadColumn(24)] public float NumberOfSections { get; set; }
        [LoadColumn(25)] public float TimeDateStamp { get; set; }
        [LoadColumn(26)] public float PointerToSymbolTable { get; set; }
        [LoadColumn(27)] public float NumberOfSymbols { get; set; }
        [LoadColumn(28)] public float SizeOfOptionalHeader { get; set; }
        [LoadColumn(29)] public float Characteristics { get; set; }
        [LoadColumn(30)] public float Magic { get; set; }
        [LoadColumn(31)] public float MajorLinkerVersion { get; set; }
        [LoadColumn(32)] public float MinorLinkerVersion { get; set; }
        [LoadColumn(33)] public float SizeOfCode { get; set; }
        [LoadColumn(34)] public float SizeOfInitializedData { get; set; }
        [LoadColumn(35)] public float SizeOfUninitializedData { get; set; }
        [LoadColumn(36)] public float AddressOfEntryPoint { get; set; }
        [LoadColumn(37)] public float BaseOfCode { get; set; }
        [LoadColumn(38)] public float ImageBase { get; set; }
        [LoadColumn(39)] public float SectionAlignment { get; set; }
        [LoadColumn(40)] public float FileAlignment { get; set; }
        [LoadColumn(41)] public float MajorOperatingSystemVersion { get; set; }
        [LoadColumn(42)] public float MinorOperatingSystemVersion { get; set; }
        [LoadColumn(43)] public float MajorImageVersion { get; set; }
        [LoadColumn(44)] public float MinorImageVersion { get; set; }
        [LoadColumn(45)] public float MajorSubsystemVersion { get; set; }
        [LoadColumn(46)] public float MinorSubsystemVersion { get; set; }
        [LoadColumn(47)] public float SizeOfImage { get; set; }
        [LoadColumn(48)] public float SizeOfHeaders { get; set; }
        [LoadColumn(49)] public float CheckSum { get; set; }
        [LoadColumn(50)] public float Subsystem { get; set; }
        [LoadColumn(51)] public float DllCharacteristics { get; set; }
        [LoadColumn(52)] public float SizeOfStackReserve { get; set; }
        [LoadColumn(53)] public float SizeOfStackCommit { get; set; }
        [LoadColumn(54)] public float SizeOfHeapReserve { get; set; }
        [LoadColumn(55)] public float SizeOfHeapCommit { get; set; }
        [LoadColumn(56)] public float LoaderFlags { get; set; }
        [LoadColumn(57)] public float NumberOfRvaAndSizes { get; set; }
        [LoadColumn(58)] public float ApiCount { get; set; }
        [LoadColumn(59)] public float ExportCount { get; set; }
        [LoadColumn(60)] public float FileDescriptionLength { get; set; }
        [LoadColumn(61)] public float FileVersionLength { get; set; }
        [LoadColumn(62)] public float ProductNameLength { get; set; }
        [LoadColumn(63)] public float ProductVersionLength { get; set; }
        [LoadColumn(64)] public float CompanyNameLength { get; set; }
        [LoadColumn(65)] public float LegalCopyrightLength { get; set; }
        [LoadColumn(66)] public float CommentsLength { get; set; }
        [LoadColumn(67)] public float InternalNameLength { get; set; }
        [LoadColumn(68)] public float LegalTrademarksLength { get; set; }
        [LoadColumn(69)] public float ExecutableSections { get; set; }
        [LoadColumn(70)] public float WritableSections { get; set; }
        [LoadColumn(71)] public float SectionCount { get; set; }
        [LoadColumn(72)] public float IconCount { get; set; }
        [LoadColumn(73)] public float SectionException { get; set; }
    }

    // 定义预测结果类
    public class MalwarePrediction
    {
        [ColumnName("PredictedLabel")]
        public bool IsMalware { get; set; }

        public float Probability { get; set; }
    }
}