using System;
using System.Linq;
using System.Threading.Tasks;
using Application.User.Commands;
using Application.User.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace AngularDotnetMVC.Tests;

public class UserTests : BaseTests
{
    private IMediator _mediator;

    public UserTests() : base(Microsoft.Extensions.Hosting.Environments.Development) { }

    [SetUp]
    public void Setup()
    {
        var serviceProvider = this.application.GetService<IServiceProvider>();
        var serviceScope = serviceProvider.CreateScope();

        _mediator = serviceScope.ServiceProvider.GetRequiredService<IMediator>();
    }

    [Test]
    [TestCase("Hieu", "Le")]
    public async Task ShouldCreateUserSuccess(string firstName, string lastName)
    {
        try
        {

            var command = new CreateUserCommand
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName
            };


            var result = await this._mediator.Send(command);
            Assert.AreEqual(Unit.Value, result);

            var allUsers = await this._mediator.Send(new GetAllUsersQuery { });
            Assert.IsTrue(allUsers.Any(p => p.Id == command.Id), "Should have the new user in the database");

        }
        catch (Exception ex)
        {
            throw;
        }
    }


    [Test]
    [TestCase("", "Le")]
    [TestCase("Hieu", "")]
    public async Task ShouldCreateUserFailed(string firstName, string lastName)
    {
        try
        {

            var command = new CreateUserCommand
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName
            };

            Assert.CatchAsync<FluentValidation.ValidationException>(async () =>
            {
                await this._mediator.Send(command);
            });

        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
