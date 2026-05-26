// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Default;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

internal sealed class Handles : IReadOnlyCollection<Handle>, IEquatable<Handles>
{
    public static readonly Handles Empty = new([], 0);

    private readonly int hashCode;

    internal Handles(Handle[] items, int hashCode)
    {
        this.Items = items;
        this.hashCode = hashCode;
    }

    public int Count => this.Items.Length;

    internal Handle[] Items { get; }

    internal bool IsEmpty => this.Items.Length == 0;

    public static bool operator ==(Handles? left, Handles? right) => Equals(left, right);

    public static bool operator !=(Handles? left, Handles? right) => !Equals(left, right);

    public IEnumerator<Handle> GetEnumerator() => this.Items.AsEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)this.Items).GetEnumerator();

    public bool Equals(Handles? other) =>
        other is not null
        && this.hashCode == other.hashCode
        && this.Items.AsSpan().SequenceEqual(other.Items);

    public override bool Equals(object? obj) => obj is Handles other && this.Equals(other);

    public override int GetHashCode() => this.hashCode;

    internal bool Contains(Handle handle) => Array.BinarySearch(this.Items, handle) >= 0;
}
