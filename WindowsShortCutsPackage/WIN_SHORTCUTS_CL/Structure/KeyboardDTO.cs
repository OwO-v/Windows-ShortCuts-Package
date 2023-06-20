namespace WIN_SHORTCUTS_CL.Structure
{
    internal static class KeyboardDTO
    {
        // ===== EnvArgs
        internal static System.Int32 Mode { get; set; }
        internal static System.Int32 TrayMode { get; set; }
        

        // ===== Custom Views
        internal static System.Windows.Controls.UserControl ViewUC_Load { get; set; }
        internal static System.Windows.Controls.UserControl ViewUC_Conf { get; set; }


        // ===== Worker
        internal static System.Threading.Tasks.Task[] ScWorker { get; set; }


        // ===== Config
        internal static System.String UserKeyPairFilePath { get; set; }
        internal static System.IO.Stream CurrentUserKeyPairFile { get; set; } // 사용중 여부 확인에만 이용, 사용 후 Dispose/null

        
        // ===== KeyStruct
        internal static WIN_SHORTCUTS_CL.Structure.ShortCutsPairList ScPairs { get; set; }
        internal static System.Int32 ScCurrentId { get; set; }

    }
}
