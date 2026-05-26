namespace Allors.Tram.Tests.Fixture;

using Allors.Tram.Schema;

public sealed class TestsM
{
    public TestsM(TramSchema m)
    {
        // Types
        this.Boolean = (TramValueType)m.TypeByName["Boolean"];
        this.C1 = (TramClass)m.TypeByName["C1"];
        this.C2 = (TramClass)m.TypeByName["C2"];
        this.C3 = (TramClass)m.TypeByName["C3"];
        this.C4 = (TramClass)m.TypeByName["C4"];
        this.DateTime = (TramValueType)m.TypeByName["DateTime"];
        this.I1 = (TramInterface)m.TypeByName["I1"];
        this.I12 = (TramInterface)m.TypeByName["I12"];
        this.I2 = (TramInterface)m.TypeByName["I2"];
        this.Integer = (TramValueType)m.TypeByName["Integer"];
        this.Organization = (TramClass)m.TypeByName["Organization"];
        this.Person = (TramClass)m.TypeByName["Person"];
        this.S1 = (TramInterface)m.TypeByName["S1"];
        this.S2 = (TramInterface)m.TypeByName["S2"];
        this.String = (TramValueType)m.TypeByName["String"];

        // Attributes
        this.C1Same = (TramAttribute)this.C1.PropertyBySingularOrPluralName["Same"];
        this.C1C1AllorsString = (TramAttribute)this.C1.PropertyBySingularOrPluralName["C1AllorsString"];
        this.C1SameAllorsString = (TramAttribute)this.C1.PropertyBySingularOrPluralName["SameAllorsString"];
        this.C1C1AllorsInteger = (TramAttribute)this.C1.PropertyBySingularOrPluralName["C1AllorsInteger"];
        this.C1SameAllorsInteger = (TramAttribute)this.C1.PropertyBySingularOrPluralName["SameAllorsInteger"];
        this.C1C1AllorsBoolean = (TramAttribute)this.C1.PropertyBySingularOrPluralName["C1AllorsBoolean"];
        this.C1SameAllorsBoolean = (TramAttribute)this.C1.PropertyBySingularOrPluralName["SameAllorsBoolean"];
        this.C2Same = (TramAttribute)this.C2.PropertyBySingularOrPluralName["Same"];
        this.C2C2AllorsString = (TramAttribute)this.C2.PropertyBySingularOrPluralName["C2AllorsString"];
        this.C2SameAllorsString = (TramAttribute)this.C2.PropertyBySingularOrPluralName["SameAllorsString"];
        this.C2C2AllorsInteger = (TramAttribute)this.C2.PropertyBySingularOrPluralName["C2AllorsInteger"];
        this.C2SameAllorsInteger = (TramAttribute)this.C2.PropertyBySingularOrPluralName["SameAllorsInteger"];
        this.C2C2AllorsBoolean = (TramAttribute)this.C2.PropertyBySingularOrPluralName["C2AllorsBoolean"];
        this.C2SameAllorsBoolean = (TramAttribute)this.C2.PropertyBySingularOrPluralName["SameAllorsBoolean"];
        this.C3C3AllorsString = (TramAttribute)this.C3.PropertyBySingularOrPluralName["C3AllorsString"];
        this.C3SameAllorsString = (TramAttribute)this.C3.PropertyBySingularOrPluralName["SameAllorsString"];
        this.C3C3AllorsInteger = (TramAttribute)this.C3.PropertyBySingularOrPluralName["C3AllorsInteger"];
        this.C3SameAllorsInteger = (TramAttribute)this.C3.PropertyBySingularOrPluralName["SameAllorsInteger"];
        this.C3C3AllorsBoolean = (TramAttribute)this.C3.PropertyBySingularOrPluralName["C3AllorsBoolean"];
        this.C3SameAllorsBoolean = (TramAttribute)this.C3.PropertyBySingularOrPluralName["SameAllorsBoolean"];
        this.C4C4AllorsString = (TramAttribute)this.C4.PropertyBySingularOrPluralName["C4AllorsString"];
        this.C4SameAllorsString = (TramAttribute)this.C4.PropertyBySingularOrPluralName["SameAllorsString"];
        this.C4C4AllorsInteger = (TramAttribute)this.C4.PropertyBySingularOrPluralName["C4AllorsInteger"];
        this.C4SameAllorsInteger = (TramAttribute)this.C4.PropertyBySingularOrPluralName["SameAllorsInteger"];
        this.C4C4AllorsBoolean = (TramAttribute)this.C4.PropertyBySingularOrPluralName["C4AllorsBoolean"];
        this.C4SameAllorsBoolean = (TramAttribute)this.C4.PropertyBySingularOrPluralName["SameAllorsBoolean"];
        this.I1I1AllorsString = (TramAttribute)this.I1.PropertyBySingularOrPluralName["I1AllorsString"];
        this.I1I1AllorsInteger = (TramAttribute)this.I1.PropertyBySingularOrPluralName["I1AllorsInteger"];
        this.I1I1AllorsBoolean = (TramAttribute)this.I1.PropertyBySingularOrPluralName["I1AllorsBoolean"];
        this.I12I12AllorsString = (TramAttribute)this.I12.PropertyBySingularOrPluralName["I12AllorsString"];
        this.I12I12AllorsInteger = (TramAttribute)this.I12.PropertyBySingularOrPluralName["I12AllorsInteger"];
        this.I12I12AllorsBoolean = (TramAttribute)this.I12.PropertyBySingularOrPluralName["I12AllorsBoolean"];
        this.I2I2AllorsString = (TramAttribute)this.I2.PropertyBySingularOrPluralName["I2AllorsString"];
        this.I2I2AllorsInteger = (TramAttribute)this.I2.PropertyBySingularOrPluralName["I2AllorsInteger"];
        this.I2I2AllorsBoolean = (TramAttribute)this.I2.PropertyBySingularOrPluralName["I2AllorsBoolean"];
        this.OrganizationName = (TramAttribute)this.Organization.PropertyBySingularOrPluralName["Name"];
        this.PersonFirstName = (TramAttribute)this.Person.PropertyBySingularOrPluralName["FirstName"];
        this.PersonLastName = (TramAttribute)this.Person.PropertyBySingularOrPluralName["LastName"];
        this.PersonFullName = (TramAttribute)this.Person.PropertyBySingularOrPluralName["FullName"];
        this.PersonDerivedAt = (TramAttribute)this.Person.PropertyBySingularOrPluralName["DerivedAt"];
        this.PersonGreeting = (TramAttribute)this.Person.PropertyBySingularOrPluralName["Greeting"];
        this.S1S1AllorsString = (TramAttribute)this.S1.PropertyBySingularOrPluralName["S1AllorsString"];
        this.S1S1AllorsInteger = (TramAttribute)this.S1.PropertyBySingularOrPluralName["S1AllorsInteger"];
        this.S1S1AllorsBoolean = (TramAttribute)this.S1.PropertyBySingularOrPluralName["S1AllorsBoolean"];
        this.S2S2AllorsString = (TramAttribute)this.S2.PropertyBySingularOrPluralName["S2AllorsString"];
        this.S2S2AllorsInteger = (TramAttribute)this.S2.PropertyBySingularOrPluralName["S2AllorsInteger"];
        this.S2S2AllorsBoolean = (TramAttribute)this.S2.PropertyBySingularOrPluralName["S2AllorsBoolean"];

        // RelationEnds
        this.C1C1C1OneToOne = (TramOneToOneRole)this.C1.PropertyBySingularOrPluralName["C1C1OneToOne"];
        this.C1C1C2OneToOne = (TramOneToOneRole)this.C1.PropertyBySingularOrPluralName["C1C2OneToOne"];
        this.C1C1C3OneToOne = (TramOneToOneRole)this.C1.PropertyBySingularOrPluralName["C1C3OneToOne"];
        this.C1C1C4OneToOne = (TramOneToOneRole)this.C1.PropertyBySingularOrPluralName["C1C4OneToOne"];
        this.C1C1I1OneToOne = (TramOneToOneRole)this.C1.PropertyBySingularOrPluralName["C1I1OneToOne"];
        this.C1C1I2OneToOne = (TramOneToOneRole)this.C1.PropertyBySingularOrPluralName["C1I2OneToOne"];
        this.C1C1I12OneToOne = (TramOneToOneRole)this.C1.PropertyBySingularOrPluralName["C1I12OneToOne"];
        this.C1C1S1OneToOne = (TramOneToOneRole)this.C1.PropertyBySingularOrPluralName["C1S1OneToOne"];
        this.C1C1S2OneToOne = (TramOneToOneRole)this.C1.PropertyBySingularOrPluralName["C1S2OneToOne"];
        this.C1C1C1OneToManies = (TramOneToManyRole)this.C1.PropertyBySingularOrPluralName["C1C1OneToMany"];
        this.C1C1C2OneToManies = (TramOneToManyRole)this.C1.PropertyBySingularOrPluralName["C1C2OneToMany"];
        this.C1C1C3OneToManies = (TramOneToManyRole)this.C1.PropertyBySingularOrPluralName["C1C3OneToMany"];
        this.C1C1C4OneToManies = (TramOneToManyRole)this.C1.PropertyBySingularOrPluralName["C1C4OneToMany"];
        this.C1C1I1OneToManies = (TramOneToManyRole)this.C1.PropertyBySingularOrPluralName["C1I1OneToMany"];
        this.C1C1I2OneToManies = (TramOneToManyRole)this.C1.PropertyBySingularOrPluralName["C1I2OneToMany"];
        this.C1C1I12OneToManies = (TramOneToManyRole)this.C1.PropertyBySingularOrPluralName["C1I12OneToMany"];
        this.C1C1S1OneToManies = (TramOneToManyRole)this.C1.PropertyBySingularOrPluralName["C1S1OneToMany"];
        this.C1C1S2OneToManies = (TramOneToManyRole)this.C1.PropertyBySingularOrPluralName["C1S2OneToMany"];
        this.C1C1C1ManyToOne = (TramManyToOneRole)this.C1.PropertyBySingularOrPluralName["C1C1ManyToOne"];
        this.C1C1C2ManyToOne = (TramManyToOneRole)this.C1.PropertyBySingularOrPluralName["C1C2ManyToOne"];
        this.C1C1C3ManyToOne = (TramManyToOneRole)this.C1.PropertyBySingularOrPluralName["C1C3ManyToOne"];
        this.C1C1C4ManyToOne = (TramManyToOneRole)this.C1.PropertyBySingularOrPluralName["C1C4ManyToOne"];
        this.C1C1I1ManyToOne = (TramManyToOneRole)this.C1.PropertyBySingularOrPluralName["C1I1ManyToOne"];
        this.C1C1I2ManyToOne = (TramManyToOneRole)this.C1.PropertyBySingularOrPluralName["C1I2ManyToOne"];
        this.C1C1I12ManyToOne = (TramManyToOneRole)this.C1.PropertyBySingularOrPluralName["C1I12ManyToOne"];
        this.C1C1S1ManyToOne = (TramManyToOneRole)this.C1.PropertyBySingularOrPluralName["C1S1ManyToOne"];
        this.C1C1S2ManyToOne = (TramManyToOneRole)this.C1.PropertyBySingularOrPluralName["C1S2ManyToOne"];
        this.C1C1C1ManyToManies = (TramManyToManyRole)this.C1.PropertyBySingularOrPluralName["C1C1ManyToMany"];
        this.C1C1C2ManyToManies = (TramManyToManyRole)this.C1.PropertyBySingularOrPluralName["C1C2ManyToMany"];
        this.C1C1C3ManyToManies = (TramManyToManyRole)this.C1.PropertyBySingularOrPluralName["C1C3ManyToMany"];
        this.C1C1C4ManyToManies = (TramManyToManyRole)this.C1.PropertyBySingularOrPluralName["C1C4ManyToMany"];
        this.C1C1I1ManyToManies = (TramManyToManyRole)this.C1.PropertyBySingularOrPluralName["C1I1ManyToMany"];
        this.C1C1I2ManyToManies = (TramManyToManyRole)this.C1.PropertyBySingularOrPluralName["C1I2ManyToMany"];
        this.C1C1I12ManyToManies = (TramManyToManyRole)this.C1.PropertyBySingularOrPluralName["C1I12ManyToMany"];
        this.C1C1S1ManyToManies = (TramManyToManyRole)this.C1.PropertyBySingularOrPluralName["C1S1ManyToMany"];
        this.C1C1S2ManyToManies = (TramManyToManyRole)this.C1.PropertyBySingularOrPluralName["C1S2ManyToMany"];
        this.C3C3C1OneToOne = (TramOneToOneRole)this.C3.PropertyBySingularOrPluralName["C3C1OneToOne"];
        this.C3C3C2OneToOne = (TramOneToOneRole)this.C3.PropertyBySingularOrPluralName["C3C2OneToOne"];
        this.C3C3C3OneToOne = (TramOneToOneRole)this.C3.PropertyBySingularOrPluralName["C3C3OneToOne"];
        this.C3C3C4OneToOne = (TramOneToOneRole)this.C3.PropertyBySingularOrPluralName["C3C4OneToOne"];
        this.C3C3I1OneToOne = (TramOneToOneRole)this.C3.PropertyBySingularOrPluralName["C3I1OneToOne"];
        this.C3C3I2OneToOne = (TramOneToOneRole)this.C3.PropertyBySingularOrPluralName["C3I2OneToOne"];
        this.C3C3I12OneToOne = (TramOneToOneRole)this.C3.PropertyBySingularOrPluralName["C3I12OneToOne"];
        this.C3C3S1OneToOne = (TramOneToOneRole)this.C3.PropertyBySingularOrPluralName["C3S1OneToOne"];
        this.C3C3S2OneToOne = (TramOneToOneRole)this.C3.PropertyBySingularOrPluralName["C3S2OneToOne"];
        this.C3C3C1OneToManies = (TramOneToManyRole)this.C3.PropertyBySingularOrPluralName["C3C1OneToMany"];
        this.C3C3C2OneToManies = (TramOneToManyRole)this.C3.PropertyBySingularOrPluralName["C3C2OneToMany"];
        this.C3C3C3OneToManies = (TramOneToManyRole)this.C3.PropertyBySingularOrPluralName["C3C3OneToMany"];
        this.C3C3C4OneToManies = (TramOneToManyRole)this.C3.PropertyBySingularOrPluralName["C3C4OneToMany"];
        this.C3C3I1OneToManies = (TramOneToManyRole)this.C3.PropertyBySingularOrPluralName["C3I1OneToMany"];
        this.C3C3I2OneToManies = (TramOneToManyRole)this.C3.PropertyBySingularOrPluralName["C3I2OneToMany"];
        this.C3C3I12OneToManies = (TramOneToManyRole)this.C3.PropertyBySingularOrPluralName["C3I12OneToMany"];
        this.C3C3S1OneToManies = (TramOneToManyRole)this.C3.PropertyBySingularOrPluralName["C3S1OneToMany"];
        this.C3C3S2OneToManies = (TramOneToManyRole)this.C3.PropertyBySingularOrPluralName["C3S2OneToMany"];
        this.C3C3C1ManyToOne = (TramManyToOneRole)this.C3.PropertyBySingularOrPluralName["C3C1ManyToOne"];
        this.C3C3C2ManyToOne = (TramManyToOneRole)this.C3.PropertyBySingularOrPluralName["C3C2ManyToOne"];
        this.C3C3C3ManyToOne = (TramManyToOneRole)this.C3.PropertyBySingularOrPluralName["C3C3ManyToOne"];
        this.C3C3C4ManyToOne = (TramManyToOneRole)this.C3.PropertyBySingularOrPluralName["C3C4ManyToOne"];
        this.C3C3I1ManyToOne = (TramManyToOneRole)this.C3.PropertyBySingularOrPluralName["C3I1ManyToOne"];
        this.C3C3I2ManyToOne = (TramManyToOneRole)this.C3.PropertyBySingularOrPluralName["C3I2ManyToOne"];
        this.C3C3I12ManyToOne = (TramManyToOneRole)this.C3.PropertyBySingularOrPluralName["C3I12ManyToOne"];
        this.C3C3S1ManyToOne = (TramManyToOneRole)this.C3.PropertyBySingularOrPluralName["C3S1ManyToOne"];
        this.C3C3S2ManyToOne = (TramManyToOneRole)this.C3.PropertyBySingularOrPluralName["C3S2ManyToOne"];
        this.C3C3C1ManyToManies = (TramManyToManyRole)this.C3.PropertyBySingularOrPluralName["C3C1ManyToMany"];
        this.C3C3C2ManyToManies = (TramManyToManyRole)this.C3.PropertyBySingularOrPluralName["C3C2ManyToMany"];
        this.C3C3C3ManyToManies = (TramManyToManyRole)this.C3.PropertyBySingularOrPluralName["C3C3ManyToMany"];
        this.C3C3C4ManyToManies = (TramManyToManyRole)this.C3.PropertyBySingularOrPluralName["C3C4ManyToMany"];
        this.C3C3I1ManyToManies = (TramManyToManyRole)this.C3.PropertyBySingularOrPluralName["C3I1ManyToMany"];
        this.C3C3I2ManyToManies = (TramManyToManyRole)this.C3.PropertyBySingularOrPluralName["C3I2ManyToMany"];
        this.C3C3I12ManyToManies = (TramManyToManyRole)this.C3.PropertyBySingularOrPluralName["C3I12ManyToMany"];
        this.C3C3S1ManyToManies = (TramManyToManyRole)this.C3.PropertyBySingularOrPluralName["C3S1ManyToMany"];
        this.C3C3S2ManyToManies = (TramManyToManyRole)this.C3.PropertyBySingularOrPluralName["C3S2ManyToMany"];
        this.OrganizationOwner = (TramOneToOneRole)this.Organization.PropertyBySingularOrPluralName["Owner"];
        this.OrganizationAccountant = (TramManyToOneRole)this.Organization.PropertyBySingularOrPluralName["Accountant"];
        this.OrganizationEmployees = (TramOneToManyRole)this.Organization.PropertyBySingularOrPluralName["Employee"];
        this.OrganizationCustomers = (TramManyToManyRole)this.Organization.PropertyBySingularOrPluralName["Customer"];

        // Inverses
        this.C1WhereC1C1OneToOne = (TramOneToOneInverse)this.C1.PropertyBySingularOrPluralName["C1WhereC1C1OneToOne"];
        this.C3WhereC3C1OneToOne = (TramOneToOneInverse)this.C1.PropertyBySingularOrPluralName["C3WhereC3C1OneToOne"];
        this.C1WhereC1C1OneToMany = (TramOneToManyInverse)this.C1.PropertyBySingularOrPluralName["C1WhereC1C1OneToMany"];
        this.C3WhereC3C1OneToMany = (TramOneToManyInverse)this.C1.PropertyBySingularOrPluralName["C3WhereC3C1OneToMany"];
        this.C1sWhereC1C1ManyToOne = (TramManyToOneInverse)this.C1.PropertyBySingularOrPluralName["C1WhereC1C1ManyToOne"];
        this.C3sWhereC3C1ManyToOne = (TramManyToOneInverse)this.C1.PropertyBySingularOrPluralName["C3WhereC3C1ManyToOne"];
        this.C1sWhereC1C1ManyToMany = (TramManyToManyInverse)this.C1.PropertyBySingularOrPluralName["C1WhereC1C1ManyToMany"];
        this.C3sWhereC3C1ManyToMany = (TramManyToManyInverse)this.C1.PropertyBySingularOrPluralName["C3WhereC3C1ManyToMany"];
        this.C1WhereC1C2OneToOne = (TramOneToOneInverse)this.C2.PropertyBySingularOrPluralName["C1WhereC1C2OneToOne"];
        this.C3WhereC3C2OneToOne = (TramOneToOneInverse)this.C2.PropertyBySingularOrPluralName["C3WhereC3C2OneToOne"];
        this.C1WhereC1C2OneToMany = (TramOneToManyInverse)this.C2.PropertyBySingularOrPluralName["C1WhereC1C2OneToMany"];
        this.C3WhereC3C2OneToMany = (TramOneToManyInverse)this.C2.PropertyBySingularOrPluralName["C3WhereC3C2OneToMany"];
        this.C1sWhereC1C2ManyToOne = (TramManyToOneInverse)this.C2.PropertyBySingularOrPluralName["C1WhereC1C2ManyToOne"];
        this.C3sWhereC3C2ManyToOne = (TramManyToOneInverse)this.C2.PropertyBySingularOrPluralName["C3WhereC3C2ManyToOne"];
        this.C1sWhereC1C2ManyToMany = (TramManyToManyInverse)this.C2.PropertyBySingularOrPluralName["C1WhereC1C2ManyToMany"];
        this.C3sWhereC3C2ManyToMany = (TramManyToManyInverse)this.C2.PropertyBySingularOrPluralName["C3WhereC3C2ManyToMany"];
        this.C1WhereC1C3OneToOne = (TramOneToOneInverse)this.C3.PropertyBySingularOrPluralName["C1WhereC1C3OneToOne"];
        this.C3WhereC3C3OneToOne = (TramOneToOneInverse)this.C3.PropertyBySingularOrPluralName["C3WhereC3C3OneToOne"];
        this.C1WhereC1C3OneToMany = (TramOneToManyInverse)this.C3.PropertyBySingularOrPluralName["C1WhereC1C3OneToMany"];
        this.C3WhereC3C3OneToMany = (TramOneToManyInverse)this.C3.PropertyBySingularOrPluralName["C3WhereC3C3OneToMany"];
        this.C1sWhereC1C3ManyToOne = (TramManyToOneInverse)this.C3.PropertyBySingularOrPluralName["C1WhereC1C3ManyToOne"];
        this.C3sWhereC3C3ManyToOne = (TramManyToOneInverse)this.C3.PropertyBySingularOrPluralName["C3WhereC3C3ManyToOne"];
        this.C1sWhereC1C3ManyToMany = (TramManyToManyInverse)this.C3.PropertyBySingularOrPluralName["C1WhereC1C3ManyToMany"];
        this.C3sWhereC3C3ManyToMany = (TramManyToManyInverse)this.C3.PropertyBySingularOrPluralName["C3WhereC3C3ManyToMany"];
        this.C1WhereC1C4OneToOne = (TramOneToOneInverse)this.C4.PropertyBySingularOrPluralName["C1WhereC1C4OneToOne"];
        this.C3WhereC3C4OneToOne = (TramOneToOneInverse)this.C4.PropertyBySingularOrPluralName["C3WhereC3C4OneToOne"];
        this.C1WhereC1C4OneToMany = (TramOneToManyInverse)this.C4.PropertyBySingularOrPluralName["C1WhereC1C4OneToMany"];
        this.C3WhereC3C4OneToMany = (TramOneToManyInverse)this.C4.PropertyBySingularOrPluralName["C3WhereC3C4OneToMany"];
        this.C1sWhereC1C4ManyToOne = (TramManyToOneInverse)this.C4.PropertyBySingularOrPluralName["C1WhereC1C4ManyToOne"];
        this.C3sWhereC3C4ManyToOne = (TramManyToOneInverse)this.C4.PropertyBySingularOrPluralName["C3WhereC3C4ManyToOne"];
        this.C1sWhereC1C4ManyToMany = (TramManyToManyInverse)this.C4.PropertyBySingularOrPluralName["C1WhereC1C4ManyToMany"];
        this.C3sWhereC3C4ManyToMany = (TramManyToManyInverse)this.C4.PropertyBySingularOrPluralName["C3WhereC3C4ManyToMany"];
        this.C1WhereC1I1OneToOne = (TramOneToOneInverse)this.I1.PropertyBySingularOrPluralName["C1WhereC1I1OneToOne"];
        this.C3WhereC3I1OneToOne = (TramOneToOneInverse)this.I1.PropertyBySingularOrPluralName["C3WhereC3I1OneToOne"];
        this.C1WhereC1I1OneToMany = (TramOneToManyInverse)this.I1.PropertyBySingularOrPluralName["C1WhereC1I1OneToMany"];
        this.C3WhereC3I1OneToMany = (TramOneToManyInverse)this.I1.PropertyBySingularOrPluralName["C3WhereC3I1OneToMany"];
        this.C1sWhereC1I1ManyToOne = (TramManyToOneInverse)this.I1.PropertyBySingularOrPluralName["C1WhereC1I1ManyToOne"];
        this.C3sWhereC3I1ManyToOne = (TramManyToOneInverse)this.I1.PropertyBySingularOrPluralName["C3WhereC3I1ManyToOne"];
        this.C1sWhereC1I1ManyToMany = (TramManyToManyInverse)this.I1.PropertyBySingularOrPluralName["C1WhereC1I1ManyToMany"];
        this.C3sWhereC3I1ManyToMany = (TramManyToManyInverse)this.I1.PropertyBySingularOrPluralName["C3WhereC3I1ManyToMany"];
        this.C1WhereC1I12OneToOne = (TramOneToOneInverse)this.I12.PropertyBySingularOrPluralName["C1WhereC1I12OneToOne"];
        this.C3WhereC3I12OneToOne = (TramOneToOneInverse)this.I12.PropertyBySingularOrPluralName["C3WhereC3I12OneToOne"];
        this.C1WhereC1I12OneToMany = (TramOneToManyInverse)this.I12.PropertyBySingularOrPluralName["C1WhereC1I12OneToMany"];
        this.C3WhereC3I12OneToMany = (TramOneToManyInverse)this.I12.PropertyBySingularOrPluralName["C3WhereC3I12OneToMany"];
        this.C1sWhereC1I12ManyToOne = (TramManyToOneInverse)this.I12.PropertyBySingularOrPluralName["C1WhereC1I12ManyToOne"];
        this.C3sWhereC3I12ManyToOne = (TramManyToOneInverse)this.I12.PropertyBySingularOrPluralName["C3WhereC3I12ManyToOne"];
        this.C1sWhereC1I12ManyToMany = (TramManyToManyInverse)this.I12.PropertyBySingularOrPluralName["C1WhereC1I12ManyToMany"];
        this.C3sWhereC3I12ManyToMany = (TramManyToManyInverse)this.I12.PropertyBySingularOrPluralName["C3WhereC3I12ManyToMany"];
        this.C1WhereC1I2OneToOne = (TramOneToOneInverse)this.I2.PropertyBySingularOrPluralName["C1WhereC1I2OneToOne"];
        this.C3WhereC3I2OneToOne = (TramOneToOneInverse)this.I2.PropertyBySingularOrPluralName["C3WhereC3I2OneToOne"];
        this.C1WhereC1I2OneToMany = (TramOneToManyInverse)this.I2.PropertyBySingularOrPluralName["C1WhereC1I2OneToMany"];
        this.C3WhereC3I2OneToMany = (TramOneToManyInverse)this.I2.PropertyBySingularOrPluralName["C3WhereC3I2OneToMany"];
        this.C1sWhereC1I2ManyToOne = (TramManyToOneInverse)this.I2.PropertyBySingularOrPluralName["C1WhereC1I2ManyToOne"];
        this.C3sWhereC3I2ManyToOne = (TramManyToOneInverse)this.I2.PropertyBySingularOrPluralName["C3WhereC3I2ManyToOne"];
        this.C1sWhereC1I2ManyToMany = (TramManyToManyInverse)this.I2.PropertyBySingularOrPluralName["C1WhereC1I2ManyToMany"];
        this.C3sWhereC3I2ManyToMany = (TramManyToManyInverse)this.I2.PropertyBySingularOrPluralName["C3WhereC3I2ManyToMany"];
        this.OrganizationWhereOwner = (TramOneToOneInverse)this.Person.PropertyBySingularOrPluralName["OrganizationWhereOwner"];
        this.OrganizationsWhereAccountant = (TramManyToOneInverse)this.Person.PropertyBySingularOrPluralName["OrganizationWhereAccountant"];
        this.OrganizationWhereEmployee = (TramOneToManyInverse)this.Person.PropertyBySingularOrPluralName["OrganizationWhereEmployee"];
        this.OrganizationsWhereCustomer = (TramManyToManyInverse)this.Person.PropertyBySingularOrPluralName["OrganizationWhereCustomer"];
        this.C1WhereC1S1OneToOne = (TramOneToOneInverse)this.S1.PropertyBySingularOrPluralName["C1WhereC1S1OneToOne"];
        this.C3WhereC3S1OneToOne = (TramOneToOneInverse)this.S1.PropertyBySingularOrPluralName["C3WhereC3S1OneToOne"];
        this.C1WhereC1S1OneToMany = (TramOneToManyInverse)this.S1.PropertyBySingularOrPluralName["C1WhereC1S1OneToMany"];
        this.C3WhereC3S1OneToMany = (TramOneToManyInverse)this.S1.PropertyBySingularOrPluralName["C3WhereC3S1OneToMany"];
        this.C1sWhereC1S1ManyToOne = (TramManyToOneInverse)this.S1.PropertyBySingularOrPluralName["C1WhereC1S1ManyToOne"];
        this.C3sWhereC3S1ManyToOne = (TramManyToOneInverse)this.S1.PropertyBySingularOrPluralName["C3WhereC3S1ManyToOne"];
        this.C1sWhereC1S1ManyToMany = (TramManyToManyInverse)this.S1.PropertyBySingularOrPluralName["C1WhereC1S1ManyToMany"];
        this.C3sWhereC3S1ManyToMany = (TramManyToManyInverse)this.S1.PropertyBySingularOrPluralName["C3WhereC3S1ManyToMany"];
        this.C1WhereC1S2OneToOne = (TramOneToOneInverse)this.S2.PropertyBySingularOrPluralName["C1WhereC1S2OneToOne"];
        this.C3WhereC3S2OneToOne = (TramOneToOneInverse)this.S2.PropertyBySingularOrPluralName["C3WhereC3S2OneToOne"];
        this.C1WhereC1S2OneToMany = (TramOneToManyInverse)this.S2.PropertyBySingularOrPluralName["C1WhereC1S2OneToMany"];
        this.C3WhereC3S2OneToMany = (TramOneToManyInverse)this.S2.PropertyBySingularOrPluralName["C3WhereC3S2OneToMany"];
        this.C1sWhereC1S2ManyToOne = (TramManyToOneInverse)this.S2.PropertyBySingularOrPluralName["C1WhereC1S2ManyToOne"];
        this.C3sWhereC3S2ManyToOne = (TramManyToOneInverse)this.S2.PropertyBySingularOrPluralName["C3WhereC3S2ManyToOne"];
        this.C1sWhereC1S2ManyToMany = (TramManyToManyInverse)this.S2.PropertyBySingularOrPluralName["C1WhereC1S2ManyToMany"];
        this.C3sWhereC3S2ManyToMany = (TramManyToManyInverse)this.S2.PropertyBySingularOrPluralName["C3WhereC3S2ManyToMany"];
    }

