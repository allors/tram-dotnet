// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema.Config;

using System.Text.Json.Serialization;

/// <summary>
/// Configuration for a single attribute declared on an object type.
/// </summary>
public class TramAttributeConfig
{
    /// <summary>
    /// Name of the object type on which the attribute is declared.
    /// </summary>
    [JsonPropertyName("declaringType")]
    public required string DeclaringType { get; set; }

    /// <summary>
    /// Name of the value type that constrains the attribute's value.
    /// </summary>
    [JsonPropertyName("type")]
    public required string Type { get; set; }

    /// <summary>
    /// The singular form of the attribute's name.
    /// </summary>
    [JsonPropertyName("singularName")]
    public required string SingularName { get; set; }

    /// <summary>
    /// The plural form of the attribute's name. When null, it is derived from <see cref="SingularName"/>.
    /// </summary>
    [JsonPropertyName("pluralName")]
    public string? PluralName { get; set; }
}
