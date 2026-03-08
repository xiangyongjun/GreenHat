using PeNet;
using PeNet.Header.Pe;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;

namespace GreenHat.Utils
{
    public class PEFeatureExtractor
    {
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        private unsafe static extern int memcmp(byte* b1, byte* b2, int count);

        public static readonly HashSet<string> PackedSectionNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            // UPX
            "UPX0", "UPX1", "UPX2", "UPX3",

            // ASPack
            ".aspack", ".adata",

            // FSG
            ".fsg", ".fsg2",

            // PECompact
            ".pcode", ".pdata", ".pext", ".prel",

            // WinUpack
            "WU0", "WU1", "WU2",

            // MEW
            "MEW", ".mew", ".mewx",

            // kkrunchy
            ".kkrn",

            // Yoda's Crypter
            ".yC", ".yP",

            // tElock
            ".tels", ".teld",

            // EXE Stealth
            ".est", ".esec",

            // Obsidium
            ".obs", ".obsd", ".obsc",

            // Armadillo
            ".armadillo",

            // Enigma Protector
            ".enigma1", ".enigma2", ".ep0", ".ep1",

            // ASProtect
            ".aspr", ".asprotect",

            // EXECryptor
            ".exr", ".execrypt",

            // Petite
            ".petite", ".ptit",

            // NsPack
            ".nsp0", ".nsp1", ".nsp2",

            // RLPack
            ".rlpack", ".rlp",

            // WWPACK
            "WWP0", "WWP1", "WWP2",

            // PC-Guard
            ".pcg", ".pcguard",

            // DotFix (.NET)
            ".dfix", ".dotfix"
        };

