using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Core.Users.Common.Models;
using WebApplication.Core.Users.Queries;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserAsync(
            [FromQuery] GetUserQuery query,
            CancellationToken cancellationToken)
        {
            UserDto result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        // TODO: create a route (at /Find) that can retrieve a list of matching users using the `FindUsersQuery`
        [HttpGet("find")]
        public async Task<IActionResult> FindUsers()
        {
            var result = await _mediator.Send(new FindUsersQuery());
            return result.ToActionResult();
        }

        // TODO: create a route (at /List) that can retrieve a paginated list of users using the `ListUsersQuery`
        [HttpGet("list")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _mediator.Send(new ListUsersQuery
            {
               
            });
            return result.ToActionResult();
        }

        // TODO: create a route that can create a user using the `CreateUserCommand`
        [HttpPost("createnewuser")]
        public async Task<IActionResult> CreateNewUser()
        {
            var commandCreateUser = new CreateUserCommand()
            {
          
            };

            var resultCreateUser = _mediator.Send(commandCreateUser);

            resultCreateUser.Result.ToActionResult();    
        }
        // TODO: create a route that can update an existing user using the `UpdateUserCommand`

        // TODO: create a route that can delete an existing user using the `DeleteUserCommand`
    }
}
