using EasyMemory.Base;

namespace EasyMemory.Pointers;

public abstract class Pointer
{
    protected readonly Memory memory;
    protected readonly IntPtr baseAddress;
    protected readonly int[] offsets;

    public Pointer(Memory mem, IntPtr absoluteAddress)
    {
        memory = mem ?? throw new ArgumentNullException(nameof(mem));
        baseAddress = absoluteAddress;
        offsets = Array.Empty<int>();
    }

    public Pointer(Memory mem, int staticOffset, int[] dynamicOffsets)
    {
        memory = mem ?? throw new ArgumentNullException(nameof(mem));
        offsets = dynamicOffsets ?? throw new ArgumentNullException(nameof(dynamicOffsets));

        IntPtr moduleBase = mem.GetMainModuleBaseAddress();
        if (moduleBase == IntPtr.Zero)
            throw new Exception("Main module not found.");

        baseAddress = IntPtr.Add(moduleBase, staticOffset);
    }

    public Memory MemoryHandle => memory;

    public IntPtr CurrentAddress { get { try { return ResolveAddress(); } catch { return IntPtr.Zero; }} }

    public bool IsValid => CurrentAddress != IntPtr.Zero;

    private IntPtr ResolveAddress()
    {
        if (baseAddress == IntPtr.Zero || offsets == null || offsets.Length == 0)
            return baseAddress;

        IntPtr currentPtr = baseAddress;
        for (int i = 0; i < offsets.Length; i++)
        {
            long ptrValue = memory.ReadValue<long>(currentPtr);
            if (ptrValue == 0)
                return IntPtr.Zero;
            currentPtr = new IntPtr(ptrValue + offsets[i]);
        }
        return currentPtr;
    }
}