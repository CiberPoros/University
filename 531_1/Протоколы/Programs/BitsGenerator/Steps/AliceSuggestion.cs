using Protocols.Common;

namespace BitsGenerator.Steps
{
    internal class AliceSuggestion : AbstractProtocolStep<AliceSuggestionParameter>
    {
        public override string FileName => "ПредположениеАлисы.txt";
        public override string Description => $"Алиса делает выбор, какое число использовать для вычисления {nameof(AliceRandomModPowNumber.Y)}: {nameof(BobRandomNumbers.H)} или {nameof(BobRandomNumbers.T)}";
    }

    public class AliceSuggestionParameter
    {
        public string UsedValue { get; set; }
    }
}
