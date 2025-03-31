using Moq;
using NUnit.Framework;

namespace Validata.OrderManagementSystem.Tests;

public abstract class BaseTest
{
    protected MockRepository MockRepository;

    [SetUp]
    public void Setup()
    {
        MockRepository = new MockRepository(MockBehavior.Strict);
    }

    [TearDown]
    public void TearDown()
    {
        MockRepository.VerifyAll();
    }
}
