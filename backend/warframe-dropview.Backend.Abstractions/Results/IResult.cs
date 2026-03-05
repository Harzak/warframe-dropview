using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace warframe_dropview.Backend.Abstractions.Results;

/// <summary>
/// Defines the contract for operation results that indicate success or failure status.
/// </summary>
public interface IResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the operation completed successfully.
    /// </summary>
    bool IsSuccess { get; set; }
}

/// <summary>
/// Defines the contract for operation results that contain strongly-typed content along with success status.
/// </summary>
public interface IResult<T> : IResult
{
    /// <summary>
    /// Gets the strongly-typed content of the operation result.
    /// </summary>
    T Content { get; }
}