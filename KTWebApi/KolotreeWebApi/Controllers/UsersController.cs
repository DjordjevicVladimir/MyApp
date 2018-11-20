using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KolotreeWebApi.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;

namespace KolotreeWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly UserService userService;
       

        public UsersController(UserService _userService)
        {
     
            userService = _userService;
        }
        // GET: api/Users
        [HttpGet(Name = "GetUsers")]       
        public async Task<IActionResult> GetAllUsers()
        {
            var resultList =  await userService.FetchAllUsers();            
            return Ok(resultList);            
        }

        // GET: api/Users/5
        [HttpGet("{id}", Name = "GetUser")]    
        public async Task<IActionResult> GetUser(int id)
        {
           User user =await userService.FindUser(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        
        // POST: api/Users
        [HttpPost]        
        public async Task<IActionResult> CreateUser([FromBody]UserForManipulation user)
        {
            if (!ModelState.IsValid)
            {
                return  BadRequest(ModelState);          
            }
            try
            {
                User newUser = await userService.AddUser(user);
                return Created($"GetUser/{newUser.UserId}",newUser);
            }
            catch (Exception xcp)
            {
                return StatusCode(500, xcp.Message);
            }

        }
        
        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody]UserForManipulation user)
        {
            User oldUser = await userService.FindUser(id);
            if (oldUser == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                userService.UpdateUser(user, oldUser);
                return NoContent();
            }
            catch (Exception xcp)
            {
                return StatusCode(500, xcp.Message);
            }          
            
        }
      
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            User user = await userService.FindUser(id);
            if (user == null)
            {
                return NotFound();
            }
            try
            {
                await userService.DeleteUser(user);
                return NoContent();
            }
            catch (Exception xcp)
            {
                return StatusCode(500, xcp.Message);
            }
        }
    }
}
