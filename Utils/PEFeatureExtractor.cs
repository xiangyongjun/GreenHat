using PeNet;
using PeNet.Header.Pe;
using System;
using System.Collections.Generic;
using System.IO;

namespace GreenHat.utils
{
    public class PEFeatureExtractor
    {
        public static PEData ExtractFeatures(string filePath)
        {
            var result = ExtractFeatures(File.ReadAllBytes(filePath));
            if (result != null)
            {
                result.TrustSigned = WinTrust.VerifyFileSignature(filePath) ? 1 : 0;
                result.HasDirSigned = WinTrust.VerifyDirSignature(filePath) ? 1 : 0;
            }
            return result;
        }

        [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions]
        public static PEData ExtractFeatures(byte[] fileBytes)
        {
            var data = new PEData();

            try
            {
                if (!PeFile.IsPeFile(fileBytes)) return null;
                var pe = new PeFile(fileBytes);

                // 计算文件熵
                data.FileEntropy = (float)CalculateEntropy(fileBytes);

                // 导入函数
                data.ImportFuncs = new HashSet<string>();
                if (pe.ImportedFunctions != null)
                {
                    data.ApiCount = pe.ImportedFunctions.Length;
                    foreach (var func in pe.ImportedFunctions)
                    {
                        if (String.IsNullOrEmpty(func.Name)) continue;
                        data.ImportFuncs.Add(func.Name);
                    }
                }

                // 导出函数
                if (pe.ExportedFunctions != null)
                {
                    data.ExceptionCount = pe.ExportedFunctions.Length;
                }

                // 文件详细信息
                if (pe.Resources?.VsVersionInfo?.StringFileInfo?.StringTable != null)
                {
                    foreach (var fileInfo in pe.Resources.VsVersionInfo.StringFileInfo.StringTable)
                    {
                        try
                        {
                            data.FileDescriptionLength = !String.IsNullOrEmpty(fileInfo.FileDescription) ? fileInfo.FileDescription.Length : 0;
                            data.FileVersionLength = !String.IsNullOrEmpty(fileInfo.FileVersion) ? fileInfo.FileVersion.Length : 0;
                            data.ProductNameLength = !String.IsNullOrEmpty(fileInfo.ProductName) ? fileInfo.ProductName.Length : 0;
                            data.ProductVersionLength = !String.IsNullOrEmpty(fileInfo.ProductVersion) ? fileInfo.ProductVersion.Length : 0;
                            data.CompanyNameLength = !String.IsNullOrEmpty(fileInfo.CompanyName) ? fileInfo.CompanyName.Length : 0;
                            data.LegalCopyrightLength = !String.IsNullOrEmpty(fileInfo.LegalCopyright) ? fileInfo.LegalCopyright.Length : 0;
                            data.SpecialBuildLength = !String.IsNullOrEmpty(fileInfo.SpecialBuild) ? fileInfo.SpecialBuild.Length : 0;
                            data.PrivateBuildLength = !String.IsNullOrEmpty(fileInfo.PrivateBuild) ? fileInfo.PrivateBuild.Length : 0;
                            data.CommentsLength = !String.IsNullOrEmpty(fileInfo.Comments) ? fileInfo.Comments.Length : 0;
                            data.InternalNameLength = !String.IsNullOrEmpty(fileInfo.InternalName) ? fileInfo.InternalName.Length : 0;
                            data.LegalTrademarksLength = !String.IsNullOrEmpty(fileInfo.LegalTrademarks) ? fileInfo.LegalTrademarks.Length : 0;
                        }
                        catch { }
                    }
                }

                // 是否有调试模式
                data.IsDebug = pe.ImageDebugDirectory?.Length > 0 ? 1 : 0;

                // 节区特征
                data.ExecutableSections = 0;
                data.WritableSections = 0;
                foreach (var section in pe.ImageSectionHeaders)
                {
                    if (section.Characteristics == ScnCharacteristicsType.Align2Bytes) data.ExecutableSections++;
                    if (section.Characteristics == ScnCharacteristicsType.Align128Bytes) data.WritableSections++;
                    if (section.SizeOfRawData + section.PointerToRawData > fileBytes.Length)
                    {
                        data.SectionException = 1;
                        continue;
                    }
                    // 计算.text节数据熵
                    if (section.Name.Equals(".text"))
                    {
                        byte[] result = new byte[section.SizeOfRawData];
                        Array.Copy(fileBytes, section.PointerToRawData, result, 0, section.SizeOfRawData);
                        data.TextSection = (float)CalculateEntropy(result);
                        data.TextSizeRatio = result.Length / pe.FileSize;
                    }
                    // 计算.data节数据熵
                    if (section.Name.Equals(".data"))
                    {
                        byte[] result = new byte[section.SizeOfRawData];
                        Array.Copy(fileBytes, section.PointerToRawData, result, 0, section.SizeOfRawData);
                        data.DataSection = (float)CalculateEntropy(result);
                        data.DataSizeRatio = result.Length / pe.FileSize;
                    }
                    // 计算.rsrc节数据熵
                    if (section.Name.Equals(".rsrc"))
                    {
                        byte[] result = new byte[section.SizeOfRawData];
                        Array.Copy(fileBytes, section.PointerToRawData, result, 0, section.SizeOfRawData);
                        data.RsrcSection = (float)CalculateEntropy(result);
                        data.RsrcSizeRatio = result.Length / pe.FileSize;
                    }
                }

                // 是否有签名
                data.IsSigned = pe.IsAuthenticodeSigned ? 1 : 0;

                // 是否是exe文件
                data.IsExe = pe.IsExe ? 1 : 0;

                // 是否是dll文件
                data.IsExe = pe.IsExe ? 1 : 0;

                // 是否是driver文件
                data.IsDriver = pe.IsDriver ? 1 : 0;

                // 是否是64位程序
                data.Is64Bit = pe.Is64Bit ? 1 : 0;

                // 签名是否有效
                data.ValidSigned = pe.HasValidAuthenticodeSignature ? 1 : 0;

                // 签名是否信任
                data.TrustSigned = 0;

                // 异常处理表数量
                data.ExceptionCount = pe.ExceptionDirectory != null ? pe.ExceptionDirectory.Length : 0;

                // 节区数量
                data.SectionCount = pe.ImageSectionHeaders.Length;

                // 是否有目录签名
                data.HasDirSigned = 0;

                // PE 文件基本信息
                data.Machine = (float)pe.ImageNtHeaders.FileHeader.Machine;
                data.NumberOfSections = (float)pe.ImageNtHeaders.FileHeader.NumberOfSections;
                data.TimeDateStamp = (float)pe.ImageNtHeaders.FileHeader.TimeDateStamp;
                data.PointerToSymbolTable = (float)pe.ImageNtHeaders.FileHeader.PointerToSymbolTable;
                data.NumberOfSymbols = (float)pe.ImageNtHeaders.FileHeader.NumberOfSymbols;
                data.SizeOfOptionalHeader = (float)pe.ImageNtHeaders.FileHeader.SizeOfOptionalHeader;
                data.Characteristics = (float)pe.ImageNtHeaders.FileHeader.Characteristics;
                data.Magic = (float)pe.ImageNtHeaders.OptionalHeader.Magic;
                data.MajorLinkerVersion = (float)pe.ImageNtHeaders.OptionalHeader.MajorLinkerVersion;
                data.MinorLinkerVersion = (float)pe.ImageNtHeaders.OptionalHeader.MinorLinkerVersion;
                data.SizeOfCode = (float)pe.ImageNtHeaders.OptionalHeader.SizeOfCode;
                data.SizeOfInitializedData = (float)pe.ImageNtHeaders.OptionalHeader.SizeOfInitializedData;
                data.SizeOfUninitializedData = (float)pe.ImageNtHeaders.OptionalHeader.SizeOfUninitializedData;
                data.AddressOfEntryPoint = (float)pe.ImageNtHeaders.OptionalHeader.AddressOfEntryPoint;
                data.BaseOfCode = (float)pe.ImageNtHeaders.OptionalHeader.BaseOfCode;
                data.ImageBase = (float)pe.ImageNtHeaders.OptionalHeader.ImageBase;
                data.SectionAlignment = (float)pe.ImageNtHeaders.OptionalHeader.SectionAlignment;
                data.FileAlignment = (float)pe.ImageNtHeaders.OptionalHeader.FileAlignment;
                data.MajorOperatingSystemVersion = (float)pe.ImageNtHeaders.OptionalHeader.MajorOperatingSystemVersion;
                data.MinorOperatingSystemVersion = (float)pe.ImageNtHeaders.OptionalHeader.MinorOperatingSystemVersion;
                data.MajorImageVersion = (float)pe.ImageNtHeaders.OptionalHeader.MajorImageVersion;
                data.MinorImageVersion = (float)pe.ImageNtHeaders.OptionalHeader.MinorImageVersion;
                data.MajorSubsystemVersion = (float)pe.ImageNtHeaders.OptionalHeader.MajorSubsystemVersion;
                data.MinorSubsystemVersion = (float)pe.ImageNtHeaders.OptionalHeader.MinorSubsystemVersion;
                data.SizeOfImage = (float)pe.ImageNtHeaders.OptionalHeader.SizeOfImage;
                data.SizeOfHeaders = (float)pe.ImageNtHeaders.OptionalHeader.SizeOfHeaders;
                data.CheckSum = (float)pe.ImageNtHeaders.OptionalHeader.CheckSum;
                data.Subsystem = (float)pe.ImageNtHeaders.OptionalHeader.Subsystem;
                data.DllCharacteristics = (float)pe.ImageNtHeaders.OptionalHeader.DllCharacteristics;
                data.SizeOfStackReserve = (float)pe.ImageNtHeaders.OptionalHeader.SizeOfStackReserve;
                data.SizeOfStackCommit = (float)pe.ImageNtHeaders.OptionalHeader.SizeOfStackCommit;
                data.SizeOfHeapReserve = (float)pe.ImageNtHeaders.OptionalHeader.SizeOfHeapReserve;
                data.SizeOfHeapCommit = (float)pe.ImageNtHeaders.OptionalHeader.SizeOfHeapCommit;
                data.LoaderFlags = (float)pe.ImageNtHeaders.OptionalHeader.LoaderFlags;
                data.NumberOfRvaAndSizes = (float)pe.ImageNtHeaders.OptionalHeader.NumberOfRvaAndSizes;

                return data;
            }
            catch
            {
                return data;
            }
        }

        public static double CalculateEntropy(byte[] data)
        {
            var frequencies = new int[256];
            foreach (var b in data)
                frequencies[b]++;
            double entropy = 0;
            double dataSize = data.Length;
            foreach (var freq in frequencies)
            {
                if (freq > 0)
                {
                    double p = freq / dataSize;
                    entropy -= p * Math.Log(p, 2);
                }
            }
            return entropy;
        }
    }
}
