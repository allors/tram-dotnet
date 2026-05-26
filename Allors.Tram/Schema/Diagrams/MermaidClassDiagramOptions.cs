// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema.Diagrams;

/// <summary>
/// Options controlling the rendering of a Mermaid class diagram from a <see cref="TramSchema"/>.
/// </summary>
public sealed record MermaidClassDiagramOptions
{
    /// <summary>
    /// An optional title rendered above the diagram.
    /// </summary>
    public string? Title { get; init; }

    /// <summary>
    /// The multiplicity label rendered on to-one ends. When null or empty, no label is rendered.
    /// </summary>
    public string? OneMultiplicity { get; init; }

    /// <summary>
    /// The multiplicity label rendered on to-many ends. When null or empty, no label is rendered.
    /// </summary>
    public string? ManyMultiplicity { get; init; }
}
