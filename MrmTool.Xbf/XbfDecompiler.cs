using MrmTool.Xbf.V2;

namespace MrmTool.Xbf;

public static class XbfDecompiler
{
    public static string Decompile(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new XbfReader(stream).RootObject.ToString();
    }
}
