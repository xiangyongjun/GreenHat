using Microsoft.ML.Data;

namespace GreenHat.Utils
{
    public class PEData
    {
        // ==================== ML.NET 训练列（LoadColumn 从 0 开始）====================

        /// <summary>
        /// 标签：是否为恶意文件 (0=良性，1=恶意) - SQL 第 5 列
        /// </summary>
        [LoadColumn(0)]
        public bool Label { get; set; }

        /// <summary>
        /// 预测概率 - SQL 第 6 列
        /// </summary>
        [LoadColumn(1)]
        public float Probability { get; set; }

        // ==================== PE 静态特征（LoadColumn 2-83）====================

        [LoadColumn(2)] public float FileEntropy { get; set; }
        [LoadColumn(3)] public float TextSection { get; set; }
        [LoadColumn(4)] public float TextSizeRatio { get; set; }
        [LoadColumn(5)] public float DataSection { get; set; }
        [LoadColumn(6)] public float DataSizeRatio { get; set; }
        [LoadColumn(7)] public float RsrcSection { get; set; }
        [LoadColumn(8)] public float RsrcSizeRatio { get; set; }
        [LoadColumn(9)] public float IsExe { get; set; }
        [LoadColumn(10)] public float IsDll { get; set; }
        [LoadColumn(11)] public float IsDriver { get; set; }
        [LoadColumn(12)] public float Is64Bit { get; set; }
        [LoadColumn(13)] public float IsDebug { get; set; }
        [LoadColumn(14)] public float SpecialBuildLength { get; set; }
        [LoadColumn(15)] public float PrivateBuildLength { get; set; }
        [LoadColumn(16)] public float TrustSigned { get; set; }
        [LoadColumn(17)] public float ExceptionCount { get; set; }
        [LoadColumn(18)] public float Machine { get; set; }
        [LoadColumn(19)] public float NumberOfSections { get; set; }
        [LoadColumn(20)] public float TimeDateStamp { get; set; }
        [LoadColumn(21)] public float PointerToSymbolTable { get; set; }
        [LoadColumn(22)] public float NumberOfSymbols { get; set; }
        [LoadColumn(23)] public float SizeOfOptionalHeader { get; set; }
        [LoadColumn(24)] public float Characteristics { get; set; }
        [LoadColumn(25)] public float Magic { get; set; }
        [LoadColumn(26)] public float MajorLinkerVersion { get; set; }
        [LoadColumn(27)] public float MinorLinkerVersion { get; set; }
        [LoadColumn(28)] public float SizeOfCode { get; set; }
        [LoadColumn(29)] public float SizeOfInitializedData { get; set; }
        [LoadColumn(30)] public float SizeOfUninitializedData { get; set; }
        [LoadColumn(31)] public float AddressOfEntryPoint { get; set; }
        [LoadColumn(32)] public float BaseOfCode { get; set; }
        [LoadColumn(33)] public float ImageBase { get; set; }
        [LoadColumn(34)] public float SectionAlignment { get; set; }
        [LoadColumn(35)] public float FileAlignment { get; set; }
        [LoadColumn(36)] public float MajorOperatingSystemVersion { get; set; }
        [LoadColumn(37)] public float MinorOperatingSystemVersion { get; set; }
        [LoadColumn(38)] public float MajorImageVersion { get; set; }
        [LoadColumn(39)] public float MinorImageVersion { get; set; }
        [LoadColumn(40)] public float MajorSubsystemVersion { get; set; }
        [LoadColumn(41)] public float MinorSubsystemVersion { get; set; }
        [LoadColumn(42)] public float SizeOfImage { get; set; }
        [LoadColumn(43)] public float SizeOfHeaders { get; set; }
        [LoadColumn(44)] public float CheckSum { get; set; }
        [LoadColumn(45)] public float Subsystem { get; set; }
        [LoadColumn(46)] public float DllCharacteristics { get; set; }
        [LoadColumn(47)] public float SizeOfStackReserve { get; set; }
        [LoadColumn(48)] public float SizeOfStackCommit { get; set; }
        [LoadColumn(49)] public float SizeOfHeapReserve { get; set; }
        [LoadColumn(50)] public float SizeOfHeapCommit { get; set; }
        [LoadColumn(51)] public float LoaderFlags { get; set; }
        [LoadColumn(52)] public float NumberOfRvaAndSizes { get; set; }
        [LoadColumn(53)] public float ApiCount { get; set; }
        [LoadColumn(54)] public float ExportCount { get; set; }
        [LoadColumn(55)] public float FileDescriptionLength { get; set; }
        [LoadColumn(56)] public float FileVersionLength { get; set; }
        [LoadColumn(57)] public float ProductNameLength { get; set; }
        [LoadColumn(58)] public float ProductVersionLength { get; set; }
        [LoadColumn(59)] public float CompanyNameLength { get; set; }
        [LoadColumn(60)] public float LegalCopyrightLength { get; set; }
        [LoadColumn(61)] public float CommentsLength { get; set; }
        [LoadColumn(62)] public float InternalNameLength { get; set; }
        [LoadColumn(63)] public float LegalTrademarksLength { get; set; }
        [LoadColumn(64)] public float ExecutableSections { get; set; }
        [LoadColumn(65)] public float WritableSections { get; set; }
        [LoadColumn(66)] public float ReadableSections { get; set; }
        [LoadColumn(67)] public float SectionCount { get; set; }
        [LoadColumn(68)] public float SectionException { get; set; }
        [LoadColumn(69)] public float DebugCount { get; set; }
        [LoadColumn(70)] public float IsPatched { get; set; }
        [LoadColumn(71)] public float IsPrivateBuild { get; set; }
        [LoadColumn(72)] public float IsPreRelease { get; set; }
        [LoadColumn(73)] public float IsSpecialBuild { get; set; }
        [LoadColumn(74)] public float IconCount { get; set; }
        [LoadColumn(75)] public float IsAdmin { get; set; }
        [LoadColumn(76)] public float IsInstall { get; set; }
        [LoadColumn(77)] public float HasTlsCallbacks { get; set; }
        [LoadColumn(78)] public float HasInvalidTimestamp { get; set; }
        [LoadColumn(79)] public float HasRelocationDirectory { get; set; }
        [LoadColumn(80)] public float HasPacked { get; set; }
        [LoadColumn(81)] public float FileTimeException { get; set; }
        [LoadColumn(82)] public float CorExeMain { get; set; }
        [LoadColumn(83)] public float CorDllMain { get; set; }

