using System.Text.Json;

namespace Protocols.Common
{
    public abstract class AbstractProtocolStep<T>
    {
        public abstract string FileName { get; }
        public abstract string Description { get; }

        public async Task<(bool success, T parameters)> ReadParameters()
        {
            try
            {
                var input = await File.ReadAllTextAsync(FileName);
                var options = new JsonSerializerOptions();
                options.Converters.Add(new BigIntegerConverter());
                options.WriteIndented = true;
                var result = JsonSerializer.Deserialize<T>(input, options);

                return (true, result);
            }
            catch
            {
                return (false, default(T));
            }
        }

        public async Task WriteParameters(T parameters)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new BigIntegerConverter());
            options.WriteIndented = true;
            var serialized = JsonSerializer.Serialize(parameters, options);
            await File.WriteAllTextAsync(FileName, serialized);
        }
    }
}
