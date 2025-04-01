using PeNet;
using PeNet.Header.Pe;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace GreenHat.utils
{
    public class PEFeatureExtractor
    {
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        private unsafe static extern int memcmp(byte* b1, byte* b2, int count);

        public static PEData ExtractFeatures(string filePath)
        {
            var data = new PEData();

            try
            {
                if (!PeFile.IsPeFile(filePath)) return null;
                var fileBytes = File.ReadAllBytes(filePath);
                var pe = new PeFile(fileBytes);
                var fileInfo = FileVersionInfo.GetVersionInfo(filePath);

                // 计算文件熵
                data.FileEntropy = (float)CalculateEntropy(fileBytes);

                // 文件详细信息
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

                data.IsDebug = fileInfo.IsDebug ? 1 : 0;
                data.IsPatched = fileInfo.IsPatched ? 1 : 0;
                data.IsPrivateBuild = fileInfo.IsPrivateBuild ? 1 : 0;
                data.IsPreRelease = fileInfo.IsPreRelease ? 1 : 0;
                data.IsSpecialBuild = fileInfo.IsSpecialBuild ? 1 : 0;

                if (pe.ImageDebugDirectory != null) data.DebugCount = pe.ImageDebugDirectory.Length ;
                if (pe.Resources?.Icons != null) data.IconCount = pe.Resources.Icons.Length;

                if (pe.ImageTlsDirectory?.AddressOfCallBacks > 0) data.HasTlsCallbacks = 1;
                data.HasInvalidTimestamp = pe.ImageNtHeaders?.FileHeader.TimeDateStamp > DateTime.UtcNow.ToFileTime() ? 1 : 0;
                data.HasRelocationDirectory = pe.ImageNtHeaders?.OptionalHeader.DataDirectory[5].Size > 0 ? 1 : 0;

                // 节区特征
                if (pe.ImageSectionHeaders != null)
                {
                    foreach (var section in pe.ImageSectionHeaders)
                    {
                        if ((section.Characteristics & ScnCharacteristicsType.MemExecute) != 0) data.ExecutableSections++;
                        if ((section.Characteristics & ScnCharacteristicsType.MemWrite) != 0) data.WritableSections++;
                        if ((section.Characteristics & ScnCharacteristicsType.MemRead) != 0) data.ReadableSections++;
                        if (section.SizeOfRawData + section.PointerToRawData > fileBytes.Length)
                        {
                            data.SectionException = 1;
                            continue;
                        }
                        // 计算.text节数据熵
                        if (section.Name.Equals(".text"))
                        {
                            ReadOnlySpan<byte> sectionData = fileBytes.AsSpan(
                                (int)section.PointerToRawData,
                                (int)section.SizeOfRawData
                            );
                            data.TextSection = (float)CalculateEntropy(sectionData.ToArray());
                            data.TextSizeRatio = sectionData.Length / pe.FileSize;
                        }
                        // 计算.data节数据熵
                        else if (section.Name.Equals(".data"))
                        {
                            ReadOnlySpan<byte> sectionData = fileBytes.AsSpan(
                                (int)section.PointerToRawData,
                                (int)section.SizeOfRawData
                            );
                            data.DataSection = (float)CalculateEntropy(sectionData.ToArray());
                            data.DataSizeRatio = sectionData.Length / pe.FileSize;
                        }
                        // 计算.rsrc节数据熵
                        else if (section.Name.Equals(".rsrc"))
                        {
                            ReadOnlySpan<byte> sectionData = fileBytes.AsSpan(
                                (int)section.PointerToRawData,
                                (int)section.SizeOfRawData
                            );
                            data.RsrcSection = (float)CalculateEntropy(sectionData.ToArray());
                            data.RsrcSizeRatio = sectionData.Length / pe.FileSize;
                            if (data.IsAdmin != 1) data.IsAdmin = HasTargetStringInBytes(sectionData.ToArray(), "requireAdministrator", 1024) ? 1 : 0;
                            if (data.IsInstall != 1) data.IsInstall = HasTargetStringInBytes(sectionData.ToArray(), "Nullsoft.NSIS.exehead", 2048) ? 1 : 0;
                            else if (data.IsInstall != 1) data.IsInstall = HasTargetStringInBytes(sectionData.ToArray(), "JR.Inno.Setup", 2048) ? 1 : 0;
                            else if (data.IsInstall != 1) data.IsInstall = HasTargetStringInBytes(sectionData.ToArray(), "7-Zip.7zipInstall", 2048) ? 1 : 0;
                            else if (data.IsInstall != 1) data.IsInstall = HasTargetStringInBytes(sectionData.ToArray(), "WinRAR SFX", 2048) ? 1 : 0;
                        }
                    }
                }

                // 导入函数
                if (pe.ImportedFunctions != null) data.ApiCount = pe.ImportedFunctions.Length;

                // 导出函数
                if (pe.ExportedFunctions != null) data.ExportCount = pe.ExportedFunctions.Length;

                // 是否是exe文件
                data.IsExe = pe.IsExe ? 1 : 0;

                // 是否是dll文件
                data.IsDll = pe.IsDll ? 1 : 0;

                // 是否是driver文件
                data.IsDriver = pe.IsDriver ? 1 : 0;

                // 是否是64位程序
                data.Is64Bit = pe.Is64Bit ? 1 : 0;

                // 签名是否信任
                data.TrustSigned = WinTrust.VerifyFileSignature(filePath) ? 1 : 0;

                // 异常处理表数量
                if (pe.ExceptionDirectory != null) data.ExceptionCount = pe.ExceptionDirectory.Length;

                // 节区数量
                if (pe.ImageSectionHeaders != null) data.SectionCount = pe.ImageSectionHeaders.Length;

                // PE 文件基本信息
                if (pe.ImageNtHeaders != null)
                {
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
                }

                return data;
            }
            catch
            {
                return data;
            }
        }

        public static unsafe double CalculateEntropy(byte[] data)
        {
            const int BufferSize = 256;
            int* frequencies = stackalloc int[BufferSize];
            for (int i = 0; i < BufferSize; i++) frequencies[i] = 0;
            fixed (byte* pData = data)
            {
                byte* ptr = pData;
                byte* end = ptr + data.Length;
                while (end - ptr >= 4)
                {
                    frequencies[ptr[0]]++;
                    frequencies[ptr[1]]++;
                    frequencies[ptr[2]]++;
                    frequencies[ptr[3]]++;
                    ptr += 4;
                }
                while (ptr < end) frequencies[*ptr++]++;
            }
            double entropy = 0;
            double dataSize = data.Length;
            if (dataSize == 0) return 0;
            double invDataSize = 1.0 / dataSize;
            double invLog2 = 1.0 / Math.Log(2);
            for (int i = 0; i < BufferSize; i++)
            {
                int freq = frequencies[i];
                if (freq > 0)
                {
                    double pValue = freq * invDataSize;
                    entropy -= pValue * (Math.Log(pValue) * invLog2);
                }
            }
            return entropy;
        }

        public static unsafe bool HasTargetStringInBytes(byte[] source, string text, int searchLength)
        {
            byte[] target = Encoding.ASCII.GetBytes(text);
            int targetLen = target.Length;
            if (targetLen == 0) return true;
            if (source.Length < targetLen) return false;
            int start = Math.Max(0, source.Length - searchLength);
            int end = source.Length - targetLen;
            if (start > end) return false;
            fixed (byte* pSource = source)
            fixed (byte* pTarget = target)
            {
                byte* pStart = pSource + start;
                byte* pEnd = pSource + end;
                for (byte* pCurrent = pEnd; pCurrent >= pStart; pCurrent--)
                {
                    if (memcmp(pCurrent, pTarget, targetLen) == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
