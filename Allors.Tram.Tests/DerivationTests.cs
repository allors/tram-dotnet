// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Tests;

using System;
using System.Linq;
using Allors.Tram.Schema;
using Xunit;

public class DerivationTests : TestBase
{
    [Fact]
    public void Derivation()
    {
        var m = this.m;

        var person = m.Person;
        var firstName = m.PersonFirstName;
        var lastName = m.PersonLastName;
        var fullName = m.PersonFullName;
        var derivedAt = m.PersonDerivedAt;

        var tram = this.NewTram([new FullNameDerivation(firstName, lastName, derivedAt, fullName)]);

        var john = tram.Create(person);
        tram.Set(john, firstName, "John");
        tram.Set(john, lastName, "Doe");

        tram.Derive();

        Assert.Equal("John Doe", tram.Get(john, fullName));

        var fullNameDerivation = new FullNameDerivation(firstName, lastName, derivedAt, fullName);
        tram = this.NewTram(
            [
                fullNameDerivation,
                new GreetingDerivation(fullNameDerivation, firstName, lastName, fullName),
            ]);

        var jane = tram.Create(person);
        tram.Set(jane, firstName, "Jane");
        tram.Set(jane, lastName, "Doe");

        tram.Derive();

        Assert.Equal("Jane Doe Chained", tram.Get(jane, fullName));
    }

    [Fact]
    public void Derive_CyclicDerivations_ThrowsMaxCyclesExceeded()
    {
        var firstName = this.m.PersonFirstName;
        var lastName = this.m.PersonLastName;

        // Two derivations bouncing changes between firstName and lastName,
        // writing a fresh value every time so no checkpoint ever quiesces.
        var tram = this.NewTram(
            [
                new BouncingDerivation(firstName, lastName),
                new BouncingDerivation(lastName, firstName),
            ],
            maxCycles: 5);

        var john = tram.Create(this.m.Person);
        tram.Set(john, firstName, "seed");

        var exception = Assert.Throws<MaxCyclesExceededException>(() => tram.Derive());

        Assert.Equal(5, exception.MaxCycles);
        Assert.Equal(2, exception.DerivationNames.Count);
        Assert.All(exception.DerivationNames, name => Assert.Equal(nameof(BouncingDerivation), name));
    }

    [Fact]
    public void Derive_CyclicDerivations_AutomaticallyRollsBack()
    {
        var firstName = this.m.PersonFirstName;
        var lastName = this.m.PersonLastName;

        var tram = this.NewTram(
            [
                new BouncingDerivation(firstName, lastName),
                new BouncingDerivation(lastName, firstName),
            ],
            maxCycles: 5);

        var john = tram.Create(this.m.Person);
        tram.Set(john, firstName, "seed");

        Assert.Throws<MaxCyclesExceededException>(() => tram.Derive());

        // After the throw the transaction is fully rolled back: no committed object exists.
        Assert.False(tram.Exists(john));

        // A second Derive() must not silently commit partial derivation state.
        tram.Derive();
        Assert.False(tram.Exists(john));
    }

    private class BouncingDerivation(TramAttribute trigger, TramAttribute target) : IDerivation
    {
        private int counter;

        public void Derive(ITram tram, IChangeSet changeSet)
        {
            foreach (var @object in changeSet.ChangedObjects(trigger))
            {
                tram.Set(@object, target, $"v{++this.counter}");
            }
        }
    }

    private class FullNameDerivation(TramAttribute firstName, TramAttribute lastName, TramAttribute derivedAt, TramAttribute fullName) : IDerivation
    {
        public void Derive(ITram tram, IChangeSet changeSet)
        {
            var firstNames = changeSet.ChangedObjects(firstName);
            var lastNames = changeSet.ChangedObjects(lastName);

            if (firstNames.Any() && lastNames.Any())
            {
                var people = firstNames.Union(lastNames);

                foreach (var person in people)
                {
                    // Dummy updates ...
                    tram.Set(person, firstName, tram.Get(person, firstName));
                    tram.Set(person, lastName, tram.Get(person, lastName));

                    tram.Set(person, derivedAt, DateTime.Now);

                    tram.Set(person, fullName, $"{tram.Get(person, firstName)} {tram.Get(person, lastName)}");
                }
            }
        }
    }

    private class GreetingDerivation(FullNameDerivation fullNameDerivation, TramAttribute firstName, TramAttribute lastName, TramAttribute fullName) : IDerivation
    {
        public void Derive(ITram tram, IChangeSet changeSet)
        {
            fullNameDerivation.Derive(tram, changeSet);

            var firstNames = changeSet.ChangedObjects(firstName);
            var lastNames = changeSet.ChangedObjects(lastName);

            if (firstNames.Any() || lastNames.Any())
            {
                var people = firstNames.Union(lastNames);

                foreach (var person in people)
                {
                    tram.Set(person, fullName, $"{tram.Get(person, fullName)} Chained");
                }
            }
        }
    }
}
