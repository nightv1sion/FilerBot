using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Filer.TelegramBot.Presentation.UserStates.Callbacks;

public sealed class CallbackSerializer
{
    private readonly JsonSerializerSettings _settings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead,
        Converters =
        {
            new StringEnumConverter(),
        }
    };
    
    public string Serialize(ICallback callback)
    {
        return JsonConvert.SerializeObject(callback, _settings);
    }

    public ICallback Deserialize(string callbackPayload)
    {
        ICallback? callback = JsonConvert.DeserializeObject<ICallback>(callbackPayload, _settings);

        if (callback is null)
        {
            throw new InvalidOperationException("Failed to deserialize callback");
        }

        return callback;
    }
}