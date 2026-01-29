using System;
using System.Collections.Generic;
using System.Linq;

namespace Beneficiarios.Application.Common;

/// <summary>
/// Represents a paginated result set of items.
/// Provides pagination metadata such as current page, page size, and total count.
/// </summary>
/// <typeparam name="T">The type of items in the result set.</typeparam>
public class PagedResult<T>
{
    /// <summary>
    /// Read-only collection of items for the current page.
    /// </summary>
    public IReadOnlyCollection<T> Items { get; }

    /// <summary>
    /// Total number of items across all pages.
    /// </summary>
    public int TotalCount { get; }

    /// <summary>
    /// Current page number (1-based index).
    /// </summary>
    public int Page { get; }

    /// <summary>
    /// Number of items per page.
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    /// Total number of pages calculated from total count and page size.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    /// <summary>
    /// Constructor that initializes a paginated result with items and pagination metadata.
    /// </summary>
    /// <param name="items">The items for the current page.</param>
    /// <param name="totalCount">Total number of items across all pages.</param>
    /// <param name="page">Current page number (1-based).</param>
    /// <param name="pageSize">Number of items per page.</param>
    public PagedResult(IEnumerable<T> items, int totalCount, int page, int pageSize)
    {
        // Convert items to read-only array
        Items = items.ToArray();
        
        // Store pagination metadata
        TotalCount = totalCount;
        Page = page;
        PageSize = pageSize;
    }
}
