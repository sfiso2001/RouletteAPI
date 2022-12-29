using Bogus;

namespace Roulette.BusinessLogicTests
{
    public abstract class BaseTest
    {
        public readonly Faker _faker;

        public const int FAKER_STRING2_LENGTH = 20;

        public BaseTest()
        {
            _faker = new Faker("en");
        }
    }
}