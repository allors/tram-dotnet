// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

// SchemaFixture caches an immutable TramSchema and TestsM that all tests share.
// Each test still constructs its own Tram, so test collections are safe to run
// in parallel. Make the intent explicit so a future contributor doesn't disable it.
[assembly: Xunit.CollectionBehavior(DisableTestParallelization = false)]
