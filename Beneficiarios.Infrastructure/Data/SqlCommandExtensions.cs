using System;
using Microsoft.Data.SqlClient;

namespace Beneficiarios.Infrastructure.Data;

/// <summary>
/// Extension methods for SQL command operations.
/// Provides utility methods for handling nullable parameters and reading nullable values from SQL data readers.
/// </summary>
public static class SqlCommandExtensions
{
    /// <summary>
    /// Adds a nullable parameter to the SQL parameter collection.
    /// Converts null values to DBNull.Value to properly handle database NULL.
    /// </summary>
    /// <param name="parameters">The SQL parameter collection to add the parameter to.</param>
    /// <param name="name">The name of the parameter.</param>
    /// <param name="value">The value of the parameter. If null, DBNull.Value is used.</param>
    /// <returns>The created SQL parameter.</returns>
    public static SqlParameter AddNullable(this SqlParameterCollection parameters, string name, object? value)
    {
        // Convert null to DBNull.Value for proper SQL NULL handling
        return parameters.AddWithValue(name, value ?? DBNull.Value);
    }

    /// <summary>
    /// Reads a field value from a SQL data reader with a default fallback for NULL values.
    /// </summary>
    /// <typeparam name="T">The type to read the field value as.</typeparam>
    /// <param name="reader">The SQL data reader.</param>
    /// <param name="column">The column name to read.</param>
    /// <param name="defaultValue">The default value to return if the column is NULL. Default is default(T).</param>
    /// <returns>The field value if not NULL; otherwise, the default value.</returns>
    public static T GetFieldValueOrDefault<T>(this SqlDataReader reader, string column, T defaultValue = default!)
    {
        // Get the ordinal position of the column
        var ordinal = reader.GetOrdinal(column);

        // Check if the value is NULL
        if (reader.IsDBNull(ordinal))
        {
            return defaultValue;
        }

        // Read and return the field value
        return reader.GetFieldValue<T>(ordinal);
    }
}