        // ==================== API 特征 - 进程创建与控制（LoadColumn 84-103）====================

        [LoadColumn(84)] public float CreateProcessA { get; set; }
        [LoadColumn(85)] public float CreateProcessW { get; set; }
        [LoadColumn(86)] public float WinExec { get; set; }
        [LoadColumn(87)] public float ShellExecuteA { get; set; }
        [LoadColumn(88)] public float ShellExecuteW { get; set; }
        [LoadColumn(89)] public float ShellExecuteExW { get; set; }
        [LoadColumn(90)] public float ExitProcess { get; set; }
        [LoadColumn(91)] public float TerminateProcess { get; set; }
        [LoadColumn(92)] public float OpenProcess { get; set; }
        [LoadColumn(93)] public float GetExitCodeProcess { get; set; }
        [LoadColumn(94)] public float SetThreadPriority { get; set; }
        [LoadColumn(95)] public float GetThreadPriority { get; set; }
        [LoadColumn(96)] public float GetCurrentProcess { get; set; }
        [LoadColumn(97)] public float GetCurrentProcessId { get; set; }
        [LoadColumn(98)] public float GetModuleFileNameA { get; set; }
        [LoadColumn(99)] public float GetCommandLineA { get; set; }
        [LoadColumn(100)] public float GetModuleFileNameW { get; set; }
        [LoadColumn(101)] public float GetStartupInfoW { get; set; }

        // ==================== API 特征 - 内存操作（LoadColumn 102-115）====================

        [LoadColumn(102)] public float VirtualAlloc { get; set; }
        [LoadColumn(103)] public float VirtualAllocEx { get; set; }
        [LoadColumn(104)] public float VirtualProtect { get; set; }
        [LoadColumn(105)] public float VirtualProtectEx { get; set; }
        [LoadColumn(106)] public float WriteProcessMemory { get; set; }
        [LoadColumn(107)] public float ReadProcessMemory { get; set; }
        [LoadColumn(108)] public float CreateRemoteThread { get; set; }
        [LoadColumn(109)] public float QueueUserAPC { get; set; }
        [LoadColumn(110)] public float SetThreadContext { get; set; }
        [LoadColumn(111)] public float GetThreadContext { get; set; }
        [LoadColumn(112)] public float Wow64SetThreadContext { get; set; }
        [LoadColumn(113)] public float NtUnmapViewOfSection { get; set; }
        [LoadColumn(114)] public float ZwUnmapViewOfSection { get; set; }
        [LoadColumn(115)] public float RtlCreateUserThread { get; set; }

        // ==================== API 特征 - 钩子与注入（LoadColumn 116-118）====================

