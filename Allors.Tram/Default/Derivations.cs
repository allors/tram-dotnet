// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Default;

using System.Collections.Generic;
using System.Linq;

internal sealed class Derivations
{
    public const int DefaultMaxCycles = 100;

    private readonly Tram tram;
    private readonly IDerivation[] derivations;
    private readonly int maxCycles;

    public Derivations(Tram tram, IEnumerable<IDerivation> derivations, int maxCycles)
    {
        this.tram = tram;
        this.derivations = derivations.ToArray();
        this.maxCycles = maxCycles;
    }

    public void Derive()
    {
        if (this.derivations.Length == 0)
        {
            this.tram.Checkpoint();
            return;
        }

        var changeSet = this.tram.Checkpoint();

        var iteration = 0;
        while (changeSet.HasChanges)
        {
            if (++iteration > this.maxCycles)
            {
                throw new MaxCyclesExceededException(
                    this.maxCycles,
                    this.derivations.Select(d => d.GetType().Name).ToArray());
            }

            foreach (var derivation in this.derivations)
            {
                derivation.Derive(this.tram, changeSet);
            }

            changeSet = this.tram.Checkpoint();
        }
    }
}
