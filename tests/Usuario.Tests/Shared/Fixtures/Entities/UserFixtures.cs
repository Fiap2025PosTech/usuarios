using Bogus;
using Usuario.Tests.Shared.Fixtures.Utils;
using Usuarios.Domain.Entities;
using Usuarios.Domain.Enums;

namespace Usuario.Tests.Shared.Fixtures.Entities;

public class UserFixtures : BaseFixtures<User>
{
    public static User GenerateUser()
    {
        var faker = new Faker<User>("pt_BR")
            .RuleFor(u => u.Name, f => f.Person.FullName)
            .RuleFor(u => u.Email, f => f.Person.Email)
            .RuleFor(u => u.Password, f => f.Internet.Password(9))
            .RuleFor(u => u.AccessLevel, f => f.PickRandom<AccessLevel>());

        return faker.Generate();
    }

    public static User CreateAs_Base()
    {
        var user = GenerateUser();
        user.PrepareToInsert(Guid.NewGuid());

        return user;
    }
}