        [LoadColumn(116)] public float SetWindowsHookExA { get; set; }
        [LoadColumn(117)] public float SetWindowsHookExW { get; set; }
        [LoadColumn(118)] public float UnhookWindowsHookEx { get; set; }

        // ==================== API 特征 - DLL 加载（LoadColumn 119-126）====================

        [LoadColumn(119)] public float LoadLibraryA { get; set; }
        [LoadColumn(120)] public float LoadLibraryW { get; set; }
        [LoadColumn(121)] public float LoadLibraryExA { get; set; }
        [LoadColumn(122)] public float LoadLibraryExW { get; set; }
        [LoadColumn(123)] public float GetProcAddress { get; set; }
        [LoadColumn(124)] public float GetModuleHandleA { get; set; }
        [LoadColumn(125)] public float GetModuleHandleW { get; set; }
        [LoadColumn(126)] public float FreeLibrary { get; set; }

        // ==================== API 特征 - 文件映射（LoadColumn 127-131）====================

        [LoadColumn(127)] public float CreateFileMappingA { get; set; }
        [LoadColumn(128)] public float CreateFileMappingW { get; set; }
        [LoadColumn(129)] public float MapViewOfFile { get; set; }
        [LoadColumn(130)] public float VirtualFree { get; set; }
        [LoadColumn(131)] public float VirtualQuery { get; set; }

        // ==================== API 特征 - 同步对象（LoadColumn 132-170）====================

        [LoadColumn(132)] public float WaitForSingleObject { get; set; }
        [LoadColumn(133)] public float WaitForSingleObjectEx { get; set; }
        [LoadColumn(134)] public float WaitForMultipleObjects { get; set; }
        [LoadColumn(135)] public float WaitForMultipleObjectsEx { get; set; }
        [LoadColumn(136)] public float CreateMutexA { get; set; }
        [LoadColumn(137)] public float CreateMutexW { get; set; }
        [LoadColumn(138)] public float OpenMutexW { get; set; }
        [LoadColumn(139)] public float ReleaseMutex { get; set; }
        [LoadColumn(140)] public float CreateEventA { get; set; }
        [LoadColumn(141)] public float CreateEventW { get; set; }
        [LoadColumn(142)] public float OpenEventW { get; set; }
        [LoadColumn(143)] public float SetEvent { get; set; }
        [LoadColumn(144)] public float ResetEvent { get; set; }
        [LoadColumn(145)] public float EnterCriticalSection { get; set; }
        [LoadColumn(146)] public float LeaveCriticalSection { get; set; }
        [LoadColumn(147)] public float InitializeCriticalSection { get; set; }
        [LoadColumn(148)] public float DeleteCriticalSection { get; set; }
        [LoadColumn(149)] public float Sleep { get; set; }
        [LoadColumn(150)] public float SleepEx { get; set; }
        [LoadColumn(151)] public float InitializeCriticalSectionAndSpinCount { get; set; }
        [LoadColumn(152)] public float InterlockedDecrement { get; set; }
        [LoadColumn(153)] public float InterlockedIncrement { get; set; }
        [LoadColumn(154)] public float InterlockedExchange { get; set; }
        [LoadColumn(155)] public float InterlockedCompareExchange { get; set; }
        [LoadColumn(156)] public float SetTimer { get; set; }
        [LoadColumn(157)] public float KillTimer { get; set; }
        [LoadColumn(158)] public float CreateThread { get; set; }
        [LoadColumn(159)] public float ResumeThread { get; set; }
        [LoadColumn(160)] public float SuspendThread { get; set; }
        [LoadColumn(161)] public float ExitThread { get; set; }
        [LoadColumn(162)] public float TerminateThread { get; set; }
        [LoadColumn(163)] public float GetCurrentThread { get; set; }
        [LoadColumn(164)] public float GetCurrentThreadId { get; set; }
        [LoadColumn(165)] public float TlsAlloc { get; set; }
        [LoadColumn(166)] public float TlsSetValue { get; set; }
        [LoadColumn(167)] public float TlsGetValue { get; set; }
        [LoadColumn(168)] public float CreateThreadpoolWork { get; set; }
        [LoadColumn(169)] public float SubmitThreadpoolWork { get; set; }
        [LoadColumn(170)] public float TlsFree { get; set; }

        // ==================== API 特征 - 网络 Socket（LoadColumn 171-186）====================

