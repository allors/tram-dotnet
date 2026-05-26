// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Tests;

using System.Collections.Generic;
using Allors.Tram.Default;
using Allors.Tram.Schema;
using Allors.Tram.Tests.Fixture;

public abstract class TestBase
{
    protected TramSchema schema => SchemaFixture.Schema;

    protected TestsM m => SchemaFixture.M;

    protected Tram NewTram() => new Tram(this.schema);

    protected Tram NewTram(IEnumerable<IDerivation> derivations, int maxCycles = 100)
        => new Tram(this.schema, derivations, maxCycles);
}
