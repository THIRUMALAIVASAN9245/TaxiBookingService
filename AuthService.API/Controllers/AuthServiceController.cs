namespace AuthService.API.Controllers
{
    using System;
    using AuthService.API.Models;
    using AuthService.API.Services;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;

    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    public class AuthServiceController : Controller
    {
        private readonly IUserService userService;

        private readonly ITokenGenerator tokenGenerator;

        public AuthServiceController(IUserService userService, ITokenGenerator tokenGenerator)
        {
            this.userService = userService;
            this.tokenGenerator = tokenGenerator;
        }

        /// <summary>
        /// Add user info.
        /// </summary>
        /// <param name="user">New user info</param>
        /// <returns>No of records Inserted</returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Models.UserModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public IActionResult Post([FromBody] Models.UserModel userModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (userService.IsUserExist(userModel))
                {
                    return StatusCode(409, $"UserId {userModel.Id} already exist in database"); ;
                }

                var result = userService.Add(userModel);

                return Created("user created", result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error Occurred While Save userinfo" + ex.Message);
            }
        }

        ///// <summary>
        ///// Get the JWT token.
        ///// </summary>
        ///// <param name="user">user object</param>
        ///// <returns>JWT token for login user</returns>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(200, Type = typeof(UserModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult Login([FromBody] UserModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var isUser = userService.GetUser(user.Email, user.Password);
                if (isUser == null)
                {
                    return NotFound($"Email Id {user.Email} not found");
                }

                string value = tokenGenerator.GetJwtTokenLoggedinUser(user);

                return Ok(value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}