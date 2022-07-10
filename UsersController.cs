using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebApplication.Core.Users.Commands;
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
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> FindUsers(
            [FromQuery] FindUsersQuery query,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        // TODO: create a route (at /List) that can retrieve a paginated list of users using the `ListUsersQuery`
        [HttpGet("list")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllUsers(
            [FromQuery] ListUsersQuery query,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        // TODO: create a route that can create a user using the `CreateUserCommand`
        [HttpPost("createnewuser")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateNewUser(UserDto userDto, CancellationToken cancellationToken)
        {
            var commandCreateUser = new CreateUserCommand()
            {
                GivenNames = userDto.GivenNames,
                LastName = userDto.LastName,
                EmailAddress = !string.IsNullOrEmpty(userDto.EmailAddress) ? userDto.EmailAddress : string.Empty,
                MobileNumber = !string.IsNullOrEmpty(userDto.MobileNumber) ? userDto.MobileNumber : string.Empty
            };

            var resultCreateUser = await _mediator.Send(commandCreateUser);

            return Ok(resultCreateUser);
        }

        // TODO: create a route that can update an existing user using the `UpdateUserCommand`
        [HttpPost("updateuser")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUser(UserDto userDto, CancellationToken cancellationToken)
        {
            var commandUpdateUser = new UpdateUserCommand()
            {
                Id = userDto.UserId,
                GivenNames = userDto.GivenNames,
                LastName = userDto.LastName,
                EmailAddress = !string.IsNullOrEmpty(userDto.EmailAddress) ? userDto.EmailAddress : string.Empty,
                MobileNumber = !string.IsNullOrEmpty(userDto.MobileNumber) ? userDto.MobileNumber : string.Empty
            };

            var resultUpdateUser = await _mediator.Send(commandUpdateUser);

            return Ok(resultUpdateUser);
        }

        // TODO: create a route that can delete an existing user using the `DeleteUserCommand`
        [HttpPost("deleteuser")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteUser(UserDto userDto, CancellationToken cancellationToken)
        {
            var commandDeleteUser = new DeleteUserCommand()
            {
                Id = userDto.UserId
            };

            var resultDeleteUser = await _mediator.Send(commandDeleteUser);

            return Ok(resultDeleteUser);
        }
    }
}
