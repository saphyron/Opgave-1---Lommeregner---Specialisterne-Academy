using System;
using System.Linq;
using System.Windows.Forms;

namespace Lommeregner
{
    /// <summary>
    /// Programindgangspunkt der vælger mellem konsol-UI og Windows-UI baseret på argumenter.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Main. Brug <c>--ui</c> eller <c>-ui</c> for at starte Windows-UI; ellers startes konsol-UI.
        /// </summary>
        /// <param name="args">Kommandolinjeargumenter.</param>
        [STAThread]
        static void Main(string[] args)
        {
            bool useUi = args.Any(a =>
                a.Equals("--ui", StringComparison.OrdinalIgnoreCase) ||
                a.Equals("-ui", StringComparison.OrdinalIgnoreCase));

            if (useUi)
            {
                // WinForms bootstrap
                try
                {
                    ApplicationConfiguration.Initialize();
                }
                catch
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                }
                Application.Run(new Windows_GUI.MainGUI());
            }
            else
            {
                // Konsol-runner
                Konsol_App.KonsolApp.Run();
            }
        }
    }
}
