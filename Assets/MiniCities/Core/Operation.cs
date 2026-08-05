using System;

namespace MiniCities.Core
{
    public enum Operation
    {
        Add,
        Subtract,
        Multiply,
        Divide
    }

    public static class OperationExtensions
    {
        public static float Apply(this Operation operation, float value, float amount) => operation switch
        {
            Operation.Add => value + amount,
            Operation.Subtract => value - amount,
            Operation.Multiply => value * amount,
            Operation.Divide => value / (amount != 0 ? amount : 1),
            _ => throw new ArgumentOutOfRangeException(nameof(operation), "Not supported.")
        };

        public static string ToDescriptionText(this Operation operation, float amount) => operation switch
        {
            Operation.Add => $"+{amount}",
            Operation.Subtract => $"-{amount}",
            Operation.Multiply => $"x{amount}",
            Operation.Divide => $"/{amount}",
            _ => throw new ArgumentOutOfRangeException(nameof(operation), "Not supported.")
        };
    }
}