    // Types
    public TramValueType Boolean { get; }
    public TramClass C1 { get; }
    public TramClass C2 { get; }
    public TramClass C3 { get; }
    public TramClass C4 { get; }
    public TramValueType DateTime { get; }
    public TramInterface I1 { get; }
    public TramInterface I12 { get; }
    public TramInterface I2 { get; }
    public TramValueType Integer { get; }
    public TramClass Organization { get; }
    public TramClass Person { get; }
    public TramInterface S1 { get; }
    public TramInterface S2 { get; }
    public TramValueType String { get; }

    // Attributes
    public TramAttribute C1Same { get; }
    public TramAttribute C1C1AllorsString { get; }
    public TramAttribute C1SameAllorsString { get; }
    public TramAttribute C1C1AllorsInteger { get; }
    public TramAttribute C1SameAllorsInteger { get; }
    public TramAttribute C1C1AllorsBoolean { get; }
    public TramAttribute C1SameAllorsBoolean { get; }
    public TramAttribute C2Same { get; }
    public TramAttribute C2C2AllorsString { get; }
    public TramAttribute C2SameAllorsString { get; }
    public TramAttribute C2C2AllorsInteger { get; }
    public TramAttribute C2SameAllorsInteger { get; }
    public TramAttribute C2C2AllorsBoolean { get; }
    public TramAttribute C2SameAllorsBoolean { get; }
    public TramAttribute C3C3AllorsString { get; }
    public TramAttribute C3SameAllorsString { get; }
    public TramAttribute C3C3AllorsInteger { get; }
    public TramAttribute C3SameAllorsInteger { get; }
    public TramAttribute C3C3AllorsBoolean { get; }
    public TramAttribute C3SameAllorsBoolean { get; }
    public TramAttribute C4C4AllorsString { get; }
    public TramAttribute C4SameAllorsString { get; }
    public TramAttribute C4C4AllorsInteger { get; }
    public TramAttribute C4SameAllorsInteger { get; }
    public TramAttribute C4C4AllorsBoolean { get; }
    public TramAttribute C4SameAllorsBoolean { get; }
    public TramAttribute I1I1AllorsString { get; }
    public TramAttribute I1I1AllorsInteger { get; }
    public TramAttribute I1I1AllorsBoolean { get; }
    public TramAttribute I12I12AllorsString { get; }
    public TramAttribute I12I12AllorsInteger { get; }
    public TramAttribute I12I12AllorsBoolean { get; }
    public TramAttribute I2I2AllorsString { get; }
    public TramAttribute I2I2AllorsInteger { get; }
    public TramAttribute I2I2AllorsBoolean { get; }
    public TramAttribute OrganizationName { get; }
    public TramAttribute PersonFirstName { get; }
    public TramAttribute PersonLastName { get; }
    public TramAttribute PersonFullName { get; }
    public TramAttribute PersonDerivedAt { get; }
    public TramAttribute PersonGreeting { get; }
    public TramAttribute S1S1AllorsString { get; }
    public TramAttribute S1S1AllorsInteger { get; }
    public TramAttribute S1S1AllorsBoolean { get; }
    public TramAttribute S2S2AllorsString { get; }
    public TramAttribute S2S2AllorsInteger { get; }
    public TramAttribute S2S2AllorsBoolean { get; }

