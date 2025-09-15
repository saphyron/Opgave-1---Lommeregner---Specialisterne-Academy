using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace Lommeregner.Windows_GUI
{
    /// <summary>
    /// Windows Forms-brugerflade for lommeregneren med to inputfelter, valg af operation og resultatvisning.
    /// </summary>
    public class MainGUI : Form
    {
        private readonly TextBox txtA = new() { Left = 20, Top = 35, Width = 180, PlaceholderText = "A" };
        private readonly TextBox txtB = new() { Left = 220, Top = 35, Width = 180, PlaceholderText = "B / Base" };
        private readonly ComboBox cmbOp = new() { Left = 20, Top = 80, Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly Button btnCalc = new() { Left = 220, Top = 80, Width = 180, Text = "Beregn" };
        private readonly TextBox txtResult = new() { Left = 20, Top = 130, Width = 380, ReadOnly = true };

        private readonly Lommeregner.Calculator calc = new();

        /// <summary>
        /// Opslagstabel fra UI-tekst til operation, antal argumenter, om heltal kræves samt hjælpetekst.
        /// </summary>
        private readonly Dictionary<string, (string Op, int Args, bool IntArg, string? Help)> OpMap = new()
        {
            { "Add (A+B)",                  ("add",     2, false, null) },
            { "Subtract (A-B)",             ("sub",     2, false, null) },
            { "Multiply (A*B)",             ("mul",     2, false, null) },
            { "Divide (A/B)",               ("div",     2, false, null) },
            { "Power (A^B)",                ("pow",     2, false, null) },
            { "SquareRoot (√A)",            ("sqrt",    1, false, null) },
            { "Percentage (A% af B)",       ("percent", 2, false, "A er procent; B er tallet som procenten anvendes på") },
            { "Factorial (n! af A)",        ("fact",    1, true , "A skal være et heltal") },
            { "Modulus (A%B)",              ("mod",     2, false, null) },
            { "Logarithm (log_base(B)(A))", ("log",     2, false, "A er argument; B er base (>1)") },
            { "NaturalLogarithm (ln A)",    ("ln",      1, false, null) },
            { "Sine (grader A)",            ("sin",     1, false, null) },
            { "Cosine (grader A)",          ("cos",     1, false, null) },
            { "Tangent (grader A)",         ("tan",     1, false, null) }
        };

        /// <summary>
        /// Initialiserer og opbygger UI-komponenterne.
        /// </summary>
        public MainGUI()
        {
            Text = "Lommeregner";
            Width = 440;
            Height = 220;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            Controls.Add(new Label { Left = 20, Top = 15, Text = "Tal (brug . eller ,)", AutoSize = true });
            Controls.Add(txtA);
            Controls.Add(txtB);
            Controls.Add(new Label { Left = 20, Top = 60, Text = "Operation", AutoSize = true });
            Controls.Add(cmbOp);
            Controls.Add(btnCalc);
            Controls.Add(new Label { Left = 20, Top = 110, Text = "Resultat", AutoSize = true });
            Controls.Add(txtResult);

            cmbOp.Items.AddRange(new object[] {
                "Add (A+B)","Subtract (A-B)","Multiply (A*B)","Divide (A/B)","Power (A^B)",
                "SquareRoot (√A)","Percentage (A% af B)","Factorial (n! af A)","Modulus (A%B)",
                "Logarithm (log_base(B)(A))","NaturalLogarithm (ln A)",
                "Sine (grader A)","Cosine (grader A)","Tangent (grader A)"
            });
            cmbOp.SelectedIndex = 0;

            btnCalc.Click += (_, __) => Calculate();
            AcceptButton = btnCalc;

            cmbOp.SelectedIndexChanged += (_, __) =>
            {
                var key = cmbOp.SelectedItem?.ToString()!;
                if (OpMap.TryGetValue(key, out var def) && def.Help is not null)
                    txtResult.PlaceholderText = def.Help;
                else
                    txtResult.PlaceholderText = "";
            };
        }

        /// <summary>
        /// Læser input fra felterne, udfører valgt operation via modellen og viser resultatet.
        /// </summary>
        private void Calculate()
        {
            try
            {
                var key = cmbOp.SelectedItem?.ToString()!;
                if (!OpMap.TryGetValue(key, out var def))
                    throw new Exception("Ukendt operation.");

                var (a, b) = BuildArgs(def);
                var res = calc.execution(def.Op, a, b); // altid to doubles; b ignoreres af unære
                txtResult.Text = res.ToString(CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Fejl", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Validerer og parser A/B fra tekstfelter i forhold til den valgte operation.
        /// </summary>
        /// <param name="def">Operationsdefinition.</param>
        /// <returns>Par af (A, B). For unære operationer sættes B = 0.0.</returns>
        private (double a, double b) BuildArgs((string Op, int Args, bool IntArg, string? Help) def)
        {
            bool TryParseD(string s, out double d) =>
                double.TryParse(s, NumberStyles.Float, CultureInfo.GetCultureInfo("da-DK"), out d) ||
                double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out d);

            if (def.Args == 1)
            {
                // Unær: kun A anvendes
                if (def.IntArg)
                {
                    if (!int.TryParse(txtA.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var n))
                        throw new Exception("A skal være et heltal.");
                    return (n, 0.0);
                }
                else
                {
                    if (!TryParseD(txtA.Text, out var a))
                        throw new Exception("A er ugyldig.");
                    return (a, 0.0);
                }
            }

            // Binære
            if (def.Op == "percent")
            {
                if (!TryParseD(txtA.Text, out var pa)) throw new Exception("A (procent) er ugyldig.");
                if (!TryParseD(txtB.Text, out var pb)) throw new Exception("B (tal) er ugyldig.");
                return (pa, pb);
            }
            if (def.Op == "log")
            {
                if (!TryParseD(txtA.Text, out var la)) throw new Exception("A (argument) er ugyldig.");
                if (!TryParseD(txtB.Text, out var lb)) throw new Exception("B (base) er ugyldig.");
                return (la, lb);
            }

            if (!TryParseD(txtA.Text, out var a1)) throw new Exception("A er ugyldig.");
            if (!TryParseD(txtB.Text, out var b1)) throw new Exception("B er ugyldig.");
            return (a1, b1);
        }
    }
}