        [LoadColumn(171)] public float Socket { get; set; }
        [LoadColumn(172)] public float Connect { get; set; }
        [LoadColumn(173)] public float Send { get; set; }
        [LoadColumn(174)] public float Recv { get; set; }
        [LoadColumn(175)] public float Bind { get; set; }
        [LoadColumn(176)] public float Listen { get; set; }
        [LoadColumn(177)] public float Accept { get; set; }
        [LoadColumn(178)] public float Gethostbyname { get; set; }
        [LoadColumn(179)] public float Getaddrinfo { get; set; }
        [LoadColumn(180)] public float WSAStartup { get; set; }
        [LoadColumn(181)] public float WSACleanup { get; set; }
        [LoadColumn(182)] public float WSAIoctl { get; set; }
        [LoadColumn(183)] public float WSASocketA { get; set; }
        [LoadColumn(184)] public float WSASocketW { get; set; }
        [LoadColumn(185)] public float Closesocket { get; set; }
        [LoadColumn(186)] public float Htons { get; set; }

        // ==================== API 特征 - 网络 WinINet（LoadColumn 187-201）====================

        [LoadColumn(187)] public float InternetOpenA { get; set; }
        [LoadColumn(188)] public float InternetOpenW { get; set; }
        [LoadColumn(189)] public float InternetConnectA { get; set; }
        [LoadColumn(190)] public float InternetConnectW { get; set; }
        [LoadColumn(191)] public float InternetOpenUrlA { get; set; }
        [LoadColumn(192)] public float InternetOpenUrlW { get; set; }
        [LoadColumn(193)] public float HttpOpenRequestA { get; set; }
        [LoadColumn(194)] public float HttpOpenRequestW { get; set; }
        [LoadColumn(195)] public float HttpSendRequestA { get; set; }
        [LoadColumn(196)] public float HttpSendRequestW { get; set; }
        [LoadColumn(197)] public float InternetReadFile { get; set; }
        [LoadColumn(198)] public float URLDownloadToFileA { get; set; }
        [LoadColumn(199)] public float URLDownloadToFileW { get; set; }
        [LoadColumn(200)] public float DnsQueryA { get; set; }
        [LoadColumn(201)] public float DnsQueryW { get; set; }

        // ==================== API 特征 - 加密 CryptoAPI（LoadColumn 202-220）====================

        [LoadColumn(202)] public float CryptAcquireContextA { get; set; }
        [LoadColumn(203)] public float CryptAcquireContextW { get; set; }
        [LoadColumn(204)] public float CryptCreateHash { get; set; }
        [LoadColumn(205)] public float CryptHashData { get; set; }
        [LoadColumn(206)] public float CryptDeriveKey { get; set; }
        [LoadColumn(207)] public float CryptEncrypt { get; set; }
        [LoadColumn(208)] public float CryptDecrypt { get; set; }
        [LoadColumn(209)] public float CryptDestroyKey { get; set; }
        [LoadColumn(210)] public float CryptDestroyHash { get; set; }
        [LoadColumn(211)] public float CryptReleaseContext { get; set; }
        [LoadColumn(212)] public float CryptGenKey { get; set; }
        [LoadColumn(213)] public float CryptImportKey { get; set; }
        [LoadColumn(214)] public float CryptExportKey { get; set; }
        [LoadColumn(215)] public float CryptSignHash { get; set; }
        [LoadColumn(216)] public float CryptVerifySignature { get; set; }
        [LoadColumn(217)] public float CryptSetKeyParam { get; set; }
        [LoadColumn(218)] public float CryptGetKeyParam { get; set; }
        [LoadColumn(219)] public float CryptSetHashParam { get; set; }
        [LoadColumn(220)] public float CryptGetHashParam { get; set; }

        // ==================== API 特征 - 其他工具函数（LoadColumn 221-233）====================

        [LoadColumn(221)] public float RtlDecompressBuffer { get; set; }
        [LoadColumn(222)] public float MultiByteToWideChar { get; set; }
        [LoadColumn(223)] public float WideCharToMultiByte { get; set; }
        [LoadColumn(224)] public float Base64Decode { get; set; }
        [LoadColumn(225)] public float CryptDecodeObject { get; set; }
        [LoadColumn(226)] public float IsDBCSLeadByte { get; set; }
        [LoadColumn(227)] public float CharUpperA { get; set; }
        [LoadColumn(228)] public float CharLowerA { get; set; }
        [LoadColumn(229)] public float GetStringTypeW { get; set; }
        [LoadColumn(230)] public float LCMapStringW { get; set; }
        [LoadColumn(231)] public float IsValidCodePage { get; set; }
        [LoadColumn(232)] public float DecodePointer { get; set; }
        [LoadColumn(233)] public float EncodePointer { get; set; }

