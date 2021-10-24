using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence;
using Persistence.File;

namespace Application.User.Commands;

public class CreateUserCommand : IRequest
{
    public Guid? Id { get; set; } = Guid.NewGuid();

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public class Validator : AbstractValidator<CreateUserCommand>
    {
        public Validator()
        {
            RuleFor(p => p.Id).NotEqual(Guid.Empty);

            RuleFor(p => p.FirstName).NotEmpty();
            RuleFor(p => p.LastName).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<CreateUserCommand>
    {
        private readonly ILogger _logger;
        private readonly IRepository<Domain.User> _userRepository;

        public Handler(ILogger<CreateUserCommand> logger,
            IRepository<Domain.User> userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            try
            {
                await _userRepository.AddAsync(new Domain.User
                {
                    Id = command.Id ?? Guid.NewGuid(),
                    FirstName = command.FirstName,
                    LastName = command.LastName,
                    CreatedAt = DateTime.UtcNow,
                });

                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed when execute command with {Payload}",
                    JsonSerializer.Serialize(new { command }));
                throw;
            }
        }
    }
}