        // 预构建 API -> 字段设置器映射表（高性能、无反射）
        // 键名与 SQL 表字段名完全一致（区分大小写）
        private static readonly Dictionary<string, Action<PEData>> ApiFieldSetters =
            new Dictionary<string, Action<PEData>>(StringComparer.OrdinalIgnoreCase)
            {
                // ==================== 进程创建与控制 ====================
                { "CreateProcessA", d => d.CreateProcessA = 1 },
                { "CreateProcessW", d => d.CreateProcessW = 1 },
                { "WinExec", d => d.WinExec = 1 },
                { "ShellExecuteA", d => d.ShellExecuteA = 1 },
                { "ShellExecuteW", d => d.ShellExecuteW = 1 },
                { "ShellExecuteExW", d => d.ShellExecuteExW = 1 },
                { "ExitProcess", d => d.ExitProcess = 1 },
                { "TerminateProcess", d => d.TerminateProcess = 1 },
                { "OpenProcess", d => d.OpenProcess = 1 },
                { "GetExitCodeProcess", d => d.GetExitCodeProcess = 1 },
                { "SetThreadPriority", d => d.SetThreadPriority = 1 },
                { "GetThreadPriority", d => d.GetThreadPriority = 1 },
                { "GetCurrentProcess", d => d.GetCurrentProcess = 1 },
                { "GetCurrentProcessId", d => d.GetCurrentProcessId = 1 },
                { "GetModuleFileNameA", d => d.GetModuleFileNameA = 1 },
                { "GetCommandLineA", d => d.GetCommandLineA = 1 },
                { "GetModuleFileNameW", d => d.GetModuleFileNameW = 1 },
                { "GetStartupInfoW", d => d.GetStartupInfoW = 1 },

                // ==================== 内存操作 ====================
                { "VirtualAlloc", d => d.VirtualAlloc = 1 },
                { "VirtualAllocEx", d => d.VirtualAllocEx = 1 },
                { "VirtualProtect", d => d.VirtualProtect = 1 },
                { "VirtualProtectEx", d => d.VirtualProtectEx = 1 },
                { "WriteProcessMemory", d => d.WriteProcessMemory = 1 },
                { "ReadProcessMemory", d => d.ReadProcessMemory = 1 },
                { "CreateRemoteThread", d => d.CreateRemoteThread = 1 },
                { "QueueUserAPC", d => d.QueueUserAPC = 1 },
                { "SetThreadContext", d => d.SetThreadContext = 1 },
                { "GetThreadContext", d => d.GetThreadContext = 1 },
                { "Wow64SetThreadContext", d => d.Wow64SetThreadContext = 1 },
                { "NtUnmapViewOfSection", d => d.NtUnmapViewOfSection = 1 },
                { "ZwUnmapViewOfSection", d => d.ZwUnmapViewOfSection = 1 },
                { "RtlCreateUserThread", d => d.RtlCreateUserThread = 1 },

                // ==================== 钩子与注入 ====================
                { "SetWindowsHookExA", d => d.SetWindowsHookExA = 1 },
                { "SetWindowsHookExW", d => d.SetWindowsHookExW = 1 },
                { "UnhookWindowsHookEx", d => d.UnhookWindowsHookEx = 1 },

                // ==================== DLL 加载 ====================
                { "LoadLibraryA", d => d.LoadLibraryA = 1 },
                { "LoadLibraryW", d => d.LoadLibraryW = 1 },
                { "LoadLibraryExA", d => d.LoadLibraryExA = 1 },
                { "LoadLibraryExW", d => d.LoadLibraryExW = 1 },
                { "GetProcAddress", d => d.GetProcAddress = 1 },
                { "GetModuleHandleA", d => d.GetModuleHandleA = 1 },
                { "GetModuleHandleW", d => d.GetModuleHandleW = 1 },
                { "FreeLibrary", d => d.FreeLibrary = 1 },

                // ==================== 文件映射 ====================
                { "CreateFileMappingA", d => d.CreateFileMappingA = 1 },
                { "CreateFileMappingW", d => d.CreateFileMappingW = 1 },
                { "MapViewOfFile", d => d.MapViewOfFile = 1 },
                { "VirtualFree", d => d.VirtualFree = 1 },
                { "VirtualQuery", d => d.VirtualQuery = 1 },

                // ==================== 同步对象 ====================
                { "WaitForSingleObject", d => d.WaitForSingleObject = 1 },
                { "WaitForSingleObjectEx", d => d.WaitForSingleObjectEx = 1 },
                { "WaitForMultipleObjects", d => d.WaitForMultipleObjects = 1 },
                { "WaitForMultipleObjectsEx", d => d.WaitForMultipleObjectsEx = 1 },
                { "CreateMutexA", d => d.CreateMutexA = 1 },
                { "CreateMutexW", d => d.CreateMutexW = 1 },
                { "OpenMutexW", d => d.OpenMutexW = 1 },
                { "ReleaseMutex", d => d.ReleaseMutex = 1 },
                { "CreateEventA", d => d.CreateEventA = 1 },
                { "CreateEventW", d => d.CreateEventW = 1 },
                { "OpenEventW", d => d.OpenEventW = 1 },
                { "SetEvent", d => d.SetEvent = 1 },
                { "ResetEvent", d => d.ResetEvent = 1 },
                { "EnterCriticalSection", d => d.EnterCriticalSection = 1 },
                { "LeaveCriticalSection", d => d.LeaveCriticalSection = 1 },
                { "InitializeCriticalSection", d => d.InitializeCriticalSection = 1 },
                { "DeleteCriticalSection", d => d.DeleteCriticalSection = 1 },
                { "Sleep", d => d.Sleep = 1 },
                { "SleepEx", d => d.SleepEx = 1 },
                { "InitializeCriticalSectionAndSpinCount", d => d.InitializeCriticalSectionAndSpinCount = 1 },
                { "InterlockedDecrement", d => d.InterlockedDecrement = 1 },
                { "InterlockedIncrement", d => d.InterlockedIncrement = 1 },
                { "InterlockedExchange", d => d.InterlockedExchange = 1 },
                { "InterlockedCompareExchange", d => d.InterlockedCompareExchange = 1 },
                { "SetTimer", d => d.SetTimer = 1 },
                { "KillTimer", d => d.KillTimer = 1 },
                { "CreateThread", d => d.CreateThread = 1 },
                { "ResumeThread", d => d.ResumeThread = 1 },
                { "SuspendThread", d => d.SuspendThread = 1 },
                { "ExitThread", d => d.ExitThread = 1 },
                { "TerminateThread", d => d.TerminateThread = 1 },
                { "GetCurrentThread", d => d.GetCurrentThread = 1 },
                { "GetCurrentThreadId", d => d.GetCurrentThreadId = 1 },
                { "TlsAlloc", d => d.TlsAlloc = 1 },
                { "TlsSetValue", d => d.TlsSetValue = 1 },
                { "TlsGetValue", d => d.TlsGetValue = 1 },
                { "CreateThreadpoolWork", d => d.CreateThreadpoolWork = 1 },
                { "SubmitThreadpoolWork", d => d.SubmitThreadpoolWork = 1 },
                { "TlsFree", d => d.TlsFree = 1 },

                // ==================== 网络 Socket ====================
                { "Socket", d => d.Socket = 1 },
                { "Connect", d => d.Connect = 1 },
                { "Send", d => d.Send = 1 },
                { "Recv", d => d.Recv = 1 },
                { "Bind", d => d.Bind = 1 },
                { "Listen", d => d.Listen = 1 },
                { "Accept", d => d.Accept = 1 },
                { "Gethostbyname", d => d.Gethostbyname = 1 },
                { "Getaddrinfo", d => d.Getaddrinfo = 1 },
                { "WSAStartup", d => d.WSAStartup = 1 },
                { "WSACleanup", d => d.WSACleanup = 1 },
                { "WSAIoctl", d => d.WSAIoctl = 1 },
                { "WSASocketA", d => d.WSASocketA = 1 },
                { "WSASocketW", d => d.WSASocketW = 1 },
                { "Closesocket", d => d.Closesocket = 1 },
                { "Htons", d => d.Htons = 1 },

                // ==================== 网络 WinINet ====================
                { "InternetOpenA", d => d.InternetOpenA = 1 },
                { "InternetOpenW", d => d.InternetOpenW = 1 },
                { "InternetConnectA", d => d.InternetConnectA = 1 },
                { "InternetConnectW", d => d.InternetConnectW = 1 },
                { "InternetOpenUrlA", d => d.InternetOpenUrlA = 1 },
                { "InternetOpenUrlW", d => d.InternetOpenUrlW = 1 },
                { "HttpOpenRequestA", d => d.HttpOpenRequestA = 1 },
                { "HttpOpenRequestW", d => d.HttpOpenRequestW = 1 },
                { "HttpSendRequestA", d => d.HttpSendRequestA = 1 },
                { "HttpSendRequestW", d => d.HttpSendRequestW = 1 },
                { "InternetReadFile", d => d.InternetReadFile = 1 },
                { "URLDownloadToFileA", d => d.URLDownloadToFileA = 1 },
                { "URLDownloadToFileW", d => d.URLDownloadToFileW = 1 },
                { "DnsQueryA", d => d.DnsQueryA = 1 },
                { "DnsQueryW", d => d.DnsQueryW = 1 },

                // ==================== 加密 CryptoAPI ====================
                { "CryptAcquireContextA", d => d.CryptAcquireContextA = 1 },
                { "CryptAcquireContextW", d => d.CryptAcquireContextW = 1 },
                { "CryptCreateHash", d => d.CryptCreateHash = 1 },
                { "CryptHashData", d => d.CryptHashData = 1 },
                { "CryptDeriveKey", d => d.CryptDeriveKey = 1 },
                { "CryptEncrypt", d => d.CryptEncrypt = 1 },
                { "CryptDecrypt", d => d.CryptDecrypt = 1 },
                { "CryptDestroyKey", d => d.CryptDestroyKey = 1 },
                { "CryptDestroyHash", d => d.CryptDestroyHash = 1 },
                { "CryptReleaseContext", d => d.CryptReleaseContext = 1 },
                { "CryptGenKey", d => d.CryptGenKey = 1 },
                { "CryptImportKey", d => d.CryptImportKey = 1 },
                { "CryptExportKey", d => d.CryptExportKey = 1 },
                { "CryptSignHash", d => d.CryptSignHash = 1 },
                { "CryptVerifySignature", d => d.CryptVerifySignature = 1 },
                { "CryptSetKeyParam", d => d.CryptSetKeyParam = 1 },
                { "CryptGetKeyParam", d => d.CryptGetKeyParam = 1 },
                { "CryptSetHashParam", d => d.CryptSetHashParam = 1 },
                { "CryptGetHashParam", d => d.CryptGetHashParam = 1 },

                // ==================== 编码/工具函数 ====================
                { "RtlDecompressBuffer", d => d.RtlDecompressBuffer = 1 },
                { "MultiByteToWideChar", d => d.MultiByteToWideChar = 1 },
                { "WideCharToMultiByte", d => d.WideCharToMultiByte = 1 },
                { "Base64Decode", d => d.Base64Decode = 1 },
                { "CryptDecodeObject", d => d.CryptDecodeObject = 1 },
                { "IsDBCSLeadByte", d => d.IsDBCSLeadByte = 1 },
                { "CharUpperA", d => d.CharUpperA = 1 },
                { "CharLowerA", d => d.CharLowerA = 1 },
                { "GetStringTypeW", d => d.GetStringTypeW = 1 },
                { "LCMapStringW", d => d.LCMapStringW = 1 },
                { "IsValidCodePage", d => d.IsValidCodePage = 1 },
                { "DecodePointer", d => d.DecodePointer = 1 },
                { "EncodePointer", d => d.EncodePointer = 1 },

                // ==================== 文件操作 ====================
                { "CreateFileA", d => d.CreateFileA = 1 },
                { "CreateFileW", d => d.CreateFileW = 1 },
                { "WriteFile", d => d.WriteFile = 1 },
                { "ReadFile", d => d.ReadFile = 1 },
                { "DeleteFileA", d => d.DeleteFileA = 1 },
                { "DeleteFileW", d => d.DeleteFileW = 1 },
                { "CopyFileA", d => d.CopyFileA = 1 },
                { "CopyFileW", d => d.CopyFileW = 1 },
                { "MoveFileA", d => d.MoveFileA = 1 },
                { "MoveFileW", d => d.MoveFileW = 1 },
                { "FindFirstFileA", d => d.FindFirstFileA = 1 },
                { "FindNextFileA", d => d.FindNextFileA = 1 },
                { "GetTempPathA", d => d.GetTempPathA = 1 },
                { "GetTempPathW", d => d.GetTempPathW = 1 },
                { "GetTempFileNameA", d => d.GetTempFileNameA = 1 },
                { "GetTempFileNameW", d => d.GetTempFileNameW = 1 },
                { "SetFileAttributesA", d => d.SetFileAttributesA = 1 },
                { "SetFileAttributesW", d => d.SetFileAttributesW = 1 },
                { "DeviceIoControl", d => d.DeviceIoControl = 1 },
                { "SetFileTime", d => d.SetFileTime = 1 },
                { "GetFileSize", d => d.GetFileSize = 1 },
                { "GetFileSizeEx", d => d.GetFileSizeEx = 1 },
                { "SetFilePointer", d => d.SetFilePointer = 1 },
                { "FlushFileBuffers", d => d.FlushFileBuffers = 1 },
                { "ConnectNamedPipe", d => d.ConnectNamedPipe = 1 },
                { "PeekNamedPipe", d => d.PeekNamedPipe = 1 },
                { "CloseHandle", d => d.CloseHandle = 1 },
                { "GetFileType", d => d.GetFileType = 1 },
                { "SetStdHandle", d => d.SetStdHandle = 1 },
                { "SetFilePointerEx", d => d.SetFilePointerEx = 1 },
                { "FindClose", d => d.FindClose = 1 },
                { "SetHandleCount", d => d.SetHandleCount = 1 },
                { "SetEndOfFile", d => d.SetEndOfFile = 1 },
                { "GetFullPathNameA", d => d.GetFullPathNameA = 1 },
                { "GetFileAttributesA", d => d.GetFileAttributesA = 1 },

                // ==================== 注册表操作 ====================
                { "RegOpenKeyExA", d => d.RegOpenKeyExA = 1 },
                { "RegOpenKeyExW", d => d.RegOpenKeyExW = 1 },
                { "RegSetValueExA", d => d.RegSetValueExA = 1 },
                { "RegSetValueExW", d => d.RegSetValueExW = 1 },
                { "RegCreateKeyExA", d => d.RegCreateKeyExA = 1 },
                { "RegCreateKeyExW", d => d.RegCreateKeyExW = 1 },
                { "RegDeleteKeyA", d => d.RegDeleteKeyA = 1 },
                { "RegDeleteKeyW", d => d.RegDeleteKeyW = 1 },
                { "RegEnumValueA", d => d.RegEnumValueA = 1 },
                { "RegEnumValueW", d => d.RegEnumValueW = 1 },
                { "RegQueryValueExA", d => d.RegQueryValueExA = 1 },
                { "RegQueryValueExW", d => d.RegQueryValueExW = 1 },
                { "RegDeleteValueA", d => d.RegDeleteValueA = 1 },
                { "RegDeleteValueW", d => d.RegDeleteValueW = 1 },
                { "RegCloseKey", d => d.RegCloseKey = 1 },

                // ==================== 服务管理 ====================
                { "OpenSCManagerA", d => d.OpenSCManagerA = 1 },
                { "OpenSCManagerW", d => d.OpenSCManagerW = 1 },
                { "CreateServiceA", d => d.CreateServiceA = 1 },
                { "CreateServiceW", d => d.CreateServiceW = 1 },
                { "StartServiceA", d => d.StartServiceA = 1 },
                { "StartServiceW", d => d.StartServiceW = 1 },
                { "ControlService", d => d.ControlService = 1 },
                { "DeleteService", d => d.DeleteService = 1 },

                // ==================== 权限与用户 ====================
                { "AdjustTokenPrivileges", d => d.AdjustTokenPrivileges = 1 },
                { "LookupPrivilegeValueA", d => d.LookupPrivilegeValueA = 1 },
                { "LookupPrivilegeValueW", d => d.LookupPrivilegeValueW = 1 },
                { "OpenProcessToken", d => d.OpenProcessToken = 1 },
                { "NetUserAdd", d => d.NetUserAdd = 1 },
                { "NetLocalGroupAddMembers", d => d.NetLocalGroupAddMembers = 1 },
                { "GetUserNameA", d => d.GetUserNameA = 1 },
                { "GetUserNameW", d => d.GetUserNameW = 1 },

                // ==================== 异常处理 ====================
                { "RtlUnwind", d => d.RtlUnwind = 1 },
                { "RtlVirtualUnwind", d => d.RtlVirtualUnwind = 1 },
                { "RtlCaptureContext", d => d.RtlCaptureContext = 1 },
                { "RtlLookupFunctionEntry", d => d.RtlLookupFunctionEntry = 1 },
                { "NtClose", d => d.NtClose = 1 },
                { "NtQueryInformationProcess", d => d.NtQueryInformationProcess = 1 },
                { "RtlAllocateHeap", d => d.RtlAllocateHeap = 1 },
                { "RtlFreeHeap", d => d.RtlFreeHeap = 1 },

                // ==================== 系统信息 ====================
                { "GetSystemTimeAsFileTime", d => d.GetSystemTimeAsFileTime = 1 },
                { "GetLocalTime", d => d.GetLocalTime = 1 },
                { "GlobalMemoryStatus", d => d.GlobalMemoryStatus = 1 },
                { "GetVersionExA", d => d.GetVersionExA = 1 },
                { "GetVersionExW", d => d.GetVersionExW = 1 },
                { "GetComputerNameA", d => d.GetComputerNameA = 1 },
                { "GetComputerNameW", d => d.GetComputerNameW = 1 },
                { "GetLastError", d => d.GetLastError = 1 },
                { "GetACP", d => d.GetACP = 1 },
                { "RaiseException", d => d.RaiseException = 1 },
                { "SetLastError", d => d.SetLastError = 1 },
                { "GetEnvironmentStringsW", d => d.GetEnvironmentStringsW = 1 },
                { "FreeEnvironmentStringsW", d => d.FreeEnvironmentStringsW = 1 },
                { "GetLocaleInfoA", d => d.GetLocaleInfoA = 1 },
                { "LstrlenA", d => d.LstrlenA = 1 },
                { "GetVersion", d => d.GetVersion = 1 },
                { "GetSystemInfo", d => d.GetSystemInfo = 1 },

                // ==================== 反调试/反分析 ====================
                { "CompareStringA", d => d.CompareStringA = 1 },
                { "IsDebuggerPresent", d => d.IsDebuggerPresent = 1 },
                { "CheckRemoteDebuggerPresent", d => d.CheckRemoteDebuggerPresent = 1 },
                { "OutputDebugStringA", d => d.OutputDebugStringA = 1 },
                { "OutputDebugStringW", d => d.OutputDebugStringW = 1 },
                { "GetTickCount", d => d.GetTickCount = 1 },
                { "GetTickCount64", d => d.GetTickCount64 = 1 },
                { "QueryPerformanceCounter", d => d.QueryPerformanceCounter = 1 },
                { "FindWindowA", d => d.FindWindowA = 1 },
                { "FindWindowW", d => d.FindWindowW = 1 },
                { "EnumWindows", d => d.EnumWindows = 1 },
                { "EnumChildWindows", d => d.EnumChildWindows = 1 },
                { "GetWindowRect", d => d.GetWindowRect = 1 },
                { "GetClientRect", d => d.GetClientRect = 1 },
                { "SetWindowDisplayAffinity", d => d.SetWindowDisplayAffinity = 1 },
                { "UnhandledExceptionFilter", d => d.UnhandledExceptionFilter = 1 },
                { "SetUnhandledExceptionFilter", d => d.SetUnhandledExceptionFilter = 1 },
                { "IsProcessorFeaturePresent", d => d.IsProcessorFeaturePresent = 1 },
                { "SetErrorMode", d => d.SetErrorMode = 1 },
                { "GetTimeZoneInformation", d => d.GetTimeZoneInformation = 1 },
                { "GetDriveTypeW", d => d.GetDriveTypeW = 1 },
                { "GetDiskFreeSpaceA", d => d.GetDiskFreeSpaceA = 1 },

                // ==================== 输入捕获（键盘/鼠标）====================
                { "GetAsyncKeyState", d => d.GetAsyncKeyState = 1 },
                { "GetKeyState", d => d.GetKeyState = 1 },
                { "GetKeyboardState", d => d.GetKeyboardState = 1 },
                { "MapVirtualKeyA", d => d.MapVirtualKeyA = 1 },
                { "MapVirtualKeyW", d => d.MapVirtualKeyW = 1 },
                { "ToAscii", d => d.ToAscii = 1 },
                { "ToAsciiEx", d => d.ToAsciiEx = 1 },
                { "ToUnicode", d => d.ToUnicode = 1 },
                { "ToUnicodeEx", d => d.ToUnicodeEx = 1 },
                { "GetKeyNameTextA", d => d.GetKeyNameTextA = 1 },
                { "GetForegroundWindow", d => d.GetForegroundWindow = 1 },
                { "KeybdEvent", d => d.KeybdEvent = 1 },
                { "SendInput", d => d.SendInput = 1 },
                { "MapVirtualKeyExA", d => d.MapVirtualKeyExA = 1 },
                { "MapVirtualKeyExW", d => d.MapVirtualKeyExW = 1 },
                { "CallNextHookEx", d => d.CallNextHookEx = 1 },
                { "GetCursorPos", d => d.GetCursorPos = 1 },
                { "SetCursorPos", d => d.SetCursorPos = 1 },
                { "MouseEvent", d => d.MouseEvent = 1 },
                { "GetDoubleClickTime", d => d.GetDoubleClickTime = 1 },
                { "GetCapture", d => d.GetCapture = 1 },
                { "SetCapture", d => d.SetCapture = 1 },

                // ==================== 截屏/图形（GDI+/DirectX）====================
                { "GetDC", d => d.GetDC = 1 },
                { "GetWindowDC", d => d.GetWindowDC = 1 },
                { "CreateCompatibleDC", d => d.CreateCompatibleDC = 1 },
                { "CreateCompatibleBitmap", d => d.CreateCompatibleBitmap = 1 },
                { "BitBlt", d => d.BitBlt = 1 },
                { "StretchBlt", d => d.StretchBlt = 1 },
                { "GdipSaveImageToFile", d => d.GdipSaveImageToFile = 1 },
                { "PrintWindow", d => d.PrintWindow = 1 },
                { "GetDesktopWindow", d => d.GetDesktopWindow = 1 },
                { "CreateDCW", d => d.CreateDCW = 1 },
                { "CreateDCA", d => d.CreateDCA = 1 },
                { "SelectObject", d => d.SelectObject = 1 },
                { "DeleteDC", d => d.DeleteDC = 1 },
                { "DeleteObject", d => d.DeleteObject = 1 },
                { "GetDeviceCaps", d => d.GetDeviceCaps = 1 },
                { "GetSystemMetrics", d => d.GetSystemMetrics = 1 },
                { "Direct3DCreate9", d => d.Direct3DCreate9 = 1 },
                { "D3D11CreateDevice", d => d.D3D11CreateDevice = 1 },
                { "IDirect3DDevice9Present", d => d.IDirect3DDevice9Present = 1 },
                { "IDXGISwapChainPresent", d => d.IDXGISwapChainPresent = 1 },

                // ==================== 音频录制 ====================
                { "WaveInOpen", d => d.WaveInOpen = 1 },
                { "WaveInClose", d => d.WaveInClose = 1 },
                { "WaveInStart", d => d.WaveInStart = 1 },
                { "WaveInStop", d => d.WaveInStop = 1 },
                { "WaveInPrepareHeader", d => d.WaveInPrepareHeader = 1 },
                { "WaveInUnprepareHeader", d => d.WaveInUnprepareHeader = 1 },
                { "WaveInAddBuffer", d => d.WaveInAddBuffer = 1 },
                { "WaveOutOpen", d => d.WaveOutOpen = 1 },
                { "WaveOutWrite", d => d.WaveOutWrite = 1 },
                { "WaveInGetNumDevs", d => d.WaveInGetNumDevs = 1 },
                { "WaveInGetDevCapsA", d => d.WaveInGetDevCapsA = 1 },
                { "WaveInGetDevCapsW", d => d.WaveInGetDevCapsW = 1 },

                // ==================== 剪贴板操作 ====================
                { "OpenClipboard", d => d.OpenClipboard = 1 },
                { "CloseClipboard", d => d.CloseClipboard = 1 },
                { "GetClipboardData", d => d.GetClipboardData = 1 },
                { "SetClipboardData", d => d.SetClipboardData = 1 },
                { "EmptyClipboard", d => d.EmptyClipboard = 1 },
                { "IsClipboardFormatAvailable", d => d.IsClipboardFormatAvailable = 1 },

                // ==================== 视频捕获/屏幕录制 ====================
                { "CapCreateCaptureWindowA", d => d.CapCreateCaptureWindowA = 1 },
                { "CapCreateCaptureWindowW", d => d.CapCreateCaptureWindowW = 1 },
                { "LoopbackCapture", d => d.LoopbackCapture = 1 },
                { "MFStartup", d => d.MFStartup = 1 },
                { "MFCreateSinkWriterFromURL", d => d.MFCreateSinkWriterFromURL = 1 },
                { "MFCreateSourceReaderFromMediaSource", d => d.MFCreateSourceReaderFromMediaSource = 1 },

                // ==================== 音频会话/媒体基础 ====================
                { "IAudioSessionManager2", d => d.IAudioSessionManager2 = 1 },
                { "IAudioSessionControl", d => d.IAudioSessionControl = 1 },
                { "IAudioClient", d => d.IAudioClient = 1 },
                { "IMMDeviceEnumerator", d => d.IMMDeviceEnumerator = 1 },

                // ==================== 键盘布局/输入法 ====================
                { "GetKeyboardLayout", d => d.GetKeyboardLayout = 1 },
                { "GetKeyboardLayoutList", d => d.GetKeyboardLayoutList = 1 },
                { "ActivateKeyboardLayout", d => d.ActivateKeyboardLayout = 1 },
                { "ImmGetVirtualKey", d => d.ImmGetVirtualKey = 1 },
                { "ImmGetIMEFileName", d => d.ImmGetIMEFileName = 1 },

                // ==================== 内存分配（堆/全局/本地）====================
                { "HeapAlloc", d => d.HeapAlloc = 1 },
                { "HeapFree", d => d.HeapFree = 1 },
                { "HeapSize", d => d.HeapSize = 1 },
                { "HeapCreate", d => d.HeapCreate = 1 },
                { "GetProcessHeap", d => d.GetProcessHeap = 1 },
                { "LocalAlloc", d => d.LocalAlloc = 1 },
                { "LocalFree", d => d.LocalFree = 1 },
                { "GlobalAlloc", d => d.GlobalAlloc = 1 },
                { "GlobalFree", d => d.GlobalFree = 1 },
                { "GlobalUnlock", d => d.GlobalUnlock = 1 },
                { "HeapDestroy", d => d.HeapDestroy = 1 },
                { "GlobalLock", d => d.GlobalLock = 1 },
                { "SysFreeString", d => d.SysFreeString = 1 },
                { "SysAllocStringLen", d => d.SysAllocStringLen = 1 },

                // ==================== 资源操作 ====================
                { "LoadResource", d => d.LoadResource = 1 },
                { "SizeofResource", d => d.SizeofResource = 1 },
                { "LockResource", d => d.LockResource = 1 },
                { "FreeResource", d => d.FreeResource = 1 },
                { "FindResourceA", d => d.FindResourceA = 1 },

                // ==================== 窗口/消息处理 ====================
                { "ShowWindow", d => d.ShowWindow = 1 },
                { "DestroyWindow", d => d.DestroyWindow = 1 },
                { "TranslateMessage", d => d.TranslateMessage = 1 },
                { "DispatchMessageA", d => d.DispatchMessageA = 1 },
                { "GetWindow", d => d.GetWindow = 1 },
                { "PeekMessageA", d => d.PeekMessageA = 1 },
                { "GetWindowLongA", d => d.GetWindowLongA = 1 },
                { "CallWindowProcA", d => d.CallWindowProcA = 1 },
                { "SetWindowLongA", d => d.SetWindowLongA = 1 },
                { "GetWindowTextA", d => d.GetWindowTextA = 1 },
                { "ScreenToClient", d => d.ScreenToClient = 1 },
                { "GetActiveWindow", d => d.GetActiveWindow = 1 },
                { "CreateWindowExA", d => d.CreateWindowExA = 1 },
                { "DefWindowProcA", d => d.DefWindowProcA = 1 },
                { "GetMessageA", d => d.GetMessageA = 1 },
                { "RegisterClassA", d => d.RegisterClassA = 1 },
                { "UnregisterClassA", d => d.UnregisterClassA = 1 },
                { "MessageBoxA", d => d.MessageBoxA = 1 },

                // ==================== COM 组件 ====================
                { "CoCreateInstance", d => d.CoCreateInstance = 1 },
                { "CoUninitialize", d => d.CoUninitialize = 1 },
                { "CoInitialize", d => d.CoInitialize = 1 },

                // ==================== GDI+ 图像 ====================
                { "GdipCreateBitmapFromHBITMAP", d => d.GdipCreateBitmapFromHBITMAP = 1 },
                { "GdipGetImageEncoders", d => d.GdipGetImageEncoders = 1 },
                { "GdipDisposeImage", d => d.GdipDisposeImage = 1 }
            };