        // ==================== API 特征 - 文件操作（LoadColumn 234-268）====================

        [LoadColumn(234)] public float CreateFileA { get; set; }
        [LoadColumn(235)] public float CreateFileW { get; set; }
        [LoadColumn(236)] public float WriteFile { get; set; }
        [LoadColumn(237)] public float ReadFile { get; set; }
        [LoadColumn(238)] public float DeleteFileA { get; set; }
        [LoadColumn(239)] public float DeleteFileW { get; set; }
        [LoadColumn(240)] public float CopyFileA { get; set; }
        [LoadColumn(241)] public float CopyFileW { get; set; }
        [LoadColumn(242)] public float MoveFileA { get; set; }
        [LoadColumn(243)] public float MoveFileW { get; set; }
        [LoadColumn(244)] public float FindFirstFileA { get; set; }
        [LoadColumn(245)] public float FindNextFileA { get; set; }
        [LoadColumn(246)] public float GetTempPathA { get; set; }
        [LoadColumn(247)] public float GetTempPathW { get; set; }
        [LoadColumn(248)] public float GetTempFileNameA { get; set; }
        [LoadColumn(249)] public float GetTempFileNameW { get; set; }
        [LoadColumn(250)] public float SetFileAttributesA { get; set; }
        [LoadColumn(251)] public float SetFileAttributesW { get; set; }
        [LoadColumn(252)] public float DeviceIoControl { get; set; }
        [LoadColumn(253)] public float SetFileTime { get; set; }
        [LoadColumn(254)] public float GetFileSize { get; set; }
        [LoadColumn(255)] public float GetFileSizeEx { get; set; }
        [LoadColumn(256)] public float SetFilePointer { get; set; }
        [LoadColumn(257)] public float FlushFileBuffers { get; set; }
        [LoadColumn(258)] public float ConnectNamedPipe { get; set; }
        [LoadColumn(259)] public float PeekNamedPipe { get; set; }
        [LoadColumn(260)] public float CloseHandle { get; set; }
        [LoadColumn(261)] public float GetFileType { get; set; }
        [LoadColumn(262)] public float SetStdHandle { get; set; }
        [LoadColumn(263)] public float SetFilePointerEx { get; set; }
        [LoadColumn(264)] public float FindClose { get; set; }
        [LoadColumn(265)] public float SetHandleCount { get; set; }
        [LoadColumn(266)] public float SetEndOfFile { get; set; }
        [LoadColumn(267)] public float GetFullPathNameA { get; set; }
        [LoadColumn(268)] public float GetFileAttributesA { get; set; }

        // ==================== API 特征 - 注册表（LoadColumn 269-283）====================

        [LoadColumn(269)] public float RegOpenKeyExA { get; set; }
        [LoadColumn(270)] public float RegOpenKeyExW { get; set; }
        [LoadColumn(271)] public float RegSetValueExA { get; set; }
        [LoadColumn(272)] public float RegSetValueExW { get; set; }
        [LoadColumn(273)] public float RegCreateKeyExA { get; set; }
        [LoadColumn(274)] public float RegCreateKeyExW { get; set; }
        [LoadColumn(275)] public float RegDeleteKeyA { get; set; }
        [LoadColumn(276)] public float RegDeleteKeyW { get; set; }
        [LoadColumn(277)] public float RegEnumValueA { get; set; }
        [LoadColumn(278)] public float RegEnumValueW { get; set; }
        [LoadColumn(279)] public float RegQueryValueExA { get; set; }
        [LoadColumn(280)] public float RegQueryValueExW { get; set; }
        [LoadColumn(281)] public float RegDeleteValueA { get; set; }
        [LoadColumn(282)] public float RegDeleteValueW { get; set; }
        [LoadColumn(283)] public float RegCloseKey { get; set; }

        // ==================== API 特征 - 服务管理（LoadColumn 284-291）====================

        [LoadColumn(284)] public float OpenSCManagerA { get; set; }
        [LoadColumn(285)] public float OpenSCManagerW { get; set; }
        [LoadColumn(286)] public float CreateServiceA { get; set; }
        [LoadColumn(287)] public float CreateServiceW { get; set; }
        [LoadColumn(288)] public float StartServiceA { get; set; }
        [LoadColumn(289)] public float StartServiceW { get; set; }
        [LoadColumn(290)] public float ControlService { get; set; }
        [LoadColumn(291)] public float DeleteService { get; set; }

        // ==================== API 特征 - 权限与用户（LoadColumn 292-299）====================

