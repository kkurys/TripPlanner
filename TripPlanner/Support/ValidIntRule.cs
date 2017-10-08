using System.Globalization;
using System.Windows.Controls;

namespace TripPlanner.Support
{
    public class ValidIntRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value as string;
            int result;

            if (!int.TryParse(input, out result) || result <= 0)
            {
                return new ValidationResult(false, "Musi być liczbą całkowitą!");
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }
    }
}