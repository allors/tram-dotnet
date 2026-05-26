// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram;

/// <summary>
/// A transactional reactive memory: read/write access plus derivation.
/// </summary>
public interface ITram : IReadWrite
{
    /// <summary>
    /// Runs the registered derivations until quiescence and commits the resulting state.
    /// </summary>
    void Derive();
}
