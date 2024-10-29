using Filer.Common.Domain.Core.Utility;

namespace Filer.Common.Domain.Core.Primitives;

public abstract class Entity
{
    protected Entity(Guid id)
    {
        Ensure.NotEmpty(id, "The identifier is required", nameof(id));

        Id = id;
    }

    protected Entity()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; private set; }
}
