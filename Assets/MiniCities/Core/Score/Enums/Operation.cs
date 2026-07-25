using System;

namespace MiniCities.Core.Score.Enums
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
            _ => throw new ArgumentOutOfRangeException(nameof(operation), $"{operation} is not supported.")
        };
    }
}