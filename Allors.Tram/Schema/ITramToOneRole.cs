// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema;

/// <summary>
/// The writable, single-valued (to-one) end of a bidirectional relationship.
/// </summary>
public interface ITramToOneRole : ITramRole, ITramToOneRelationEnd;
