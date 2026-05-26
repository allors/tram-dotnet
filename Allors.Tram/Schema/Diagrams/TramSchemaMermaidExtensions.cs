// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema.Diagrams;

using System.Globalization;
using System.Linq;
using System.Text;

/// <summary>
/// Renders a <see cref="TramSchema"/> as a Mermaid class diagram.
/// </summary>
public static class TramSchemaMermaidExtensions
{
    /// <summary>
    /// Returns the given schema rendered as a Mermaid class diagram string.
    /// </summary>
    public static string ToMermaidClassDiagram(this TramSchema schema, MermaidClassDiagramOptions? options = null)
    {
        var diagram = new StringBuilder();

        if (options?.Title != null)
        {
            diagram.Append(CultureInfo.InvariantCulture, $"""
                            ---
                            title: {EscapeYamlDoubleQuoted(options.Title)}
                            ---

                            """);
        }

        diagram.Append("""
                       classDiagram

                       """);

        var objectTypes = schema.TypeByName.Values
            .OfType<TramObjectType>()
            .OrderBy(v => v.Name);

        foreach (var objectType in objectTypes)
        {
            diagram.Append(CultureInfo.InvariantCulture, $"""
                                class {objectType.Name}

                            """);

            var directSuperTypes = objectType.DirectSupertypes;
            foreach (var directSuperType in directSuperTypes)
            {
                diagram.Append(CultureInfo.InvariantCulture, $"""
                                    {directSuperType.Name} <|-- {objectType.Name}

                                """);
            }

            var declaredAttributes = objectType.DeclaredAttributes.OrderBy(v => v.Name);
            foreach (var attribute in declaredAttributes)
            {
                diagram.Append(CultureInfo.InvariantCulture, $"""
                                    {objectType.Name} : {attribute.Type.Name} {attribute.Name}

                                """);
            }

            var declaredRoles = objectType.DeclaredRoles.OrderBy(v => v.Name);
            foreach (var role in declaredRoles)
            {
                var inverse = role.Inverse;

                var oneMultiplicity = options?.OneMultiplicity;
                var manyMultiplicity = options?.ManyMultiplicity;

                var inverseMultiplicity = inverse.IsOne ? oneMultiplicity : manyMultiplicity;
                var roleMultiplicity = role.IsOne ? oneMultiplicity : manyMultiplicity;

                if (!string.IsNullOrWhiteSpace(inverseMultiplicity))
                {
                    inverseMultiplicity = $"\"{inverseMultiplicity}\" ";
                }

                if (!string.IsNullOrWhiteSpace(roleMultiplicity))
                {
                    roleMultiplicity = $" \"{roleMultiplicity}\"";
                }

                diagram.Append(CultureInfo.InvariantCulture, $"""
                                    {objectType.Name} {inverseMultiplicity}o--{roleMultiplicity} {role.Type.Name} : {role.Name}

                                """);
            }
        }

        return diagram.ToString();
    }

    private static string EscapeYamlDoubleQuoted(string value)
    {
        var builder = new StringBuilder(value.Length + 2);
        builder.Append('"');
        foreach (var c in value)
        {
            switch (c)
            {
                case '\\':
                    builder.Append("\\\\");
                    break;
                case '"':
                    builder.Append("\\\"");
                    break;
                case '\n':
                    builder.Append("\\n");
                    break;
                case '\r':
                    builder.Append("\\r");
                    break;
                case '\t':
                    builder.Append("\\t");
                    break;
                default:
                    builder.Append(c);
                    break;
            }
        }

        builder.Append('"');
        return builder.ToString();
    }
}
