using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence;
using Persistence.File;
using System.Linq;

using static Application.User.Queries.GetAllUsersQuery;

namespace Application.User.Queries;

public class GetAllUsersQuery : IRequest<IEnumerable<Response>>
{

    public class Handler : IRequestHandler<GetAllUsersQuery, IEnumerable<Response>>
    {
        private readonly ILogger _logger;
        private readonly IRepository<Domain.User> _userRepository;

        public Handler(ILogger<GetAllUsersQuery> logger,
            IRepository<Domain.User> userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<Response>> Handle(GetAllUsersQuery querry, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _userRepository.GetAllAsync();

                return users.Select(p => new Response
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName
                })
                .AsEnumerable();
            }
            catch (Exception e)
            {
                //_logger.LogError(e, "Failed when execute command with {Payload}",
                //    JsonConvert.SerializeObject(new { command }));
                throw;
            }
        }
    }

    public record Response
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}

