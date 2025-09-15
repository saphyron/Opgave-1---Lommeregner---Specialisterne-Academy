using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace Lommeregner.Konsol_App
{
    /// <summary>
    /// Konsol-baseret brugerflade for lommeregneren.
    /// Viser en menustyret oplevelse og kalder <see cref="Lommeregner.Calculator.execution(string, double, double)"/>.
    /// </summary>
    public static class KonsolApp
    {
        /// <summary>
        /// Mapping fra menupunkt til operation, antal argumenter og inputkrav.
        /// </summary>
        private static readonly Dictionary<string, (string Label, string Op, int Args, bool IntArg)> Ops =
            new(StringComparer.OrdinalIgnoreCase)
        {
            { "1",  ("Add (a + b)",                "add",     2, false) },
            { "2",  ("Subtract (a - b)",           "sub",     2, false) },
            { "3",  ("Multiply (a * b)",           "mul",     2, false) },
            { "4",  ("Divide (a / b)",             "div",     2, false) },
            { "5",  ("Power (a ^ b)",              "pow",     2, false) },
            { "6",  ("SquareRoot (√a)",            "sqrt",    1, false) },
            { "7",  ("Percentage (a% af b)",       "percent", 2, false) },
            { "8",  ("Factorial (n!)",             "fact",    1, true ) },
            { "9",  ("Modulus (a % b)",            "mod",     2, false) },
            { "10", ("Logarithm (log_base(B)(A))", "log",     2, false) },
            { "11", ("NaturalLogarithm (ln A)",    "ln",      1, false) },
            { "12", ("Sine (grader A)",            "sin",     1, false) },
            { "13", ("Cosine (grader A)",          "cos",     1, false) },
            { "14", ("Tangent (grader A)",         "tan",     1, false) }
        };

        /// <summary>
        /// Starter konsol-UI’et. Skriv <c>--ui</c> i menuen for at åbne Windows-UI fra samme proces.
        /// </summary>
        public static void Run()
        {
            var calc = new Lommeregner.Calculator();

            while (true)
            {
                Console.WriteLine("\n=== Lommeregner (skriv --ui for at åbne Windows UI) ===");
                foreach (var kv in Ops)
                    Console.WriteLine($"{kv.Key}) {kv.Value.Label}");
                Console.WriteLine("0) Afslut");
                Console.Write("Vælg: ");

                var choice = (Console.ReadLine() ?? "").Trim();

                if (choice == "0") return;

                if (choice.Equals("--ui", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        // Klassisk WinForms bootstrap (kompatibel bredt)
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new Windows_GUI.MainGUI());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Fejl ved åbning af UI: {ex.Message}");
                    }
                    continue;
                }

                if (!Ops.TryGetValue(choice, out var def))
                {
                    Console.WriteLine("Ugyldigt valg.");
                    continue;
                }

                try
                {
                    // Læs A og (evt.) B
                    double a, b;
                    if (def.Args == 1)
                    {
                        a = def.IntArg ? ReadInt("n (heltal)") : ReadDouble("A");
                        b = 0.0; // dummy, bruges ikke
                    }
                    else
                    {
                        // Særlige tekster for percent/log
                        if (def.Op == "percent")
                        {
                            a = ReadDouble("A (procent, fx 25 eller 25,5)");
                            b = ReadDouble("B (tal som procenten beregnes af)");
                        }
                        else if (def.Op == "log")
                        {
                            a = ReadDouble("A (argument)");
                            b = ReadDouble("Base (>1)");
                        }
                        else
                        {
                            a = ReadDouble("A");
                            b = ReadDouble("B");
                        }

                        if (def.IntArg) a = (int)a; // håndhæver heltal for factorial
                    }

                    // Udfør operationen
                    var result = calc.execution(def.Op, a, b);
                    Console.WriteLine($"= {result.ToString(CultureInfo.InvariantCulture)}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fejl: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Læser et flydende tal fra konsollen. Accepterer både dansk og invariant format.
        /// </summary>
        private static double ReadDouble(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt}: ");
                var s = Console.ReadLine() ?? "";
                if (double.TryParse(s, NumberStyles.Float, CultureInfo.GetCultureInfo("da-DK"), out var d) ||
                    double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out d))
                    return d;

                Console.WriteLine("Ugyldigt tal. Prøv igen (fx 12,34 eller 12.34).");
            }
        }

        /// <summary>
        /// Læser et heltal fra konsollen (invariant kultur).
        /// </summary>
        private static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt}: ");
                var s = Console.ReadLine() ?? "";
                if (int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var n))
                    return n;

                Console.WriteLine("Ugyldigt heltal. Prøv igen.");
            }
        }
    }
}
