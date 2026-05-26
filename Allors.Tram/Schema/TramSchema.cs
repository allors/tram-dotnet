// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Schema;

using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The set of types, attributes and relationships that defines the shape of a TRAM.
/// </summary>
public sealed class TramSchema
{
    /// <summary>
    /// Creates a new <see cref="TramSchema"/> from the given configuration.
    /// </summary>
    public TramSchema(Config.TramSchemaConfig cfg)
    {
        Dictionary<string, TramType> typeByName = [];

        foreach (var typeConfig in cfg.Types)
        {
            switch (typeConfig.Kind)
            {
            case Config.TramTypeKind.ValueType:
                var valueType = new TramValueType(this, typeConfig.SingularName, typeConfig.PluralName ?? Pluralize(typeConfig.SingularName));
                typeByName.Add(valueType.Name, valueType);
                break;
            case Config.TramTypeKind.Interface:
                var @interface = new TramInterface(this, typeConfig.SingularName, typeConfig.PluralName ?? Pluralize(typeConfig.SingularName));
                typeByName.Add(@interface.Name, @interface);
                break;
            case Config.TramTypeKind.Class:
                var @class = new TramClass(this, typeConfig.SingularName, typeConfig.PluralName ?? Pluralize(typeConfig.SingularName));
                typeByName.Add(@class.Name, @class);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(cfg), typeConfig.Kind, $"unknown TramTypeKind {typeConfig.Kind}");
            }
        }

        foreach (var typeConfig in cfg.Types)
        {
            if (typeConfig.Kind is not (Config.TramTypeKind.Class or Config.TramTypeKind.Interface))
            {
                continue;
            }

            if (!(typeConfig.DirectSupertypes?.Length > 0))
            {
                continue;
            }

            var @class = (TramObjectType)typeByName[typeConfig.SingularName];
            foreach (var supertype in typeConfig.DirectSupertypes.Select(v => (TramInterface)typeByName[v]))
            {
                @class.AddDirectSupertype(supertype);
            }
        }

        foreach (var attributeConfig in cfg.Attributes)
        {
            var declaringType = (TramObjectType)typeByName[attributeConfig.DeclaringType];
            var type = (TramValueType)typeByName[attributeConfig.Type];
            declaringType.AddValueType(type, attributeConfig.SingularName, attributeConfig.PluralName);
        }

        foreach (var relationConfig in cfg.Relations)
        {
            var directObjectType = (TramObjectType)typeByName[relationConfig.DeclaringType];
            var inverseObjectType = (TramObjectType)typeByName[relationConfig.Type];

            switch (relationConfig.Multiplicity)
            {
            case TramMultiplicity.OneToOne:
                directObjectType.AddOneToOne(inverseObjectType, relationConfig.SingularName, relationConfig.PluralName);
                break;
            case TramMultiplicity.ManyToOne:
                directObjectType.AddManyToOne(inverseObjectType, relationConfig.SingularName, relationConfig.PluralName);
                break;
            case TramMultiplicity.ManyToMany:
                directObjectType.AddManyToMany(inverseObjectType, relationConfig.SingularName, relationConfig.PluralName);
                break;
            case null:
            case TramMultiplicity.OneToMany:
                directObjectType.AddOneToMany(inverseObjectType, relationConfig.SingularName, relationConfig.PluralName);
                break;
            default:
                throw new ArgumentOutOfRangeException($"Unknown multiplicity {relationConfig.Multiplicity}");
            }
        }

        this.TypeByName = typeByName.ToFrozenDictionary();
    }

    /// <summary>
    /// The types in this schema indexed by their singular name.
    /// </summary>
    public IReadOnlyDictionary<string, TramType> TypeByName { get; }

    internal static string Pluralize(string singular)
    {
        static bool EndsWith(string word, string ending)
        {
            return word.EndsWith(ending, StringComparison.InvariantCultureIgnoreCase);
        }

        if (EndsWith(singular, "y") &&
            !EndsWith(singular, "ay") &&
            !EndsWith(singular, "ey") &&
            !EndsWith(singular, "iy") &&
            !EndsWith(singular, "oy") &&
            !EndsWith(singular, "uy"))
        {
            return string.Concat(singular.AsSpan(0, singular.Length - 1), "ies");
        }

        if (EndsWith(singular, "us"))
        {
            return singular + "es";
        }

        if (EndsWith(singular, "ss"))
        {
            return singular + "es";
        }

        if (EndsWith(singular, "x") ||
            EndsWith(singular, "ch") ||
            EndsWith(singular, "sh"))
        {
            return singular + "es";
        }

        if (EndsWith(singular, "f") && singular.Length > 1)
        {
            return string.Concat(singular.AsSpan(0, singular.Length - 1), "ves");
        }

        if (EndsWith(singular, "fe") && singular.Length > 2)
        {
            return string.Concat(singular.AsSpan(0, singular.Length - 2), "ves");
        }

        return singular + "s";
    }
}
