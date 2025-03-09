using System.Runtime.InteropServices;
using System;
using System.IO;

internal static class WinTrust
{
    // 定义 WinTrustFileInfo 结构体
    [StructLayout(LayoutKind.Sequential)]
    public struct WinTrustFileInfo
    {
        public uint cbStruct;
        public string pcwszFilePath;
        public IntPtr hFile;
        public IntPtr pgKnownSubject;
    }

    // 定义 WinTrustData 结构体
    [StructLayout(LayoutKind.Sequential)]
    public struct WinTrustData
    {
        public uint cbStruct;
        public IntPtr pPolicyCallbackData;
        public IntPtr pSIPClientData;
        public uint dwUIChoice;
        public uint fdwRevocationChecks;
        public uint dwUnionChoice;
        public IntPtr pFile;
        public uint dwStateAction;
        public IntPtr hWVTStateData;
        public string pwszURLReference;
        public uint dwProvFlags;
        public uint dwUIContext;
    }

    // 定义WinVerifyTrust函数所需的GUID
    private static readonly Guid WINTRUST_ACTION_GENERIC_VERIFY_V2 = new Guid("{00AAC56B-CD44-11d0-8CC2-00C04FC295EE}");

    // WinVerifyTrust函数的返回值
    private const int S_OK = 0; // 验证成功

    // WinVerifyTrust函数的参数结构体
    [StructLayout(LayoutKind.Sequential)]
    private struct WINTRUST_FILE_INFO
    {
        public uint cbStruct;
        public IntPtr pcwszFilePath;
        public IntPtr hFile;
        public IntPtr pgKnownSubject;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct WINTRUST_DATA
    {
        public uint cbStruct;
        public IntPtr pPolicyCallbackData;
        public IntPtr pSIPClientData;
        public uint dwUIChoice;         // 设置为2 (WTD_UI_NONE) 禁用UI
        public uint fdwRevocationChecks;// 设置为0 (WTD_REVOKE_NONE) 禁用吊销检查
        public uint dwUnionChoice;      // 设置为1 (WTD_CHOICE_FILE) 表示文件验证
        public IntPtr pFile;
        public IntPtr pCatalog;
        public IntPtr pBlob;
        public IntPtr pSgnr;
        public IntPtr pCert;
        public uint dwStateAction;      // 设置为0 (WTD_STATEACTION_VERIFY) 表示验证操作
        public IntPtr hWVTStateData;
        public IntPtr pwszURLReference;
        public uint dwProvFlags;        // 设置为0x00000080 (WTD_REVOCATION_CHECK_NONE) 禁用吊销检查
        public uint dwUIContext;
    }

    // 导入WinVerifyTrust函数
    [DllImport("wintrust.dll", SetLastError = true)]
    private static extern uint WinVerifyTrust(IntPtr hwnd, [MarshalAs(UnmanagedType.LPStruct)] Guid pgActionID, ref WINTRUST_DATA pWVTData);

    // 导入LocalFree函数用于释放内存
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr LocalFree(IntPtr hMem);

    // 导入 Wintrust.dll 和 Kernel32.dll 中的函数
    [DllImport("Wintrust.dll", SetLastError = true)]
    private static extern bool CryptCATAdminAcquireContext(ref IntPtr phCatAdmin, int pgSubsystem, uint dwFlags);

    [DllImport("Wintrust.dll", SetLastError = true)]
    private static extern bool CryptCATAdminReleaseContext(IntPtr hCatAdmin, uint dwFlags);

    [DllImport("Wintrust.dll", SetLastError = true)]
    private static extern bool CryptCATAdminCalcHashFromFileHandle(IntPtr hFile, ref uint pcbHash, byte[] pbHash, uint dwFlags);

    [DllImport("Wintrust.dll", SetLastError = true)]
    private static extern bool CryptCATAdminEnumCatalogFromHash(IntPtr hCatAdmin, byte[] pbHash, uint cbHash, uint dwFlags, ref IntPtr phPrevCatInfo);

    [DllImport("Wintrust.dll", SetLastError = true)]
    private static extern bool CryptCATAdminReleaseCatalogContext(IntPtr hCatAdmin, IntPtr hCatInfo, uint dwFlags);

