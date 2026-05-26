// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram;

using System;
using System.Collections.Generic;

/// <summary>
/// Thrown when derivations fail to reach quiescence within the configured maximum number of cycles. The transaction is rolled back.
/// </summary>
public sealed class MaxCyclesExceededException : Exception
{
    /// <summary>
    /// Creates a new <see cref="MaxCyclesExceededException"/> with no message.
    /// </summary>
    public MaxCyclesExceededException()
    {
        this.DerivationNames = [];
    }

    /// <summary>
    /// Creates a new <see cref="MaxCyclesExceededException"/> with the given message.
    /// </summary>
    public MaxCyclesExceededException(string message)
        : base(message)
    {
        this.DerivationNames = [];
    }

    /// <summary>
    /// Creates a new <see cref="MaxCyclesExceededException"/> with the given message and inner exception.
    /// </summary>
    public MaxCyclesExceededException(string message, Exception innerException)
        : base(message, innerException)
    {
        this.DerivationNames = [];
    }

    /// <summary>
    /// Creates a new <see cref="MaxCyclesExceededException"/> describing the maximum number of cycles and the derivations that were still running when the limit was hit.
    /// </summary>
    public MaxCyclesExceededException(int maxCycles, IReadOnlyList<string> derivationNames)
        : base($"Derivation did not reach quiescence after {maxCycles} cycles. Running derivations: {string.Join(", ", derivationNames)}.")
    {
        this.MaxCycles = maxCycles;
        this.DerivationNames = derivationNames;
    }

    /// <summary>
    /// The maximum number of derivation cycles that was reached.
    /// </summary>
    public int MaxCycles { get; }

    /// <summary>
    /// The names of the derivations that were still running when the limit was reached.
    /// </summary>
    public IReadOnlyList<string> DerivationNames { get; }
}
