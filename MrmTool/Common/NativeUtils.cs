using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using TerraFX.Interop.Windows;
using WinRT;

using static MrmTool.Common.ErrorHelpers;
using static TerraFX.Interop.Windows.Windows;

namespace MrmTool
{
    internal static unsafe partial class NativeUtils
    {
        internal static void ShowFileInExplorer(string path)
        {
            fixed (char* pPath = path)
            {
                ITEMIDLIST* pList = default;
                if (SUCCEEDED_LOG(SHParseDisplayName(pPath, null, &pList, 0, null)))
                {
                    LOG_IF_FAILED(SHOpenFolderAndSelectItems(pList, 0, null, 0));
                    ILFree(pList);
                }
            }
        }
    }

#if ENABLE_HSTRING_MARSHALLER
    [CustomMarshaller(typeof(string), MarshalMode.Default, typeof(HStringMarshaller))]
    internal static unsafe class HStringMarshaller
    {
        public static nint ConvertToUnmanaged(string? managed)
            => MarshalString.FromManaged(managed);

        public static string? ConvertToManaged(nint unmanaged)
            => MarshalString.FromAbi(unmanaged);

        public static void Free(nint unmanaged)
            => MarshalString.DisposeAbi(unmanaged);
    }
#endif

    [Guid("4dc10e42-52e7-46da-8ae8-92a4e8afe20c")]
    [GeneratedComInterface(Options = ComInterfaceOptions.ComObjectWrapper)]
    internal unsafe partial interface IPickerPrivateInitialization
    {
        void _stub0();
        void _stub1();
        void _stub2();

        void SetInitialLocationWithShellItem(void* pShellItem);

        void SetNamespaceRoot(void* pShellItem);

        void SetInProcOverride(int bInProc);

        void SetTargetFolderLibrary(void* pShellItem);

#if ENABLE_HSTRING_MARSHALLER
        void PrepopulateCallingAppData([MarshalUsing(typeof(HStringMarshaller))] string appId, [MarshalUsing(typeof(HStringMarshaller))] string packageFullName);
#endif
    }
}