        [LoadColumn(292)] public float AdjustTokenPrivileges { get; set; }
        [LoadColumn(293)] public float LookupPrivilegeValueA { get; set; }
        [LoadColumn(294)] public float LookupPrivilegeValueW { get; set; }
        [LoadColumn(295)] public float OpenProcessToken { get; set; }
        [LoadColumn(296)] public float NetUserAdd { get; set; }
        [LoadColumn(297)] public float NetLocalGroupAddMembers { get; set; }
        [LoadColumn(298)] public float GetUserNameA { get; set; }
        [LoadColumn(299)] public float GetUserNameW { get; set; }

        // ==================== API 特征 - 异常处理（LoadColumn 300-307）====================

        [LoadColumn(300)] public float RtlUnwind { get; set; }
        [LoadColumn(301)] public float RtlVirtualUnwind { get; set; }
        [LoadColumn(302)] public float RtlCaptureContext { get; set; }
        [LoadColumn(303)] public float RtlLookupFunctionEntry { get; set; }
        [LoadColumn(304)] public float NtClose { get; set; }
        [LoadColumn(305)] public float NtQueryInformationProcess { get; set; }
        [LoadColumn(306)] public float RtlAllocateHeap { get; set; }
        [LoadColumn(307)] public float RtlFreeHeap { get; set; }

        // ==================== API 特征 - 系统信息（LoadColumn 308-324）====================

        [LoadColumn(308)] public float GetSystemTimeAsFileTime { get; set; }
        [LoadColumn(309)] public float GetLocalTime { get; set; }
        [LoadColumn(310)] public float GlobalMemoryStatus { get; set; }
        [LoadColumn(311)] public float GetVersionExA { get; set; }
        [LoadColumn(312)] public float GetVersionExW { get; set; }
        [LoadColumn(313)] public float GetComputerNameA { get; set; }
        [LoadColumn(314)] public float GetComputerNameW { get; set; }
        [LoadColumn(315)] public float GetLastError { get; set; }
        [LoadColumn(316)] public float GetACP { get; set; }
        [LoadColumn(317)] public float RaiseException { get; set; }
        [LoadColumn(318)] public float SetLastError { get; set; }
        [LoadColumn(319)] public float GetEnvironmentStringsW { get; set; }
        [LoadColumn(320)] public float FreeEnvironmentStringsW { get; set; }
        [LoadColumn(321)] public float GetLocaleInfoA { get; set; }
        [LoadColumn(322)] public float LstrlenA { get; set; }
        [LoadColumn(323)] public float GetVersion { get; set; }
        [LoadColumn(324)] public float GetSystemInfo { get; set; }

        // ==================== API 特征 - 反调试与其他（LoadColumn 325-354）====================

        [LoadColumn(325)] public float CompareStringA { get; set; }
        [LoadColumn(326)] public float IsDebuggerPresent { get; set; }
        [LoadColumn(327)] public float CheckRemoteDebuggerPresent { get; set; }
        [LoadColumn(328)] public float OutputDebugStringA { get; set; }
        [LoadColumn(329)] public float OutputDebugStringW { get; set; }
        [LoadColumn(330)] public float GetTickCount { get; set; }
        [LoadColumn(331)] public float GetTickCount64 { get; set; }
        [LoadColumn(332)] public float QueryPerformanceCounter { get; set; }
        [LoadColumn(333)] public float FindWindowA { get; set; }
        [LoadColumn(334)] public float FindWindowW { get; set; }
        [LoadColumn(335)] public float EnumWindows { get; set; }
        [LoadColumn(336)] public float EnumChildWindows { get; set; }
        [LoadColumn(337)] public float GetWindowRect { get; set; }
        [LoadColumn(338)] public float GetClientRect { get; set; }
        [LoadColumn(339)] public float SetWindowDisplayAffinity { get; set; }
        [LoadColumn(340)] public float UnhandledExceptionFilter { get; set; }
        [LoadColumn(341)] public float SetUnhandledExceptionFilter { get; set; }
        [LoadColumn(342)] public float IsProcessorFeaturePresent { get; set; }
        [LoadColumn(343)] public float SetErrorMode { get; set; }
        [LoadColumn(344)] public float GetTimeZoneInformation { get; set; }
        [LoadColumn(345)] public float GetDriveTypeW { get; set; }
        [LoadColumn(346)] public float GetDiskFreeSpaceA { get; set; }
        [LoadColumn(347)] public float GetAsyncKeyState { get; set; }
        [LoadColumn(348)] public float GetKeyState { get; set; }
        [LoadColumn(349)] public float GetKeyboardState { get; set; }
        [LoadColumn(350)] public float MapVirtualKeyA { get; set; }
        [LoadColumn(351)] public float MapVirtualKeyW { get; set; }
        [LoadColumn(352)] public float ToAscii { get; set; }
        [LoadColumn(353)] public float ToAsciiEx { get; set; }
        [LoadColumn(354)] public float ToUnicode { get; set; }

