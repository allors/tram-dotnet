// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema.Config;

using System.Text.Json.Serialization;

/// <summary>
/// Configuration for a single TRAM type (value type, interface or class).
/// </summary>
public class TramTypeConfig
{
    /// <summary>
    /// The kind of type (value type, interface or class).
    /// </summary>
    [JsonPropertyName("kind")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TramTypeKind Kind { get; set; }

    /// <summary>
    /// The singular form of the type's name.
    /// </summary>
    [JsonPropertyName("singularName")]
    public required string SingularName { get; set; }

    /// <summary>
    /// The plural form of the type's name. When null, it is derived from <see cref="SingularName"/>.
    /// </summary>
    [JsonPropertyName("pluralName")]
    public string? PluralName { get; set; }

    /// <summary>
    /// Names of the interfaces this type directly inherits from. Only meaningful for classes and interfaces.
    /// </summary>
    [JsonPropertyName("directSupertypes")]
    public string[]? DirectSupertypes { get; set; }
}
