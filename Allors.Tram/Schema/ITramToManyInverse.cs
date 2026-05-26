// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema;

/// <summary>
/// The read-only, multi-valued (to-many) reverse end of a bidirectional relationship.
/// </summary>
public interface ITramToManyInverse : ITramInverse, ITramToManyRelationEnd;