        // ==================== API 特征 - 输入与图形（LoadColumn 355-380）====================

        [LoadColumn(355)] public float ToUnicodeEx { get; set; }
        [LoadColumn(356)] public float GetKeyNameTextA { get; set; }
        [LoadColumn(357)] public float GetForegroundWindow { get; set; }
        [LoadColumn(358)] public float KeybdEvent { get; set; }
        [LoadColumn(359)] public float SendInput { get; set; }
        [LoadColumn(360)] public float MapVirtualKeyExA { get; set; }
        [LoadColumn(361)] public float MapVirtualKeyExW { get; set; }
        [LoadColumn(362)] public float CallNextHookEx { get; set; }
        [LoadColumn(363)] public float GetCursorPos { get; set; }
        [LoadColumn(364)] public float SetCursorPos { get; set; }
        [LoadColumn(365)] public float MouseEvent { get; set; }
        [LoadColumn(366)] public float GetDoubleClickTime { get; set; }
        [LoadColumn(367)] public float GetCapture { get; set; }
        [LoadColumn(368)] public float SetCapture { get; set; }
        [LoadColumn(369)] public float GetDC { get; set; }
        [LoadColumn(370)] public float GetWindowDC { get; set; }
        [LoadColumn(371)] public float CreateCompatibleDC { get; set; }
        [LoadColumn(372)] public float CreateCompatibleBitmap { get; set; }
        [LoadColumn(373)] public float BitBlt { get; set; }
        [LoadColumn(374)] public float StretchBlt { get; set; }
        [LoadColumn(375)] public float GdipSaveImageToFile { get; set; }
        [LoadColumn(376)] public float PrintWindow { get; set; }
        [LoadColumn(377)] public float GetDesktopWindow { get; set; }
        [LoadColumn(378)] public float CreateDCW { get; set; }
        [LoadColumn(379)] public float CreateDCA { get; set; }
        [LoadColumn(380)] public float SelectObject { get; set; }

        // ==================== API 特征 - 图形与音频（LoadColumn 381-410）====================

        [LoadColumn(381)] public float DeleteDC { get; set; }
        [LoadColumn(382)] public float DeleteObject { get; set; }
        [LoadColumn(383)] public float GetDeviceCaps { get; set; }
        [LoadColumn(384)] public float GetSystemMetrics { get; set; }
        [LoadColumn(385)] public float Direct3DCreate9 { get; set; }
        [LoadColumn(386)] public float D3D11CreateDevice { get; set; }
        [LoadColumn(387)] public float WaveInOpen { get; set; }
        [LoadColumn(388)] public float WaveInClose { get; set; }
        [LoadColumn(389)] public float WaveInStart { get; set; }
        [LoadColumn(390)] public float WaveInStop { get; set; }
        [LoadColumn(391)] public float OpenClipboard { get; set; }
        [LoadColumn(392)] public float CloseClipboard { get; set; }
        [LoadColumn(393)] public float GetClipboardData { get; set; }
        [LoadColumn(394)] public float SetClipboardData { get; set; }
        [LoadColumn(395)] public float CapCreateCaptureWindowA { get; set; }
        [LoadColumn(396)] public float CapCreateCaptureWindowW { get; set; }
        [LoadColumn(397)] public float HeapAlloc { get; set; }
        [LoadColumn(398)] public float HeapFree { get; set; }
        [LoadColumn(399)] public float HeapSize { get; set; }
        [LoadColumn(400)] public float HeapCreate { get; set; }
        [LoadColumn(401)] public float GetProcessHeap { get; set; }
        [LoadColumn(402)] public float LocalAlloc { get; set; }
        [LoadColumn(403)] public float LocalFree { get; set; }
        [LoadColumn(404)] public float GlobalAlloc { get; set; }
        [LoadColumn(405)] public float GlobalFree { get; set; }
        [LoadColumn(406)] public float GlobalUnlock { get; set; }
        [LoadColumn(407)] public float HeapDestroy { get; set; }
        [LoadColumn(408)] public float GlobalLock { get; set; }
        [LoadColumn(409)] public float SysFreeString { get; set; }
        [LoadColumn(410)] public float SysAllocStringLen { get; set; }

        // ==================== API 特征 - 资源与窗口（LoadColumn 411-440）====================