    // RelationEnds
    public TramOneToOneRole C1C1C1OneToOne { get; }
    public TramOneToOneRole C1C1C2OneToOne { get; }
    public TramOneToOneRole C1C1C3OneToOne { get; }
    public TramOneToOneRole C1C1C4OneToOne { get; }
    public TramOneToOneRole C1C1I1OneToOne { get; }
    public TramOneToOneRole C1C1I2OneToOne { get; }
    public TramOneToOneRole C1C1I12OneToOne { get; }
    public TramOneToOneRole C1C1S1OneToOne { get; }
    public TramOneToOneRole C1C1S2OneToOne { get; }
    public TramOneToManyRole C1C1C1OneToManies { get; }
    public TramOneToManyRole C1C1C2OneToManies { get; }
    public TramOneToManyRole C1C1C3OneToManies { get; }
    public TramOneToManyRole C1C1C4OneToManies { get; }
    public TramOneToManyRole C1C1I1OneToManies { get; }
    public TramOneToManyRole C1C1I2OneToManies { get; }
    public TramOneToManyRole C1C1I12OneToManies { get; }
    public TramOneToManyRole C1C1S1OneToManies { get; }
    public TramOneToManyRole C1C1S2OneToManies { get; }
    public TramManyToOneRole C1C1C1ManyToOne { get; }
    public TramManyToOneRole C1C1C2ManyToOne { get; }
    public TramManyToOneRole C1C1C3ManyToOne { get; }
    public TramManyToOneRole C1C1C4ManyToOne { get; }
    public TramManyToOneRole C1C1I1ManyToOne { get; }
    public TramManyToOneRole C1C1I2ManyToOne { get; }
    public TramManyToOneRole C1C1I12ManyToOne { get; }
    public TramManyToOneRole C1C1S1ManyToOne { get; }
    public TramManyToOneRole C1C1S2ManyToOne { get; }
    public TramManyToManyRole C1C1C1ManyToManies { get; }
    public TramManyToManyRole C1C1C2ManyToManies { get; }
    public TramManyToManyRole C1C1C3ManyToManies { get; }
    public TramManyToManyRole C1C1C4ManyToManies { get; }
    public TramManyToManyRole C1C1I1ManyToManies { get; }
    public TramManyToManyRole C1C1I2ManyToManies { get; }
    public TramManyToManyRole C1C1I12ManyToManies { get; }
    public TramManyToManyRole C1C1S1ManyToManies { get; }
    public TramManyToManyRole C1C1S2ManyToManies { get; }
    public TramOneToOneRole C3C3C1OneToOne { get; }
    public TramOneToOneRole C3C3C2OneToOne { get; }
    public TramOneToOneRole C3C3C3OneToOne { get; }
    public TramOneToOneRole C3C3C4OneToOne { get; }
    public TramOneToOneRole C3C3I1OneToOne { get; }
    public TramOneToOneRole C3C3I2OneToOne { get; }
    public TramOneToOneRole C3C3I12OneToOne { get; }
    public TramOneToOneRole C3C3S1OneToOne { get; }
    public TramOneToOneRole C3C3S2OneToOne { get; }
    public TramOneToManyRole C3C3C1OneToManies { get; }
    public TramOneToManyRole C3C3C2OneToManies { get; }
    public TramOneToManyRole C3C3C3OneToManies { get; }
    public TramOneToManyRole C3C3C4OneToManies { get; }
    public TramOneToManyRole C3C3I1OneToManies { get; }
    public TramOneToManyRole C3C3I2OneToManies { get; }
    public TramOneToManyRole C3C3I12OneToManies { get; }
    public TramOneToManyRole C3C3S1OneToManies { get; }
    public TramOneToManyRole C3C3S2OneToManies { get; }
    public TramManyToOneRole C3C3C1ManyToOne { get; }
    public TramManyToOneRole C3C3C2ManyToOne { get; }
    public TramManyToOneRole C3C3C3ManyToOne { get; }
    public TramManyToOneRole C3C3C4ManyToOne { get; }
    public TramManyToOneRole C3C3I1ManyToOne { get; }
    public TramManyToOneRole C3C3I2ManyToOne { get; }
    public TramManyToOneRole C3C3I12ManyToOne { get; }
    public TramManyToOneRole C3C3S1ManyToOne { get; }
    public TramManyToOneRole C3C3S2ManyToOne { get; }
    public TramManyToManyRole C3C3C1ManyToManies { get; }
    public TramManyToManyRole C3C3C2ManyToManies { get; }
    public TramManyToManyRole C3C3C3ManyToManies { get; }
    public TramManyToManyRole C3C3C4ManyToManies { get; }
    public TramManyToManyRole C3C3I1ManyToManies { get; }
    public TramManyToManyRole C3C3I2ManyToManies { get; }
    public TramManyToManyRole C3C3I12ManyToManies { get; }
    public TramManyToManyRole C3C3S1ManyToManies { get; }
    public TramManyToManyRole C3C3S2ManyToManies { get; }
    public TramOneToOneRole OrganizationOwner { get; }
    public TramManyToOneRole OrganizationAccountant { get; }
    public TramOneToManyRole OrganizationEmployees { get; }
    public TramManyToManyRole OrganizationCustomers { get; }

