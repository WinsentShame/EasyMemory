using EasyMemory.Addresses.Instructions;
using EasyMemory.Base;

namespace EasyMemory.Addresses;

public abstract class Address : IDisposable
{
    protected readonly Memory memory;
    protected readonly IntPtr address;
    protected readonly IBase instruction;
    protected readonly int instructionSize;
    protected byte[] originalBytes;
    protected IntPtr valueAddress;
    protected bool patched;

    protected Address(Memory memory, int relativeAddress, IBase instruction,
                      byte[]? originalBytes = null, float initialValue = 0f, int customSize = 0)
        : this(memory, IntPtr.Add(memory.GetMainModuleBaseAddress(), relativeAddress),
              instruction, originalBytes, initialValue, customSize)
    { }

    protected Address(Memory mem, IntPtr absoluteAddress, IBase ins,
                      byte[] origBytes = null, float initialValue = 0f, int customSize = 0)
    {
        memory = mem ?? throw new ArgumentNullException(nameof(mem));
        address = absoluteAddress;
        instruction = ins;

        if (ins == IBase.nan)
        {
            if (customSize <= 0)
                throw new ArgumentException("For NaN, customSize must be > 0");
            instructionSize = customSize;
            originalBytes = origBytes ?? Array.Empty<byte>();
            valueAddress = IntPtr.Zero;
            patched = false;

            if (originalBytes.Length == 0)
                originalBytes = memory.ReadBytes(address, instructionSize);
            return;
        }

        instructionSize = IHelper.GetSize(ins);
        originalBytes = origBytes ?? Array.Empty<byte>();
        patched = false;

        valueAddress = memory.AllocateMemoryNear(address, 8, 64);
        if (valueAddress == IntPtr.Zero)
            throw new InvalidOperationException("Failed to allocate mem near the target address.");
        memory.WriteValue(valueAddress, initialValue);

        if (originalBytes.Length == 0)
            originalBytes = memory.ReadBytes(address, instructionSize);
    }

    public void Hook()
    {
        if (instruction == IBase.nan) return;
        if (patched) return;

        long ripOffset = valueAddress.ToInt64() - (address.ToInt64() + instructionSize);
        if (ripOffset < int.MinValue || ripOffset > int.MaxValue)
            throw new InvalidOperationException($"RIP offset {ripOffset} does not fit into 32 bits");

        byte[] newInstruction = IHelper.BuildInstructions(instruction, (int)ripOffset);
        if (newInstruction.Length != instructionSize)
            throw new InvalidOperationException($"Instruction length ({newInstruction.Length}) != {instructionSize}");

        Patch(newInstruction);
    }

    public void Patch(byte[] patchBytes)
    {
        if (instructionSize == 0) return;
        if (patchBytes.Length != instructionSize)
            throw new ArgumentException($"The patch size ({patchBytes.Length}) does not match the ins size ({instructionSize})");

        if (originalBytes == null || originalBytes.Length == 0)
            originalBytes = memory.ReadBytes(address, instructionSize);

        memory.WriteBytes(address, patchBytes);
        patched = true;
    }

    public void Nop()
    {
        if (instructionSize == 0) return;
        byte[] nops = new byte[instructionSize];
        for (int i = 0; i < nops.Length; i++) nops[i] = 0x90;
        Patch(nops);
    }

    public void Restore()
    {
        if (instructionSize == 0) return;
        if (!patched) return;
        if (originalBytes != null && originalBytes.Length > 0)
            memory.WriteBytes(address, originalBytes);
        patched = false;
    }

    public void SetValue<T>(T value) where T : struct
    {
        if (valueAddress == IntPtr.Zero)
            throw new InvalidOperationException("Memory not allocated.");
        memory.WriteValue(valueAddress, value);
    }

    public T GetValue<T>() where T : struct
    {
        if (valueAddress == IntPtr.Zero)
            throw new InvalidOperationException("Memory not allocated.");
        return memory.ReadValue<T>(valueAddress);
    }

    public void Dispose()
    {
        Restore();
        if (valueAddress != IntPtr.Zero)
            memory.FreeMemory(valueAddress);
        valueAddress = IntPtr.Zero;
    }
}

