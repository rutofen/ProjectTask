using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Interface;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using services;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
namespace Controllers
{ 
    using Models;
    
[ApiController]
[Route("[controller]")]
public class UserController:ControllerBase
{
    private readonly int userId;
    InterfaceUser userService;
    public UserController(InterfaceUser userService,IHttpContextAccessor httpContextAccessor)
    {
        this.userService = userService;
        this.userId = int.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value);
    }
        [HttpGet]
        [Route("GetAll")]
        [Authorize(Policy = "Manager")]
        public ActionResult<List<User>> GetAll() => userService.GetAll();
       
        [HttpGet]
        [Authorize(Policy = "User")]
        [Route("GetMyUser")]
        public ActionResult<User> GetMyUser()
        {
            var user = userService.Get(userId);
              if (user == null)
                return NotFound();
            return user;
        }
        
        [HttpPost]
        [Authorize(Policy = "Manager")]
        public ActionResult Post([FromBody] User user)
        {
            userService.Post(user);
            return CreatedAtAction(nameof(Post), new { Id = user.UserId }, user);

        }

        [HttpDelete]
        [Authorize(Policy = "Manager")]
        public ActionResult Delete(int id)
        {
            var user = userService.Get(id);
            if (user == null){
                return NotFound();
            }
            userService.Delete(id);
            return NoContent();
        }


}

}