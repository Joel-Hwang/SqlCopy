using System;
using System.Runtime.InteropServices;
namespace SqlCopy
{
    class Program
    {
        [DllImport("user32.dll")]
        internal static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll")]
        internal static extern bool CloseClipboard();

        [DllImport("user32.dll")]
        internal static extern bool SetClipboardData(uint uFormat, IntPtr data);

        [DllImport("user32.dll")]
        static extern IntPtr GetClipboardData(uint uFormat);

        [STAThread]
        static void Main(string[] args)
        {
            OpenClipboard(IntPtr.Zero);
            var yourString = getData();
            var ptr = Marshal.StringToHGlobalUni(yourString);
            SetClipboardData(13, ptr);
            CloseClipboard();
            Marshal.FreeHGlobal(ptr);
        }

        static string getData() {
            string result = "";
            IntPtr ptr = GetClipboardData(13);
            string cbData = System.Runtime.InteropServices.Marshal.PtrToStringAuto(ptr);
            string[] query = cbData.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (string line in query) {
                result = result + "\"" + line + " \\n\"+\r\n";
            }

            return result.Substring(0, result.Length-3)+";";
        
        }
    }
}
