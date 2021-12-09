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

        /// <summary>
        /// Get user ToDoList by ID ,or leave it blank to get all of theme
        /// </summary>
        /// <param name="taskId">Guid</param>
        /// <returns></returns>
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
        /// <summary>
        /// Get user ToDoList within specified dates
        /// </summary>
        /// <param name="dueTimefrom">DateTime Sample 2021-12-09T13:14:39.513Z</param>
        /// <param name="dueTimeto">Date Sample 2021-12-09</param>
        /// <returns></returns>
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
        /// <summary>
        /// Update user ToDoList
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Delete user ToDoList by ID
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Add new ToDo items 
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
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
