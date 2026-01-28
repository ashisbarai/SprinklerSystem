namespace SprinklerSystem.Core.Configuration
{
    public static class PrecisionConfig
    {
        private static int _decimalPlaces = 6; // High precision default

        public static int DecimalPlaces
        {
            get => _decimalPlaces;
            set
            {
                if (value < 0 || value > 15)
                    throw new ArgumentOutOfRangeException(nameof(value),
                        "Decimal places must be between 0 and 15");
                _decimalPlaces = value;
            }
        }

        public static string FormatSpecifier => $"F{DecimalPlaces}";
    }
}
