
using System.Windows.Controls;
using System.Windows.Input;

namespace WIN_SHORTCUTS_CL.Structure
{
    internal class KeyboardDTO
    {
        internal static int Mode { get; set; }
        internal static int TrayMode { get; set; }

        internal static UserControl ViewUC_Load { get; set; }
        internal static UserControl ViewUC_Conf { get; set; }

        /// <summary>
        /// Default Modifier Key는 Control + Shift;
        /// </summary>
        internal ModiKey? ModifierKey
        {
            get
            {
                if (modifierKey == null || modifierKey == ModiKey.None)
                    return ModiKey.Control | ModiKey.Shift;

                return modifierKey;
            }
            set
            {
                modifierKey = value;
            }
        }
        private ModiKey? modifierKey;

        /// <summary>
        /// Default Data Key는 C; 
        /// </summary>
        internal Key? DataKey
        {
            get
            {
                if (dataKey == null || dataKey == Key.None)
                    return Key.C;

                return dataKey;
            }
            set
            {
                dataKey = value;
            }
        }
        private Key? dataKey;
    }
}
