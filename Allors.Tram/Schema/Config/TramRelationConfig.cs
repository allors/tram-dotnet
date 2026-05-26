// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema.Config;

using System.Text.Json.Serialization;

/// <summary>
/// Configuration for a single bidirectional relationship declared on an object type.
/// </summary>
public class TramRelationConfig
{
    /// <summary>
    /// Name of the object type on which the role is declared.
    /// </summary>
    [JsonPropertyName("declaringType")]
    public required string DeclaringType { get; set; }

    /// <summary>
    /// Name of the object type that the relation points at.
    /// </summary>
    [JsonPropertyName("type")]
    public required string Type { get; set; }

    /// <summary>
    /// The singular form of the role's name. When null, it is derived from <see cref="Type"/>.
    /// </summary>
    [JsonPropertyName("singularName")]
    public string? SingularName { get; set; }

    /// <summary>
    /// The plural form of the role's name. When null, it is derived from <see cref="SingularName"/>.
    /// </summary>
    [JsonPropertyName("pluralName")]
    public string? PluralName { get; set; }

    /// <summary>
    /// The cardinality of the relationship. Defaults to <see cref="TramMultiplicity.OneToMany"/> when null.
    /// </summary>
    [JsonPropertyName("multiplicity")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TramMultiplicity? Multiplicity { get; set; }
}
