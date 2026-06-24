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
    }

    protected IntPtr Address
    {
        get
        {
            if (!parent.IsValid)
                return IntPtr.Zero;

            return IntPtr.Add(parent.CurrentAddress, offset);
        }
    }

    protected bool IsValid => parent.IsValid;

    public Memory MemoryHandle => memory;
}