        [LoadColumn(411)] public float LoadResource { get; set; }
        [LoadColumn(412)] public float SizeofResource { get; set; }
        [LoadColumn(413)] public float LockResource { get; set; }
        [LoadColumn(414)] public float FreeResource { get; set; }
        [LoadColumn(415)] public float FindResourceA { get; set; }
        [LoadColumn(416)] public float ShowWindow { get; set; }
        [LoadColumn(417)] public float DestroyWindow { get; set; }
        [LoadColumn(418)] public float TranslateMessage { get; set; }
        [LoadColumn(419)] public float DispatchMessageA { get; set; }
        [LoadColumn(420)] public float GetWindow { get; set; }
        [LoadColumn(421)] public float PeekMessageA { get; set; }
        [LoadColumn(422)] public float GetWindowLongA { get; set; }
        [LoadColumn(423)] public float CallWindowProcA { get; set; }
        [LoadColumn(424)] public float SetWindowLongA { get; set; }
        [LoadColumn(425)] public float GetWindowTextA { get; set; }
        [LoadColumn(426)] public float ScreenToClient { get; set; }
        [LoadColumn(427)] public float GetActiveWindow { get; set; }
        [LoadColumn(428)] public float CreateWindowExA { get; set; }
        [LoadColumn(429)] public float DefWindowProcA { get; set; }
        [LoadColumn(430)] public float GetMessageA { get; set; }
        [LoadColumn(431)] public float RegisterClassA { get; set; }
        [LoadColumn(432)] public float UnregisterClassA { get; set; }
        [LoadColumn(433)] public float MessageBoxA { get; set; }
        [LoadColumn(434)] public float CoCreateInstance { get; set; }
        [LoadColumn(435)] public float CoUninitialize { get; set; }
        [LoadColumn(436)] public float CoInitialize { get; set; }
        [LoadColumn(437)] public float EmptyClipboard { get; set; }
        [LoadColumn(438)] public float IsClipboardFormatAvailable { get; set; }
        [LoadColumn(439)] public float WaveInPrepareHeader { get; set; }
        [LoadColumn(440)] public float WaveInUnprepareHeader { get; set; }

        // ==================== API 特征 - 音频与 COM（LoadColumn 441-450）====================

        [LoadColumn(441)] public float WaveInAddBuffer { get; set; }
        [LoadColumn(442)] public float WaveOutOpen { get; set; }
        [LoadColumn(443)] public float WaveOutWrite { get; set; }
        [LoadColumn(444)] public float WaveInGetNumDevs { get; set; }
        [LoadColumn(445)] public float WaveInGetDevCapsA { get; set; }
        [LoadColumn(446)] public float WaveInGetDevCapsW { get; set; }
        [LoadColumn(447)] public float IAudioSessionManager2 { get; set; }
        [LoadColumn(448)] public float IAudioSessionControl { get; set; }
        [LoadColumn(449)] public float IAudioClient { get; set; }
        [LoadColumn(450)] public float IMMDeviceEnumerator { get; set; }

        // ==================== API 特征 - 输入法与媒体（LoadColumn 451-458）====================

        [LoadColumn(451)] public float GetKeyboardLayout { get; set; }
        [LoadColumn(452)] public float GetKeyboardLayoutList { get; set; }
        [LoadColumn(453)] public float ActivateKeyboardLayout { get; set; }
        [LoadColumn(454)] public float ImmGetVirtualKey { get; set; }
        [LoadColumn(455)] public float ImmGetIMEFileName { get; set; }
        [LoadColumn(456)] public float LoopbackCapture { get; set; }
        [LoadColumn(457)] public float MFStartup { get; set; }
        [LoadColumn(458)] public float MFCreateSinkWriterFromURL { get; set; }

        // ==================== API 特征 - 媒体与 DirectX（LoadColumn 459-462）====================

        [LoadColumn(459)] public float MFCreateSourceReaderFromMediaSource { get; set; }
        [LoadColumn(460)] public float GdipCreateBitmapFromHBITMAP { get; set; }
        [LoadColumn(461)] public float GdipGetImageEncoders { get; set; }
        [LoadColumn(462)] public float GdipDisposeImage { get; set; }
        [LoadColumn(463)] public float IDirect3DDevice9Present { get; set; }
        [LoadColumn(464)] public float IDXGISwapChainPresent { get; set; }
    }

    // 定义预测结果类
    public class MalwarePrediction
    {
        [ColumnName("PredictedLabel")]
        public bool IsMalware { get; set; }

        public float Probability { get; set; }
    }
}