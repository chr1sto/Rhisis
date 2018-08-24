using System.Text;

namespace Rhisis.Core.IO
{
    public static class EncodingUtilities
    {
        /// <summary>
        /// Registers an encoding provider.
        /// </summary>
        public static void Initialize()
        {
#if !NET45
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
        }
    }
}
