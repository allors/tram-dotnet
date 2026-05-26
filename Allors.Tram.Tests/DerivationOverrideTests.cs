// Copyright (c) Allors bv. All Rights Reserved.
// Licensed under the LGPL-3.0 License. See LICENSE in the project root for license information.

namespace Allors.Tram.Tests;

using System;
using System.Linq;
using Allors.Tram.Schema;
using Xunit;

public class DerivationOverrideTests : TestBase
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
        var greeting = m.PersonGreeting;

        var tram = this.NewTram(
            [
                new FullNameDerivation(firstName, lastName, derivedAt, fullName),
                new GreetingDerivation(fullName, greeting),
            ]);

        var john = tram.Create(person);
        tram.Set(john, firstName, "John");
        tram.Set(john, lastName, "Doe");

        tram.Derive();

        Assert.Equal("Hello John Doe!", tram.Get(john, greeting));
    }

    private class FullNameDerivation(TramAttribute firstName, TramAttribute lastName, TramAttribute derivedAt, TramAttribute fullName) : IDerivation
    {
        public void Derive(ITram tram, IChangeSet changeSet)
        {
            var firstNames = changeSet.ChangedObjects(firstName);
            var lastNames = changeSet.ChangedObjects(lastName);

            if (firstNames.Any() || lastNames.Any())
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

    private class GreetingDerivation(TramAttribute fullName, TramAttribute greeting) : IDerivation
    {
        public void Derive(ITram tram, IChangeSet changeSet)
        {
            var fullNames = changeSet.ChangedObjects(fullName);

            if (fullNames.Any())
            {
                var people = fullNames;

                foreach (var person in people)
                {
                    tram.Set(person, greeting, $"Hello {tram.Get(person, fullName)}!");
                }
            }
        }
    }
}
