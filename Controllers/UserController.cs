using API.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoAPI.Models;
using ToDoAPI.Services;
using ToDoAPI.Uilities.Responses;

namespace ToDoAPI.Controllers
{
    public class UserController : AuthAPIControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
           
        }


        [HttpGet]
        public async Task<ActionResult> GetUserToDoList(Guid taskId = default)
        {
            var response = await _userService.GetUserToDoList(CurrentUser.Username,taskId);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }
        [HttpGet("{dueTimefrom}/{dueTimeto}")]
        public async Task<ActionResult> GetUserToDoList(DateTime dueTimefrom, DateTime dueTimeto)
        {

            var response = await _userService.GetUserToDoList(dueTimefrom, dueTimeto);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }
        [HttpPost]
        public async Task<ActionResult> UpdateToDo(ToDoTask task)
        {

            var response = await _userService.UpdateToDo(CurrentUser.Username, task);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }
        [HttpDelete("{taskId}")]
        public async Task<ActionResult> DeleteToDo(Guid taskId)
        {

            var response = await _userService.DeleteToDo(CurrentUser.Username, taskId);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }
        [HttpPut]
        public async Task<ActionResult> AddToDo(ICollection<ToDoTask> tasks)
        {

            var response = await _userService.AddToDo(CurrentUser.Username, tasks);
            if (response.Success)
            {
                return Created(nameof(GetUserToDoList), response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}
