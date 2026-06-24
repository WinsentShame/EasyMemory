using EasyMemory.Utils;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static EasyMemory.Utils.Native;

namespace EasyMemory.Base;

public class Memory : IDisposable
{
    private IntPtr processHandle;
    private Process? targetProcess;
    private string? processName;

    public IntPtr Handle => processHandle;

    public bool OpenProcess(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Process name cannot be empty.");

        string cleanName = name.Replace(".exe", "");
        Process[] processes = Process.GetProcessesByName(cleanName);
        if (processes.Length == 0)
            return false;

        targetProcess = processes[0];
        processName = targetProcess.ProcessName;

        uint accessFlags = PROCESS_VM_READ | PROCESS_VM_WRITE | PROCESS_VM_OPERATION | PROCESS_QUERY_INFORMATION;
        processHandle = Native.OpenProcess(accessFlags, false, targetProcess.Id);

        return processHandle != IntPtr.Zero;
    }

    public void CloseProcess()
    {
        if (processHandle != IntPtr.Zero)
        {
            CloseHandle(processHandle);
            processHandle = IntPtr.Zero;
            targetProcess = null;
            processName = null;
        }
    }

    public IntPtr GetMainModuleBaseAddress()
    {
        if (targetProcess == null)
            throw new InvalidOperationException("Process is not open.");
        if (string.IsNullOrEmpty(processName))
            throw new InvalidOperationException("Module name is not stored.");

        foreach (ProcessModule module in targetProcess.Modules)
        {
            if (module.ModuleName.Equals(processName + ".exe", StringComparison.OrdinalIgnoreCase))
                return module.BaseAddress;
        }
        return IntPtr.Zero;
    }

    public IntPtr GetModuleBaseAddress(string moduleName)
    {
        if (targetProcess == null)
            throw new InvalidOperationException("Process is not open.");
        if (string.IsNullOrWhiteSpace(moduleName))
            throw new ArgumentException("Module name cannot be empty.");

        foreach (ProcessModule module in targetProcess.Modules)
        {
            if (module.ModuleName.Equals(moduleName, StringComparison.OrdinalIgnoreCase))
                return module.BaseAddress;
        }
        return IntPtr.Zero;
    }

    public T ReadValue<T>(IntPtr address) where T : struct
    {
        int size = Marshal.SizeOf<T>();
        byte[] buffer = new byte[size];
        ReadProcessMemory(processHandle, address, buffer, (uint)size, out IntPtr bytesRead);

        if (bytesRead.ToInt64() != size)
            throw new Exception($"Failed to read memory at address 0x{address.ToInt64():X}.");

        GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        try{ return Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject()); }
        finally { handle.Free(); }
    }

    public void WriteValue<T>(IntPtr address, T value) where T : struct
    {
        int size = Marshal.SizeOf<T>();
        byte[] buffer = new byte[size];

        GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        try { Marshal.StructureToPtr(value, handle.AddrOfPinnedObject(), false); }
        finally { handle.Free(); }

        WriteProcessMemory(processHandle, address, buffer, (uint)size, out IntPtr bytesWritten);
        if (bytesWritten.ToInt64() != size)
            throw new Exception($"Failed to write memory at address 0x{address.ToInt64():X}.");
    }

    public byte[] ReadBytes(IntPtr address, int length)
    {
        byte[] buffer = new byte[length];
        ReadProcessMemory(processHandle, address, buffer, (uint)length, out IntPtr bytesRead);
        if (bytesRead.ToInt64() != length)
            throw new Exception($"Failed to read {length} bytes at address 0x{address.ToInt64():X}.");
        return buffer;
    }

    public void WriteBytes(IntPtr address, byte[] buffer)
    {
        WriteProcessMemory(processHandle, address, buffer, (uint)buffer.Length, out IntPtr bytesWritten);
        if (bytesWritten.ToInt64() != buffer.Length)
            throw new Exception($"Failed to write {buffer.Length} bytes at address 0x{address.ToInt64():X}.");
    }

    public IntPtr AllocateMemoryNear(IntPtr targetAddress, int size, int maxDistanceMB = 64)
    {
        long maxDistance = (long)maxDistanceMB * 1024 * 1024;
        long start = targetAddress.ToInt64() - maxDistance;
        if (start < 0) start = 0;
        long end = targetAddress.ToInt64() + maxDistance;

        for (long addr = start; addr <= end; addr += 0x10000)
        {
            IntPtr ptr = VirtualAllocEx(processHandle, new IntPtr(addr), (uint)size,
                MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
            if (ptr != IntPtr.Zero)
            {
                long offset = ptr.ToInt64() - targetAddress.ToInt64();
                if (Math.Abs(offset) <= maxDistance)
                    return ptr;
                VirtualFreeEx(processHandle, ptr, 0, MEM_RELEASE);
            }
        }
        return VirtualAllocEx(processHandle, IntPtr.Zero, (uint)size,
           MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
    }

    public bool FreeMemory(IntPtr address) { return VirtualFreeEx(processHandle, address, 0, MEM_RELEASE); }

    public void Dispose() => CloseProcess();
}