    [DllImport("Kernel32.dll", SetLastError = true)]
    private static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

    [DllImport("Kernel32.dll", SetLastError = true)]
    private static extern bool CloseHandle(IntPtr hObject);

    // 常量定义
    private const uint GENERIC_READ = 0x80000000;
    private const uint FILE_SHARE_READ = 0x00000001;
    private const uint OPEN_EXISTING = 3;
    private const uint FILE_ATTRIBUTE_NORMAL = 0x80;

    // 检查文件是否具有目录签名
    public static bool VerifyDirSignature(string filePath)
    {
        IntPtr hFile = CreateFile(filePath, GENERIC_READ, FILE_SHARE_READ, IntPtr.Zero, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);
        if (hFile == new IntPtr(-1)) return false;

        try
        {
            uint cbHash = 0;
            if (!CryptCATAdminCalcHashFromFileHandle(hFile, ref cbHash, null, 0)) return false;

            byte[] pbHash = new byte[cbHash];
            if (!CryptCATAdminCalcHashFromFileHandle(hFile, ref cbHash, pbHash, 0)) return false;

            IntPtr hCatAdmin = IntPtr.Zero;

            if (!CryptCATAdminAcquireContext(ref hCatAdmin, 0, 0)) return false;

            try
            {
                IntPtr hPrevCatInfo = IntPtr.Zero;
                bool hasSignature = CryptCATAdminEnumCatalogFromHash(hCatAdmin, pbHash, cbHash, 0, ref hPrevCatInfo);
                if (hasSignature) CryptCATAdminReleaseCatalogContext(hCatAdmin, hPrevCatInfo, 0);
                return hasSignature;
            }
            finally
            {
                CryptCATAdminReleaseContext(hCatAdmin, 0);
            }
        }
        finally
        {
            CloseHandle(hFile);
        }
    }

    // 验证文件签名的方法
    public static bool VerifyFileSignature(string filePath)
    {
        WINTRUST_FILE_INFO fileInfo = new WINTRUST_FILE_INFO();
        fileInfo.cbStruct = (uint)Marshal.SizeOf(typeof(WINTRUST_FILE_INFO));
        fileInfo.pcwszFilePath = Marshal.StringToCoTaskMemUni(filePath);

        WINTRUST_DATA winTrustData = new WINTRUST_DATA();
        winTrustData.cbStruct = (uint)Marshal.SizeOf(typeof(WINTRUST_DATA));
        winTrustData.dwUIChoice = 2; // WTD_UI_NONE: 禁用UI，确保没有弹窗
        winTrustData.fdwRevocationChecks = 0x00000001; // 启用完整证书链吊销检查
        winTrustData.dwUnionChoice = 1; // WTD_CHOICE_FILE: 验证文件
        winTrustData.pFile = Marshal.AllocCoTaskMem(Marshal.SizeOf(fileInfo));
        Marshal.StructureToPtr(fileInfo, winTrustData.pFile, false);
        winTrustData.dwStateAction = 0; // WTD_STATEACTION_VERIFY: 验证操作
        winTrustData.dwProvFlags = 0x00000080; // WTD_REVOCATION_CHECK_CHAIN_EXCLUDE_ROOT: 检查证书链但不包括根证书

        try
        {
            uint result = WinVerifyTrust(IntPtr.Zero, WINTRUST_ACTION_GENERIC_VERIFY_V2, ref winTrustData);

            return result == S_OK;
        }
        finally
        {
            if (fileInfo.pcwszFilePath != IntPtr.Zero)
                Marshal.FreeCoTaskMem(fileInfo.pcwszFilePath);

            if (winTrustData.pFile != IntPtr.Zero)
            {
                Marshal.DestroyStructure(winTrustData.pFile, typeof(WINTRUST_FILE_INFO));
                Marshal.FreeCoTaskMem(winTrustData.pFile);
            }
        }
    }

    public static bool VerifyFileSignature(byte[] data)
    {
        string path = Path.GetTempFileName();
        File.WriteAllBytes(path, data);
        bool result = VerifyFileSignature(path);
        File.Delete(path);
        return result;
    }
}