    // Inverses
    public TramOneToOneInverse C1WhereC1C1OneToOne { get; }
    public TramOneToOneInverse C3WhereC3C1OneToOne { get; }
    public TramOneToManyInverse C1WhereC1C1OneToMany { get; }
    public TramOneToManyInverse C3WhereC3C1OneToMany { get; }
    public TramManyToOneInverse C1sWhereC1C1ManyToOne { get; }
    public TramManyToOneInverse C3sWhereC3C1ManyToOne { get; }
    public TramManyToManyInverse C1sWhereC1C1ManyToMany { get; }
    public TramManyToManyInverse C3sWhereC3C1ManyToMany { get; }
    public TramOneToOneInverse C1WhereC1C2OneToOne { get; }
    public TramOneToOneInverse C3WhereC3C2OneToOne { get; }
    public TramOneToManyInverse C1WhereC1C2OneToMany { get; }
    public TramOneToManyInverse C3WhereC3C2OneToMany { get; }
    public TramManyToOneInverse C1sWhereC1C2ManyToOne { get; }
    public TramManyToOneInverse C3sWhereC3C2ManyToOne { get; }
    public TramManyToManyInverse C1sWhereC1C2ManyToMany { get; }
    public TramManyToManyInverse C3sWhereC3C2ManyToMany { get; }
    public TramOneToOneInverse C1WhereC1C3OneToOne { get; }
    public TramOneToOneInverse C3WhereC3C3OneToOne { get; }
    public TramOneToManyInverse C1WhereC1C3OneToMany { get; }
    public TramOneToManyInverse C3WhereC3C3OneToMany { get; }
    public TramManyToOneInverse C1sWhereC1C3ManyToOne { get; }
    public TramManyToOneInverse C3sWhereC3C3ManyToOne { get; }
    public TramManyToManyInverse C1sWhereC1C3ManyToMany { get; }
    public TramManyToManyInverse C3sWhereC3C3ManyToMany { get; }
    public TramOneToOneInverse C1WhereC1C4OneToOne { get; }
    public TramOneToOneInverse C3WhereC3C4OneToOne { get; }
    public TramOneToManyInverse C1WhereC1C4OneToMany { get; }
    public TramOneToManyInverse C3WhereC3C4OneToMany { get; }
    public TramManyToOneInverse C1sWhereC1C4ManyToOne { get; }
    public TramManyToOneInverse C3sWhereC3C4ManyToOne { get; }
    public TramManyToManyInverse C1sWhereC1C4ManyToMany { get; }
    public TramManyToManyInverse C3sWhereC3C4ManyToMany { get; }
    public TramOneToOneInverse C1WhereC1I1OneToOne { get; }
    public TramOneToOneInverse C3WhereC3I1OneToOne { get; }
    public TramOneToManyInverse C1WhereC1I1OneToMany { get; }
    public TramOneToManyInverse C3WhereC3I1OneToMany { get; }
    public TramManyToOneInverse C1sWhereC1I1ManyToOne { get; }
    public TramManyToOneInverse C3sWhereC3I1ManyToOne { get; }
    public TramManyToManyInverse C1sWhereC1I1ManyToMany { get; }
    public TramManyToManyInverse C3sWhereC3I1ManyToMany { get; }
    public TramOneToOneInverse C1WhereC1I12OneToOne { get; }
    public TramOneToOneInverse C3WhereC3I12OneToOne { get; }
    public TramOneToManyInverse C1WhereC1I12OneToMany { get; }
    public TramOneToManyInverse C3WhereC3I12OneToMany { get; }
    public TramManyToOneInverse C1sWhereC1I12ManyToOne { get; }
    public TramManyToOneInverse C3sWhereC3I12ManyToOne { get; }
    public TramManyToManyInverse C1sWhereC1I12ManyToMany { get; }
    public TramManyToManyInverse C3sWhereC3I12ManyToMany { get; }
    public TramOneToOneInverse C1WhereC1I2OneToOne { get; }
    public TramOneToOneInverse C3WhereC3I2OneToOne { get; }
    public TramOneToManyInverse C1WhereC1I2OneToMany { get; }
    public TramOneToManyInverse C3WhereC3I2OneToMany { get; }
    public TramManyToOneInverse C1sWhereC1I2ManyToOne { get; }
    public TramManyToOneInverse C3sWhereC3I2ManyToOne { get; }
    public TramManyToManyInverse C1sWhereC1I2ManyToMany { get; }
    public TramManyToManyInverse C3sWhereC3I2ManyToMany { get; }
    public TramOneToOneInverse OrganizationWhereOwner { get; }
    public TramManyToOneInverse OrganizationsWhereAccountant { get; }
    public TramOneToManyInverse OrganizationWhereEmployee { get; }
    public TramManyToManyInverse OrganizationsWhereCustomer { get; }
    public TramOneToOneInverse C1WhereC1S1OneToOne { get; }
    public TramOneToOneInverse C3WhereC3S1OneToOne { get; }
    public TramOneToManyInverse C1WhereC1S1OneToMany { get; }
    public TramOneToManyInverse C3WhereC3S1OneToMany { get; }
    public TramManyToOneInverse C1sWhereC1S1ManyToOne { get; }
    public TramManyToOneInverse C3sWhereC3S1ManyToOne { get; }
    public TramManyToManyInverse C1sWhereC1S1ManyToMany { get; }
    public TramManyToManyInverse C3sWhereC3S1ManyToMany { get; }
    public TramOneToOneInverse C1WhereC1S2OneToOne { get; }
    public TramOneToOneInverse C3WhereC3S2OneToOne { get; }
    public TramOneToManyInverse C1WhereC1S2OneToMany { get; }
    public TramOneToManyInverse C3WhereC3S2OneToMany { get; }
    public TramManyToOneInverse C1sWhereC1S2ManyToOne { get; }
    public TramManyToOneInverse C3sWhereC3S2ManyToOne { get; }
    public TramManyToManyInverse C1sWhereC1S2ManyToMany { get; }
    public TramManyToManyInverse C3sWhereC3S2ManyToMany { get; }
}
