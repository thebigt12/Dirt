using Dirt.Parser.Nodes;

namespace Dirt.Interpreter
{
    public class StopSignal : Exception
    {
        public object? Value { get; set; }

        public StopSignal(object? value)
        {
            Value = value;
        }
    }
}
