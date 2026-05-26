// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema.Config;

using System.Text.Json.Serialization;

/// <summary>
/// Configuration for TRAM schema data.
/// </summary>
public class TramSchemaConfig
{
    /// <summary>
    /// The types (value types, interfaces and classes) declared in the schema.
    /// </summary>
    [JsonPropertyName("types")]
    public required TramTypeConfig[] Types { get; set; }

    /// <summary>
    /// The attributes declared on the schema's object types.
    /// </summary>
    [JsonPropertyName("attributes")]
    public required TramAttributeConfig[] Attributes { get; set; }

    /// <summary>
    /// The bidirectional relationships declared on the schema's object types.
    /// </summary>
    [JsonPropertyName("relations")]
    public required TramRelationConfig[] Relations { get; set; }
}
