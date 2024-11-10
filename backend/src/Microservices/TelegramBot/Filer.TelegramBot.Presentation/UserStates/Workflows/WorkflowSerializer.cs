using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Filer.TelegramBot.Presentation.UserStates.Workflows;

public sealed class WorkflowSerializer
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
    
    public string Serialize(IWorkflow callback)
    {
        return JsonConvert.SerializeObject(callback, _settings);
    }

    public IWorkflow Deserialize(string callbackPayload)
    {
        IWorkflow? callback = JsonConvert.DeserializeObject<IWorkflow>(callbackPayload, _settings);

        if (callback is null)
        {
            throw new InvalidOperationException("Failed to deserialize workflow");
        }

        return callback;
    }
}