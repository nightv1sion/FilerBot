using System.Data;
using Filer.Common.Infrastructure.Abstractions;
using Npgsql;

namespace Filer.Common.Infrastructure;

internal sealed class DbConnectionFactory(NpgsqlDataSource dataSource) : IDbConnectionFactory
{
    public IDbConnection CreateConnection()
    {
        return dataSource.OpenConnection();
    }
}
