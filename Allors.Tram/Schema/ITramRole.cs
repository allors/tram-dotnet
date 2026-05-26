// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema;

/// <summary>
/// The writable forward end of a bidirectional relationship.
/// </summary>
public interface ITramRole : ITramRelationEnd
{
    /// <summary>
    /// The read-only inverse end of this role.
    /// </summary>
    ITramInverse Inverse { get; }

    /// <summary>
    /// The fully-qualified name of this role, including its declaring type.
    /// </summary>
    string FullName { get; }
}
