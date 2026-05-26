// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram;

/// <summary>
/// A single derivation step that observes a change set and writes derived state back to the TRAM.
/// </summary>
public interface IDerivation
{
    /// <summary>
    /// Runs the derivation against the given TRAM and change set.
    /// </summary>
    void Derive(ITram tram, IChangeSet changeSet);
}
