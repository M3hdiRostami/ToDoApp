using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoAPI.Models;
using ToDoAPI.Uilities.Responses;

namespace ToDoAPI.Services
{
    public interface ITodoOperatable
    {
        Task<BaseResponse<ToDoTask>> UpdateToDo(string username, ToDoTask task);
        Task<BaseResponse<ToDoTask>> DeleteToDo(string username, Guid taskId);
        Task<BaseResponse<ICollection<ToDoTask>>> AddToDo(string username, ICollection<ToDoTask> tasks);
        Task<BaseResponse<ICollection<ToDoTask>>> GetUserToDoList(DateTime dueTimefrom, DateTime dueTimeto);
        Task<BaseResponse<ICollection<ToDoTask>>> GetUserToDoList(string username, Guid taskId = default);
    }

}
