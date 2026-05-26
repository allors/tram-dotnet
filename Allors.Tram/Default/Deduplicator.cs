// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Default;

using System;
using System.Collections.Generic;
using System.Linq;

internal sealed class Deduplicator
{
    private readonly Dictionary<Handles, Handles> deduplicatedHandlesByHandle = [];

    internal Handles Append(Handles? handles, Handle item)
    {
        if (handles is null || handles.Items.Length == 0)
        {
            return this.Deduplicate(item);
        }

        // TODO: optimize
        var appended = handles.Items.Append(item);
        return this.FromUnordered(appended);
    }

    internal Handles Concat(Handles handles, Handles items)
    {
        if (handles.Items.Length == 0)
        {
            return this.Deduplicate(items);
        }

        var concatenatedItems = handles.Items.Concat(items.Items);
        return this.FromUnordered(concatenatedItems);
    }

    internal Handles Remove(Handles handles, Handle item)
    {
        // TODO: optimize
        var removedItems = handles.Items.Where(v => v != item);
        return this.FromOrdered(removedItems);
    }

    internal Handles Except(Handles handles, Handles items)
    {
        // TODO: optimize
        var exceptedItems = handles.Items.Except(items.Items);
        return this.FromUnordered(exceptedItems);
    }

    internal Handles Intersect(Handles handles, Handles items)
    {
        // TODO: optimize
        var exceptedItems = handles.Items.Intersect(items.Items);
        return this.FromUnordered(exceptedItems);
    }

    internal Handles FromUnordered(IEnumerable<Handle> unorderedItems)
    {
        var orderedItems = unorderedItems.ToArray();
        Array.Sort(orderedItems);
        return this.Deduplicate(orderedItems);
    }

    internal Handles FromOrdered(IEnumerable<Handle> orderedItems)
    {
        return this.Deduplicate(orderedItems.ToArray());
    }

    internal Handles Deduplicate(Handle handle)
    {
        return this.Deduplicate(new[] { handle });
    }

    internal Handles Deduplicate(Handle[] items)
    {
        var hashCode = CalculateHashCode(items);
        var handles = new Handles(items, hashCode);
        return this.Deduplicate(handles);
    }

    internal Handles Deduplicate(Handles handle)
    {
        if (this.deduplicatedHandlesByHandle.TryGetValue(handle, out var dedupedHandle))
        {
            return dedupedHandle;
        }

        this.deduplicatedHandlesByHandle.Add(handle, handle);
        return handle;
    }

    private static int CalculateHashCode(Handle[] sorted)
    {
        if (sorted.Length == 0)
        {
            return 0;
        }

        var hash = default(HashCode);
        foreach (var item in sorted)
        {
            hash.Add(item);
        }

        return hash.ToHashCode();
    }
}
