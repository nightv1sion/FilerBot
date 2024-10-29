using System.Data;

namespace Filer.Common.Infrastructure.Abstractions;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
