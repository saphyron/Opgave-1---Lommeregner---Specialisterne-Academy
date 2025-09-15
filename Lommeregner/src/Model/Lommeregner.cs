using System;

namespace Lommeregner
{
    /// <summary>
    /// Simpel lommeregner med basale aritmetiske, trigonometriske og logaritmiske funktioner.
    /// </summary>
    public class Calculator
    {
        public double Add(double a, double b)
        {
            return a + b;
        }

        public double Subtract(double a, double b)
        {
            return a - b;
        }

        public double Multiply(double a, double b)
        {
            return a * b;
        }

        public double Divide(double a, double b)
        {
            if (b == 0)
                throw new DivideByZeroException("Cannot divide by zero.");
            return a / b;
        }
        public double Power(double a, double b)
        {
            return Math.Pow(a, b);
        }
        public double SquareRoot(double a)
        {
            if (a < 0)
                throw new ArgumentException("Cannot take the square root of a negative number.");
            return Math.Sqrt(a);
        }
        public double Percentage(double a, double b)
        {
            return (a / 100) * b;
        }

        public double Factorial(int n)
        {
            if (n < 0)
                throw new ArgumentException("Cannot compute factorial of a negative number.");
            if (n == 0 || n == 1)
                return 1;
            double result = 1;
            for (int i = 2; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }
        public double Modulus(double a, double b)
        {
            if (b == 0)
                throw new DivideByZeroException("Cannot perform modulus by zero.");
            return a % b;
        }
        public double Logarithm(double a, double baseValue)
        {
            if (a <= 0 || baseValue <= 1)
                throw new ArgumentException("Logarithm input must be greater than 0 and base must be greater than 1.");
            return Math.Log(a, baseValue);
        }
        public double NaturalLogarithm(double a)
        {
            if (a <= 0)
                throw new ArgumentException("Natural logarithm input must be greater than 0.");
            return Math.Log(a);
        }
        public double Sine(double angleInDegrees)
        {
            double angleInRadians = angleInDegrees * (Math.PI / 180);
            return Math.Sin(angleInRadians);
        }
        public double Cosine(double angleInDegrees)
        {
            double angleInRadians = angleInDegrees * (Math.PI / 180);
            return Math.Cos(angleInRadians);
        }
        public double Tangent(double angleInDegrees)
        {
            double angleInRadians = angleInDegrees * (Math.PI / 180);
            return Math.Tan(angleInRadians);
        }
        /// <summary>
        /// Udfører en navngiven operation med to argumenter (unære operationer ignorerer <paramref name="b"/>).
        /// Understøtter både fulde og korte aliasser (fx <c>multiply</c>/<c>mul</c>, <c>naturalLogarithm</c>/<c>ln</c>).
        /// </summary>
        /// <param name="operation">Navnet/alias på operationen (fx "add", "subtract", "sqrt", "ln").</param>
        /// <param name="a">Første argument. For unære operationer bruges kun dette.</param>
        /// <param name="b">Andet argument. Ignoreres for unære operationer som <c>sqrt</c>, <c>ln</c>, <c>sine</c> osv.</param>
        /// <returns>Resultatet af den valgte operation.</returns>
        /// <exception cref="ArgumentNullException">Hvis <paramref name="operation"/> er null.</exception>
        /// <exception cref="InvalidOperationException">Hvis operationen ikke genkendes.</exception>
        public double execution(string operation, double a, double b)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));
            var op = operation.Trim().ToLowerInvariant();

            // Alias-normalisering
            op = op switch
            {
                "add" or "plus" => "add",
                "subtract" or "sub" or "minus" => "subtract",
                "multiply" or "mul" => "multiply",
                "divide" or "div" => "divide",
                "power" or "pow" => "power",
                "sqrt" or "squareroot" => "sqrt",
                "percentage" or "percent" => "percentage",
                "factorial" or "fact" => "factorial",
                "modulus" or "mod" => "modulus",
                "logarithm" or "log" => "logarithm",
                "naturallogarithm" or "ln" => "naturalLogarithm",
                "sine" or "sin" => "sine",
                "cosine" or "cos" => "cosine",
                "tangent" or "tan" => "tangent",
                _ => operation  // bevar original hvis ikke i listen (case-sensitiv fallback)
            };

            return op switch
            {
                "add" => Add(a, b),
                "subtract" => Subtract(a, b),
                "multiply" => Multiply(a, b),
                "divide" => Divide(a, b),
                "power" => Power(a, b),
                "sqrt" => SquareRoot(a),
                "percentage" => Percentage(a, b),
                "factorial" => Factorial((int)a),
                "modulus" => Modulus(a, b),
                "logarithm" => Logarithm(a, b),
                "naturalLogarithm" => NaturalLogarithm(a),
                "sine" => Sine(a),
                "cosine" => Cosine(a),
                "tangent" => Tangent(a),
                _ => throw new InvalidOperationException("Unknown operation"),
            };
        }

    }
}