        public static PEData ExtractFeatures(string filePath)
        {
            try
            {
                var data = new PEData();
                if (!PeFile.IsPeFile(filePath)) return null;
                var fileBytes = File.ReadAllBytes(filePath);
                var pe = new PeFile(fileBytes);
                var fileInfo = FileVersionInfo.GetVersionInfo(filePath);

                // 文件时间是否异常
                data.FileTimeException = IsTimeException(File.GetCreationTime(filePath)) || IsTimeException(File.GetLastWriteTime(filePath)) ? 1 : 0;

                // 计算文件熵
                data.FileEntropy = (float)CalculateEntropy(fileBytes);

                // 文件详细信息
                data.FileDescriptionLength = !string.IsNullOrEmpty(fileInfo.FileDescription) ? fileInfo.FileDescription.Length : 0;
                data.FileVersionLength = !string.IsNullOrEmpty(fileInfo.FileVersion) ? fileInfo.FileVersion.Length : 0;
                data.ProductNameLength = !string.IsNullOrEmpty(fileInfo.ProductName) ? fileInfo.ProductName.Length : 0;
                data.ProductVersionLength = !string.IsNullOrEmpty(fileInfo.ProductVersion) ? fileInfo.ProductVersion.Length : 0;
                data.CompanyNameLength = !string.IsNullOrEmpty(fileInfo.CompanyName) ? fileInfo.CompanyName.Length : 0;
                data.LegalCopyrightLength = !string.IsNullOrEmpty(fileInfo.LegalCopyright) ? fileInfo.LegalCopyright.Length : 0;
                data.SpecialBuildLength = !string.IsNullOrEmpty(fileInfo.SpecialBuild) ? fileInfo.SpecialBuild.Length : 0;
                data.PrivateBuildLength = !string.IsNullOrEmpty(fileInfo.PrivateBuild) ? fileInfo.PrivateBuild.Length : 0;
                data.CommentsLength = !string.IsNullOrEmpty(fileInfo.Comments) ? fileInfo.Comments.Length : 0;
                data.InternalNameLength = !string.IsNullOrEmpty(fileInfo.InternalName) ? fileInfo.InternalName.Length : 0;
                data.LegalTrademarksLength = !string.IsNullOrEmpty(fileInfo.LegalTrademarks) ? fileInfo.LegalTrademarks.Length : 0;

                data.IsDebug = fileInfo.IsDebug ? 1 : 0;
                data.IsPatched = fileInfo.IsPatched ? 1 : 0;
                data.IsPrivateBuild = fileInfo.IsPrivateBuild ? 1 : 0;
                data.IsPreRelease = fileInfo.IsPreRelease ? 1 : 0;
                data.IsSpecialBuild = fileInfo.IsSpecialBuild ? 1 : 0;

                if (pe.ImageDebugDirectory != null) data.DebugCount = pe.ImageDebugDirectory.Length;
                if (pe.Resources?.Icons != null) data.IconCount = pe.Resources.Icons.Length;

                if (pe.ImageTlsDirectory?.AddressOfCallBacks > 0) data.HasTlsCallbacks = 1;
                data.HasInvalidTimestamp = pe.ImageNtHeaders?.FileHeader.TimeDateStamp > DateTime.UtcNow.ToFileTime() ? 1 : 0;
                data.HasRelocationDirectory = pe.ImageNtHeaders?.OptionalHeader.DataDirectory[5].Size > 0 ? 1 : 0;

                // 节区特征
                if (pe.ImageSectionHeaders != null)
                {
                    foreach (var section in pe.ImageSectionHeaders)
                    {
                        // 是否加壳
                        if (PackedSectionNames.Contains(section.Name)) data.HasPacked = 1;
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
                            data.TextSection = (float)CalculateEntropy(sectionData);
                            data.TextSizeRatio = (float)sectionData.Length / pe.FileSize;
                        }
                        // 计算.data节数据熵
                        else if (section.Name.Equals(".data"))
                        {
                            ReadOnlySpan<byte> sectionData = fileBytes.AsSpan(
                                (int)section.PointerToRawData,
                                (int)section.SizeOfRawData
                            );
                            data.DataSection = (float)CalculateEntropy(sectionData);
                            data.DataSizeRatio = (float)sectionData.Length / pe.FileSize;
                        }
                        // 计算.rsrc节数据熵
                        else if (section.Name.Equals(".rsrc"))
                        {
                            ReadOnlySpan<byte> sectionData = fileBytes.AsSpan(
                                (int)section.PointerToRawData,
                                (int)section.SizeOfRawData
                            );
                            data.RsrcSection = (float)CalculateEntropy(sectionData.ToArray());
                            data.RsrcSizeRatio = (float)sectionData.Length / pe.FileSize;
                            if (data.IsAdmin != 1) data.IsAdmin = HasTargetStringInBytes(sectionData.ToArray(), "requireAdministrator", 1024) ? 1 : 0;
                            if (data.IsInstall != 1) data.IsInstall = HasTargetStringInBytes(sectionData.ToArray(), "Nullsoft.NSIS.exehead", 2048) ||
                                HasTargetStringInBytes(sectionData.ToArray(), "JR.Inno.Setup", 2048) ||
                                HasTargetStringInBytes(sectionData.ToArray(), "7-Zip.7zipInstall", 2048) ||
                                HasTargetStringInBytes(sectionData.ToArray(), "WinRAR SFX", 2048) ||
                                HasTargetStringInBytes(sectionData.ToArray(), "InstallShield", 2048) ||
                                HasTargetStringInBytes(sectionData.ToArray(), "Advanced Installer", 2048) ||
                                HasTargetStringInBytes(sectionData.ToArray(), "AutoIt v3", 2048) ||
                                HasTargetStringInBytes(sectionData.ToArray(), "Inno Setup", 2048) ||
                                HasTargetStringInBytes(sectionData.ToArray(), "7zS.sfx", 2048) ||
                                HasTargetStringInBytes(sectionData.ToArray(), "Squirrel", 2048)
                                ? 1 : 0;
                        }
                    }
                }

                // 导入函数
                if (pe.ImportedFunctions != null)
                {
                    data.ApiCount = pe.ImportedFunctions.Length;
                    foreach (var func in pe.ImportedFunctions)
                    {
                        if (!string.IsNullOrEmpty(func.Name) && ApiFieldSetters.TryGetValue(func.Name, out var setter))
                        {
                            setter(data);
                        }
                    }
                }

                // 导出函数
                if (pe.ExportedFunctions != null)
                {
                    data.ExportCount = pe.ExportedFunctions.Length;
                }

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
                    data.NumberOfSections = pe.ImageNtHeaders.FileHeader.NumberOfSections;
                    data.TimeDateStamp = pe.ImageNtHeaders.FileHeader.TimeDateStamp;
                    data.PointerToSymbolTable = pe.ImageNtHeaders.FileHeader.PointerToSymbolTable;
                    data.NumberOfSymbols = pe.ImageNtHeaders.FileHeader.NumberOfSymbols;
                    data.SizeOfOptionalHeader = pe.ImageNtHeaders.FileHeader.SizeOfOptionalHeader;
                    data.Characteristics = (float)pe.ImageNtHeaders.FileHeader.Characteristics;
                    data.Magic = (float)pe.ImageNtHeaders.OptionalHeader.Magic;
                    data.MajorLinkerVersion = pe.ImageNtHeaders.OptionalHeader.MajorLinkerVersion;
                    data.MinorLinkerVersion = pe.ImageNtHeaders.OptionalHeader.MinorLinkerVersion;
                    data.SizeOfCode = pe.ImageNtHeaders.OptionalHeader.SizeOfCode;
                    data.SizeOfInitializedData = pe.ImageNtHeaders.OptionalHeader.SizeOfInitializedData;
                    data.SizeOfUninitializedData = pe.ImageNtHeaders.OptionalHeader.SizeOfUninitializedData;
                    data.AddressOfEntryPoint = pe.ImageNtHeaders.OptionalHeader.AddressOfEntryPoint;
                    data.BaseOfCode = pe.ImageNtHeaders.OptionalHeader.BaseOfCode;
                    data.ImageBase = pe.ImageNtHeaders.OptionalHeader.ImageBase;
                    data.SectionAlignment = pe.ImageNtHeaders.OptionalHeader.SectionAlignment;
                    data.FileAlignment = pe.ImageNtHeaders.OptionalHeader.FileAlignment;
                    data.MajorOperatingSystemVersion = pe.ImageNtHeaders.OptionalHeader.MajorOperatingSystemVersion;
                    data.MinorOperatingSystemVersion = pe.ImageNtHeaders.OptionalHeader.MinorOperatingSystemVersion;
                    data.MajorImageVersion = pe.ImageNtHeaders.OptionalHeader.MajorImageVersion;
                    data.MinorImageVersion = pe.ImageNtHeaders.OptionalHeader.MinorImageVersion;
                    data.MajorSubsystemVersion = pe.ImageNtHeaders.OptionalHeader.MajorSubsystemVersion;
                    data.MinorSubsystemVersion = pe.ImageNtHeaders.OptionalHeader.MinorSubsystemVersion;
                    data.SizeOfImage = pe.ImageNtHeaders.OptionalHeader.SizeOfImage;
                    data.SizeOfHeaders = pe.ImageNtHeaders.OptionalHeader.SizeOfHeaders;
                    data.CheckSum = pe.ImageNtHeaders.OptionalHeader.CheckSum;
                    data.Subsystem = (float)pe.ImageNtHeaders.OptionalHeader.Subsystem;
                    data.DllCharacteristics = (float)pe.ImageNtHeaders.OptionalHeader.DllCharacteristics;
                    data.SizeOfStackReserve = pe.ImageNtHeaders.OptionalHeader.SizeOfStackReserve;
                    data.SizeOfStackCommit = pe.ImageNtHeaders.OptionalHeader.SizeOfStackCommit;
                    data.SizeOfHeapReserve = pe.ImageNtHeaders.OptionalHeader.SizeOfHeapReserve;
                    data.SizeOfHeapCommit = pe.ImageNtHeaders.OptionalHeader.SizeOfHeapCommit;
                    data.LoaderFlags = pe.ImageNtHeaders.OptionalHeader.LoaderFlags;
                    data.NumberOfRvaAndSizes = pe.ImageNtHeaders.OptionalHeader.NumberOfRvaAndSizes;
                }

                return data;
            }
            catch
            {
                return null;
            }
        }

        public static bool IsTimeException(DateTime time)
        {
            return time < new DateTime(1990, 1, 1) || time > DateTime.Now.AddMonths(1);
        }

        public static unsafe double CalculateEntropy(ReadOnlySpan<byte> data)
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
