// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram;

using System;
using System.Globalization;

/// <summary>
/// Opaque identifier of an object in a TRAM. The zero value is the null handle.
/// </summary>
public readonly struct Handle(uint value) : IComparable<Handle>, IEquatable<Handle>
{
    private readonly uint value = value;

    /// <summary>
    /// True when this handle is the null handle.
    /// </summary>
    public bool IsNull => this.value == 0;

    /// <summary>
    /// Creates a handle from its underlying numeric identifier.
    /// </summary>
    public static implicit operator Handle(uint value)
    {
        return new Handle(value);
    }

    /// <summary>
    /// Returns the underlying numeric identifier of the handle.
    /// </summary>
    public static implicit operator uint(Handle handle)
    {
        return handle.value;
    }

    /// <summary>
    /// Returns true when <paramref name="a"/>'s identifier is strictly less than <paramref name="b"/>'s.
    /// </summary>
    public static bool operator <(Handle a, Handle b)
    {
        return a.value < b.value;
    }

    /// <summary>
    /// Returns true when <paramref name="a"/>'s identifier is strictly greater than <paramref name="b"/>'s.
    /// </summary>
    public static bool operator >(Handle a, Handle b)
    {
        return a.value > b.value;
    }

    /// <summary>
    /// Returns true when <paramref name="a"/>'s identifier is less than or equal to <paramref name="b"/>'s.
    /// </summary>
    public static bool operator <=(Handle a, Handle b)
    {
        return a.value <= b.value;
    }

    /// <summary>
    /// Returns true when <paramref name="a"/>'s identifier is greater than or equal to <paramref name="b"/>'s.
    /// </summary>
    public static bool operator >=(Handle a, Handle b)
    {
        return a.value >= b.value;
    }

    /// <summary>
    /// Returns true when the two handles refer to the same object.
    /// </summary>
    public static bool operator ==(Handle a, Handle b)
    {
        return a.Equals(b);
    }

    /// <summary>
    /// Returns true when the two handles refer to different objects.
    /// </summary>
    public static bool operator !=(Handle a, Handle b)
    {
        return !a.Equals(b);
    }

    /// <inheritdoc/>
    public int CompareTo(Handle other)
    {
        return this.value.CompareTo(other.value);
    }

    /// <inheritdoc/>
    public bool Equals(Handle other)
    {
        return this.value == other.value;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is Handle other)
        {
            return this.Equals(other);
        }

        return false;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return this.value.GetHashCode();
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return this.value.ToString(CultureInfo.InvariantCulture);
    }
}
