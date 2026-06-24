using EasyMemory.Base;

namespace EasyMemory.Pointers;

public abstract class Offset<T> where T : Pointer
{
    protected readonly Memory memory;
    protected readonly int offset;
    protected readonly T parent;

    public Offset(T parent, int offset)
    {
        this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
        this.offset = offset;
        this.memory = parent.MemoryHandle;
    }

    protected IntPtr Address => IntPtr.Add(BaseAddress, offset);

    protected IntPtr BaseAddress => parent.CurrentAddress;

    protected bool IsValid => parent.IsValid;

    protected Memory Memory => memory;
}