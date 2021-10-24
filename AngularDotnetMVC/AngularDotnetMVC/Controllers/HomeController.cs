using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.User.Commands;
using Application.User.Queries;
using System.Dynamic;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AngularDotnetMVC.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public UserController(ILogger<UserController> logger, IMediator mediator)
    {
        _logger = logger;
        this._mediator = mediator;
    }

    [HttpGet]
    public async Task<IEnumerable<GetAllUsersQuery.Response>> GetUserAsync()
    {
        var allUsers = await this._mediator.Send(new GetAllUsersQuery { });

        return allUsers;
    }

    [HttpPost]
    public async Task<CreateUserCommand> AddUserAsync(CreateUserCommand command)
    {
        try
        {

            command.Id = Guid.NewGuid();

            await this._mediator.Send(command);

            return command;
        }
        catch (Exception ex)
        {
            throw;
        }

    }
}

