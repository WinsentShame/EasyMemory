namespace EasyMemory.Addresses.Instructions;

public class IHelper
{
    private static readonly Dictionary<IBase, byte[]> iBasePrefix = new Dictionary<IBase, byte[]>()
        {
            { IBase.mulss_xmm0, new byte[] { 0xF3, 0x0F, 0x59, 0x05 } },
            { IBase.mulss_xmm1, new byte[] { 0xF3, 0x0F, 0x59, 0x0D } },
            { IBase.mulss_xmm2, new byte[] { 0xF3, 0x0F, 0x59, 0x15 } },
            { IBase.mulss_xmm3, new byte[] { 0xF3, 0x0F, 0x59, 0x1D } },
            { IBase.mulss_xmm4, new byte[] { 0xF3, 0x0F, 0x59, 0x25 } },
            { IBase.mulss_xmm5, new byte[] { 0xF3, 0x0F, 0x59, 0x2D } },
            { IBase.mulss_xmm6, new byte[] { 0xF3, 0x0F, 0x59, 0x35 } },
            { IBase.mulss_xmm7, new byte[] { 0xF3, 0x0F, 0x59, 0x3D } },
            { IBase.mulss_xmm8, new byte[] { 0xF3, 0x45, 0x0F, 0x59, 0x05 } },
            { IBase.mulss_xmm9, new byte[] { 0xF3, 0x45, 0x0F, 0x59, 0x0D } },
            { IBase.mulss_xmm10, new byte[] { 0xF3, 0x45, 0x0F, 0x59, 0x15 } },
            { IBase.mulss_xmm11, new byte[] { 0xF3, 0x45, 0x0F, 0x59, 0x1D } },
            { IBase.mulss_xmm12, new byte[] { 0xF3, 0x45, 0x0F, 0x59, 0x25 } },
            { IBase.mulss_xmm13, new byte[] { 0xF3, 0x45, 0x0F, 0x59, 0x2D } },
            { IBase.mulss_xmm14, new byte[] { 0xF3, 0x45, 0x0F, 0x59, 0x35 } },
            { IBase.mulss_xmm15, new byte[] { 0xF3, 0x45, 0x0F, 0x59, 0x3D } },

            { IBase.movss_xmm0, new byte[] { 0xF3, 0x0F, 0x10, 0x05 } },
            { IBase.movss_xmm1, new byte[] { 0xF3, 0x0F, 0x10, 0x0D } },
            { IBase.movss_xmm2, new byte[] { 0xF3, 0x0F, 0x10, 0x15 } },
            { IBase.movss_xmm3, new byte[] { 0xF3, 0x0F, 0x10, 0x1D } },
            { IBase.movss_xmm4, new byte[] { 0xF3, 0x0F, 0x10, 0x25 } },
            { IBase.movss_xmm5, new byte[] { 0xF3, 0x0F, 0x10, 0x2D } },
            { IBase.movss_xmm6, new byte[] { 0xF3, 0x0F, 0x10, 0x35 } },
            { IBase.movss_xmm7, new byte[] { 0xF3, 0x0F, 0x10, 0x3D } },
            { IBase.movss_xmm8, new byte[] { 0xF3, 0x45, 0x0F, 0x10, 0x05 } },
            { IBase.movss_xmm9, new byte[] { 0xF3, 0x45, 0x0F, 0x10, 0x0D } },
            { IBase.movss_xmm10, new byte[] { 0xF3, 0x45, 0x0F, 0x10, 0x15 } },
            { IBase.movss_xmm11, new byte[] { 0xF3, 0x45, 0x0F, 0x10, 0x1D } },
            { IBase.movss_xmm12, new byte[] { 0xF3, 0x45, 0x0F, 0x10, 0x25 } },
            { IBase.movss_xmm13, new byte[] { 0xF3, 0x45, 0x0F, 0x10, 0x2D } },
            { IBase.movss_xmm14, new byte[] { 0xF3, 0x45, 0x0F, 0x10, 0x35 } },
            { IBase.movss_xmm15, new byte[] { 0xF3, 0x45, 0x0F, 0x10, 0x3D } },

            { IBase.addss_xmm0, new byte[] { 0xF3, 0x0F, 0x58, 0x05 } },
            { IBase.addss_xmm1, new byte[] { 0xF3, 0x0F, 0x58, 0x0D } },
            { IBase.addss_xmm2, new byte[] { 0xF3, 0x0F, 0x58, 0x15 } },
            { IBase.addss_xmm3, new byte[] { 0xF3, 0x0F, 0x58, 0x1D } },
            { IBase.addss_xmm4, new byte[] { 0xF3, 0x0F, 0x58, 0x25 } },
            { IBase.addss_xmm5, new byte[] { 0xF3, 0x0F, 0x58, 0x2D } },
            { IBase.addss_xmm6, new byte[] { 0xF3, 0x0F, 0x58, 0x35 } },
            { IBase.addss_xmm7, new byte[] { 0xF3, 0x0F, 0x58, 0x3D } },
            { IBase.addss_xmm8, new byte[] { 0xF3, 0x45, 0x0F, 0x58, 0x05 } },
            { IBase.addss_xmm9, new byte[] { 0xF3, 0x45, 0x0F, 0x58, 0x0D } },
            { IBase.addss_xmm10, new byte[] { 0xF3, 0x45, 0x0F, 0x58, 0x15 } },
            { IBase.addss_xmm11, new byte[] { 0xF3, 0x45, 0x0F, 0x58, 0x1D } },
            { IBase.addss_xmm12, new byte[] { 0xF3, 0x45, 0x0F, 0x58, 0x25 } },
            { IBase.addss_xmm13, new byte[] { 0xF3, 0x45, 0x0F, 0x58, 0x2D } },
            { IBase.addss_xmm14, new byte[] { 0xF3, 0x45, 0x0F, 0x58, 0x35 } },
            { IBase.addss_xmm15, new byte[] { 0xF3, 0x45, 0x0F, 0x58, 0x3D } },

            { IBase.subss_xmm0, new byte[] { 0xF3, 0x0F, 0x5C, 0x05 } },
            { IBase.subss_xmm1, new byte[] { 0xF3, 0x0F, 0x5C, 0x0D } },
            { IBase.subss_xmm2, new byte[] { 0xF3, 0x0F, 0x5C, 0x15 } },
            { IBase.subss_xmm3, new byte[] { 0xF3, 0x0F, 0x5C, 0x1D } },
            { IBase.subss_xmm4, new byte[] { 0xF3, 0x0F, 0x5C, 0x25 } },
            { IBase.subss_xmm5, new byte[] { 0xF3, 0x0F, 0x5C, 0x2D } },
            { IBase.subss_xmm6, new byte[] { 0xF3, 0x0F, 0x5C, 0x35 } },
            { IBase.subss_xmm7, new byte[] { 0xF3, 0x0F, 0x5C, 0x3D } },
            { IBase.subss_xmm8, new byte[] { 0xF3, 0x45, 0x0F, 0x5C, 0x05 } },
            { IBase.subss_xmm9, new byte[] { 0xF3, 0x45, 0x0F, 0x5C, 0x0D } },
            { IBase.subss_xmm10, new byte[] { 0xF3, 0x45, 0x0F, 0x5C, 0x15 } },
            { IBase.subss_xmm11, new byte[] { 0xF3, 0x45, 0x0F, 0x5C, 0x1D } },
            { IBase.subss_xmm12, new byte[] { 0xF3, 0x45, 0x0F, 0x5C, 0x25 } },
            { IBase.subss_xmm13, new byte[] { 0xF3, 0x45, 0x0F, 0x5C, 0x2D } },
            { IBase.subss_xmm14, new byte[] { 0xF3, 0x45, 0x0F, 0x5C, 0x35 } },
            { IBase.subss_xmm15, new byte[] { 0xF3, 0x45, 0x0F, 0x5C, 0x3D } },

            { IBase.divss_xmm0, new byte[] { 0xF3, 0x0F, 0x5E, 0x05 } },
            { IBase.divss_xmm1, new byte[] { 0xF3, 0x0F, 0x5E, 0x0D } },
            { IBase.divss_xmm2, new byte[] { 0xF3, 0x0F, 0x5E, 0x15 } },
            { IBase.divss_xmm3, new byte[] { 0xF3, 0x0F, 0x5E, 0x1D } },
            { IBase.divss_xmm4, new byte[] { 0xF3, 0x0F, 0x5E, 0x25 } },
            { IBase.divss_xmm5, new byte[] { 0xF3, 0x0F, 0x5E, 0x2D } },
            { IBase.divss_xmm6, new byte[] { 0xF3, 0x0F, 0x5E, 0x35 } },
            { IBase.divss_xmm7, new byte[] { 0xF3, 0x0F, 0x5E, 0x3D } },
            { IBase.divss_xmm8, new byte[] { 0xF3, 0x45, 0x0F, 0x5E, 0x05 } },
            { IBase.divss_xmm9, new byte[] { 0xF3, 0x45, 0x0F, 0x5E, 0x0D } },
            { IBase.divss_xmm10, new byte[] { 0xF3, 0x45, 0x0F, 0x5E, 0x15 } },
            { IBase.divss_xmm11, new byte[] { 0xF3, 0x45, 0x0F, 0x5E, 0x1D } },
            { IBase.divss_xmm12, new byte[] { 0xF3, 0x45, 0x0F, 0x5E, 0x25 } },
            { IBase.divss_xmm13, new byte[] { 0xF3, 0x45, 0x0F, 0x5E, 0x2D } },
            { IBase.divss_xmm14, new byte[] { 0xF3, 0x45, 0x0F, 0x5E, 0x35 } },
            { IBase.divss_xmm15, new byte[] { 0xF3, 0x45, 0x0F, 0x5E, 0x3D } },
        };

    public static int GetSize(IBase instructions)
    {
        string name = instructions.ToString();
        if (name.StartsWith("mulss_") || name.StartsWith("movss_") ||
            name.StartsWith("addss_") || name.StartsWith("subss_") ||
            name.StartsWith("divss_"))
        {
            if (name.Contains("_xmm8") || name.Contains("_xmm9") ||
                name.Contains("_xmm10") || name.Contains("_xmm11") ||
                name.Contains("_xmm12") || name.Contains("_xmm13") ||
                name.Contains("_xmm14") || name.Contains("_xmm15"))
                return 9;
            else
                return 8;
        }

        throw new ArgumentException("Unknown instruction");
    }

    public static byte[] BuildInstructions(IBase instructions, int ripOffset)
    {
        if (!iBasePrefix.TryGetValue(instructions, out var insPrefix))
            throw new ArgumentException("Unknown instruction");

        byte[] full = new byte[insPrefix.Length + 4];
        Array.Copy(insPrefix, full, insPrefix.Length);
        byte[] offsetBytes = BitConverter.GetBytes(ripOffset);
        Array.Copy(offsetBytes, 0, full, insPrefix.Length, 4);
        return full;
    }
}

