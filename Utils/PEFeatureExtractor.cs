using PeNet;
using PeNet.Header.Pe;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace GreenHat.utils
{
    public class PEFeatureExtractor
    {
        // 定义API集合，使用HashSet提高查找效率
        private static Dictionary<string, Action<PEData>> apiGroups = new Dictionary<string, Action<PEData>> {
            { "ReadFile", (data) => data.HasReadFile = 1 },
            { "WriteFile", (data) => data.HasWriteFile = 1 },
            { "DeleteFile", (data) => data.HasDeleteFile = 1 },
            { "ReadReg", (data) => data.HasReadReg = 1 },
            { "WriteReg", (data) => data.HasWriteReg = 1 },
            { "DeleteReg", (data) => data.HasDeleteReg = 1 },
            { "ProcessFunc", (data) => data.HasProcessFunc = 1 },
            { "InternetFunc", (data) => data.HasInternetFunc = 1 },
            { "LoadLibraryFunc", (data) => data.HasLoadLibraryFunc = 1 },
            { "MemoryFunc", (data) => data.HasMemoryFunc = 1 },
            { "HookFunc", (data) => data.HasHookFunc = 1 },
            { "SensitiveFunc", (data) => data.HasSensitiveFunc = 1 }
        };

        // 初始化哈希集合，避免重复创建
        private static Dictionary<string, HashSet<string>> apiSets = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase) {
            { "ReadFile", new HashSet<string>(new[] { "ReadFileA", "ReadFileW", "ReadFileEx" }) },
            { "WriteFile", new HashSet<string>(new[] { "WriteFile", "WriteFileEx" }) },
            { "DeleteFile", new HashSet<string>(new[] { "DeleteFile", "MoveFileEx" }) },
            { "ReadReg", new HashSet<string>(new[] { "RegQueryValueEx", "RegNotifyChangeKeyValue", "RegEnumKeyEx", "RegEnumValue", "RegCopyTree" }) },
            { "WriteReg", new HashSet<string>(new[] { "RegCreateKeyEx", "RegSetValueEx" }) },
            { "DeleteReg", new HashSet<string>(new[] { "RegDeleteValue", "RegDeleteKeyEx", "RegDeleteTree" }) },
            { "ProcessFunc", new HashSet<string>(new[] { "CreateProcess", "TerminateProcess", "OpenProcess", "GetProcessId", "CreateRemoteThread", "ShellExecute", "WinExec" }) },
            { "InternetFunc", new HashSet<string>(new[] { "InternetOpen", "HttpOpenRequest", "WinHttpOpen", "WinHttpSendRequest", "WinHttpReadData", "socket", "connect", "send", "recv", "HttpCreateHttpHandle", "HttpReceiveHttpRequest", "URLDownloadToFile", "InternetConnect", "URLDownloadToCacheFile" }) },
            { "LoadLibraryFunc", new HashSet<string>(new[] { "LoadLibrary", "GetProcAddress" }) },
            { "MemoryFunc", new HashSet<string>(new[] { "VirtualProtect", "VirtualFree", "VirtualQuery", "VirtualLock", "VirtualUnlock", "ReadProcessMemory", "WriteProcessMemory", "VirtualAllocEx" }) },
            { "HookFunc", new HashSet<string>(new[] { "SetWindowsHookEx", "UnhookWindowsHookEx", "CallNextHookEx" }) },
            { "SensitiveFunc", new HashSet<string>(new[] { "SetThreadContext", "NtSetInformationToken", "AdjustTokenPrivileges", "RtlAdjustPrivilege", "memcpy", "strcpy", "NtDelayExecution", "IsDebuggerPresent", "CheckRemoteDebuggerPresent" }) }
        };

        // API统一字典
        private static Dictionary<string, List<Action<PEData>>> apiLookup = new Dictionary<string, List<Action<PEData>>>(StringComparer.OrdinalIgnoreCase);

        static PEFeatureExtractor()
        {
            // 预处理：将所有API存入一个统一的字典
            foreach (var group in apiSets)
            {
                foreach (var api in group.Value)
                {
                    if (!apiLookup.ContainsKey(api)) apiLookup[api] = new List<Action<PEData>>();
                    apiLookup[api].Add(apiGroups[group.Key]);
                }
            }
        }

        [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions]
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
                data.OriginalFilenameLength = !String.IsNullOrEmpty(fileInfo.OriginalFilename) ? fileInfo.OriginalFilename.Length : 0;
                data.LanguageLength = !String.IsNullOrEmpty(fileInfo.Language) ? fileInfo.Language.Length : 0;

                data.IsDebug = fileInfo.IsDebug ? 1 : 0;
                data.IsPatched = fileInfo.IsPatched ? 1 : 0;
                data.IsPrivateBuild = fileInfo.IsPrivateBuild ? 1 : 0;
                data.IsPreRelease = fileInfo.IsPreRelease ? 1 : 0;
                data.IsSpecialBuild = fileInfo.IsSpecialBuild ? 1 : 0;

                data.DebugCount = pe.ImageDebugDirectory?.Length > 0 ? pe.ImageDebugDirectory.Length : 0;
                data.IconCount = pe.Resources?.Icons?.Length > 0 ? pe.Resources.Icons.Length : 0;
                data.GroupIconCount = pe.Resources?.GroupIconDirectories?.Length > 0 ? pe.Resources.GroupIconDirectories.Length : 0;

                data.HasTlsCallbacks = pe.ImageTlsDirectory?.AddressOfCallBacks != null ? 1 : 0;
                data.HasInvalidTimestamp = pe.ImageNtHeaders.FileHeader.TimeDateStamp > DateTime.UtcNow.ToFileTime() ? 1 : 0;
                data.HasRelocationDirectory = pe.ImageNtHeaders.OptionalHeader.DataDirectory[5].Size > 0 ? 1 : 0;

                // 节区特征
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
                        byte[] result = new byte[section.SizeOfRawData];
                        Array.Copy(fileBytes, section.PointerToRawData, result, 0, section.SizeOfRawData);
                        data.TextSection = (float)CalculateEntropy(result);
                        data.TextSizeRatio = result.Length / pe.FileSize;
                    }
                    // 计算.data节数据熵
                    else if (section.Name.Equals(".data"))
                    {
                        byte[] result = new byte[section.SizeOfRawData];
                        Array.Copy(fileBytes, section.PointerToRawData, result, 0, section.SizeOfRawData);
                        data.DataSection = (float)CalculateEntropy(result);
                        data.DataSizeRatio = result.Length / pe.FileSize;
                    }
                    // 计算.rsrc节数据熵
                    else if (section.Name.Equals(".rsrc"))
                    {
                        byte[] result = new byte[section.SizeOfRawData];
                        Array.Copy(fileBytes, section.PointerToRawData, result, 0, section.SizeOfRawData);
                        data.IsAdmin = HasTargetStringInBytes(result, "requireAdministrator", 1024) ? 1 : 0;
                        data.RsrcSection = (float)CalculateEntropy(result);
                        data.RsrcSizeRatio = result.Length / pe.FileSize;
                        if (data.IsInstall != 1) data.IsInstall = HasTargetStringInBytes(result, "Nullsoft.NSIS.exehead", 2048) ? 1 : 0;
                        if (data.IsInstall != 1) data.IsInstall = HasTargetStringInBytes(result, "JR.Inno.Setup", 2048) ? 1 : 0;
                        if (data.IsInstall != 1) data.IsInstall = HasTargetStringInBytes(result, "7-Zip.7zipInstall", 2048) ? 1 : 0;
                        if (data.IsInstall != 1) data.IsInstall = HasTargetStringInBytes(result, "WinRAR SFX", 2048) ? 1 : 0;
                    }
                    else
                    {
                        if (section.Name.Length < 3 || !section.Name.StartsWith(".") || section.Name.Contains(" ")) data.HasNonStandardSection = 1;
                        byte[] result = new byte[section.SizeOfRawData];
                        Array.Copy(fileBytes, section.PointerToRawData, result, 0, section.SizeOfRawData);
                        if (CalculateEntropy(result) > 7.0)
                        {
                            data.HighEntropySections += 1;
                        }
                    }
                }

                // 导入函数
                data.ImportFuncs = new HashSet<string>();
                if (pe.ImportedFunctions != null)
                {
                    data.ApiCount = pe.ImportedFunctions.Length;

                    foreach (var func in pe.ImportedFunctions)
                    {
                        if (string.IsNullOrEmpty(func.Name)) continue;
                        data.ImportFuncs.Add(func.Name);
                        // 检查API分类
                        if (apiLookup.TryGetValue(func.Name, out var actions))
                        {
                            foreach (var action in actions) action(data);
                        }
                    }
                }

                // 导出函数
                if (pe.ExportedFunctions != null)
                {
                    data.ExportCount = pe.ExportedFunctions.Length;
                }

                // 是否有签名
                data.IsSigned = pe.IsAuthenticodeSigned ? 1 : 0;

                // 是否是exe文件
                data.IsExe = pe.IsExe ? 1 : 0;

                // 是否是dll文件
                data.IsDll = pe.IsDll ? 1 : 0;

                // 是否是driver文件
                data.IsDriver = pe.IsDriver ? 1 : 0;

                // 是否是64位程序
                data.Is64Bit = pe.Is64Bit ? 1 : 0;

                // 是否是DotNet程序
                data.IsDotNet = pe.IsDotNet ? 1 : 0;

                // 签名是否有效
                data.ValidSigned = pe.HasValidAuthenticodeSignature ? 1 : 0;

                // 签名是否信任
                data.TrustSigned = data.IsSigned == 1 && WinTrust.VerifyFileSignature(filePath) ? 1 : 0;

                // 异常处理表数量
                data.ExceptionCount = pe.ExceptionDirectory != null ? pe.ExceptionDirectory.Length : 0;

                // 节区数量
                data.SectionCount = pe.ImageSectionHeaders.Length;

                // 是否有目录签名
                data.HasDirSigned = WinTrust.VerifyDirSignature(filePath) ? 1 : 0;

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

        public static unsafe double CalculateEntropy(byte[] data)
        {
            int[] frequencies = new int[256];
            fixed (byte* pData = data)
            fixed (int* pFreq = frequencies)
            {
                byte* ptr = pData;
                byte* end = ptr + data.Length;
                while (ptr < end)
                {
                    pFreq[*ptr++]++;
                }
            }
            double entropy = 0;
            double dataSize = data.Length;
            fixed (int* pFreq = frequencies)
            {
                int* pEnd = pFreq + 256;
                for (int* p = pFreq; p < pEnd; p++)
                {
                    if (*p > 0)
                    {
                        double pValue = (*p) / dataSize;
                        entropy -= pValue * Math.Log(pValue, 2);
                    }
                }
            }
            return entropy;
        }

        public static unsafe bool HasTargetStringInBytes(byte[] source, string text, int searchLength)
        {
            byte[] target = Encoding.ASCII.GetBytes(text);
            int targetLen = target.Length;
            if (source.Length < targetLen) return false;
            int start = Math.Max(0, source.Length - searchLength);
            int end = source.Length - targetLen;
            fixed (byte* pSource = source)
            fixed (byte* pTarget = target)
            {
                byte* pStart = pSource + start;
                byte* pEnd = pSource + end;
                for (byte* pCurrent = pEnd; pCurrent >= pStart; pCurrent--)
                {
                    bool match = true;
                    byte* pSrc = pCurrent;
                    byte* pTgt = pTarget;
                    for (int j = 0; j < targetLen; j++)
                    {
                        if (*pSrc != *pTgt)
                        {
                            match = false;
                            break;
                        }
                        pSrc++;
                        pTgt++;
                    }
                    if (match) return true;
                }
            }
            return false;
        }
    }
}
