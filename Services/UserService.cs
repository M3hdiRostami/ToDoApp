using Couchbase.KeyValue;
using Couchbase.Query;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoAPI.Models;
using ToDoAPI.Uilities.config;
using ToDoAPI.Uilities.Responses;

namespace ToDoAPI.Services
{

    public class UserService : IUserService
    {
        protected readonly ICouchbaseService _couchbaseService;
        protected readonly ILogger<CouchDatabaseInintService> _logger;
        protected readonly CouchbaseConfig _couchbaseConfig;
        protected ICouchbaseCollection userCollection
        {
            get => _couchbaseService.GetScopeCollection(_couchbaseConfig.CollectionName).Result;
        }

        public UserService(ICouchbaseService couchbaseService, IOptions<CouchbaseConfig> options, ILogger<CouchDatabaseInintService> logger)
        {
            _couchbaseService = couchbaseService;
            _logger = logger;
            _couchbaseConfig = options.Value;

        }

        public async Task<bool> UserExists(string username)
        {
            try
            {
                var result = await userCollection.ExistsAsync(
                $"user::{username}",
                new Couchbase.KeyValue.ExistsOptions());
                return result.Exists;
            }
            catch
            {
                return false;
            }
        }

        public async Task<User> CreateUser(ProfileCreateRequestCommand profileCreateRequestCommand)
        {
            User user = profileCreateRequestCommand.GetUser();
            try
            {

                await userCollection.InsertAsync($"user::{user.Username}", user, new Couchbase.KeyValue.InsertOptions());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }

            return user;
        }

        public async Task<User> GetUser(string Username)
        {
            try
            {
                var result = await userCollection.GetAsync($"user::{Username}", new Couchbase.KeyValue.GetOptions());
                return result.ContentAs<User>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task UpdateUser(User user)
        {

            await userCollection.ReplaceAsync(
                 $"user::{user.Username}",
                 user,
                 new Couchbase.KeyValue.ReplaceOptions());

        }

        #region ITodoOperatable

        public async Task<BaseResponse<ICollection<ToDoTask>>> GetUserToDoList(DateTime dueTimefrom, DateTime dueTimeto)
        {
            var cluster = await _couchbaseService.GetClusterAsync();
            string query = $@"SELECT toDos.*
                            FROM {_couchbaseConfig.BucketName}.
                                 {_couchbaseConfig.ScopeName}.
                                {_couchbaseConfig.CollectionName} p
                                UNNEST toDos
                            WHERE toDos.dueTime BETWEEN $from AND $to";

            var queryOptions = new QueryOptions()
                .Parameter("from", dueTimefrom.ToUniversalTime().ToString("o"))
                .Parameter("to", dueTimeto.ToUniversalTime().ToString("o"));

            var queryResult = await cluster.QueryAsync<ToDoTask>(query, queryOptions);

            // check query was successful
            if (queryResult.MetaData.Status != QueryStatus.Success)
            {
                _logger.LogError($"error in {nameof(GetUserToDoList)} query");
                return new BaseResponse<ICollection<ToDoTask>>("Error in executing query!"); ;
            }

            IAsyncEnumerable<ToDoTask> pocoRows = queryResult.Rows;
            var list = await pocoRows.ToListAsync();
            if (list.Count() == 0)
            {
                return new BaseResponse<ICollection<ToDoTask>>("Tasks not Found,try to change dates filter");
            }
            return new BaseResponse<ICollection<ToDoTask>>(list);
        }
        public async Task<BaseResponse<ToDoTask>> UpdateToDo(string username, ToDoTask task)
        {
            BaseResponse<ToDoTask> deleteResponse = await DeleteToDo(username, task.ID);
            if (deleteResponse.Success)
            {
                await AddToDo(username, new List<ToDoTask> { task });
            }
            return new BaseResponse<ToDoTask>(task);
        }

        public async Task<BaseResponse<ToDoTask>> DeleteToDo(string username, Guid taskId)
        {
            var user = await GetUser(username);

            ToDoTask targetTask = user.ToDos.FirstOrDefault(t => t.ID == taskId);
            if (targetTask is default(ToDoTask))
            {
                return new BaseResponse<ToDoTask>("Task not Found!");
            }
            user.ToDos.Remove(targetTask);
            await UpdateUser(user);
            return new BaseResponse<ToDoTask>(targetTask);
        }

        public async Task<BaseResponse<ICollection<ToDoTask>>> AddToDo(string username, ICollection<ToDoTask> tasks)
        {
            var user = await GetUser(username);
            foreach (var task in tasks)
            {
                task.ID = Guid.NewGuid();
            }
            user.ToDos.AddRange(tasks);

            await UpdateUser(user);
            return new BaseResponse<ICollection<ToDoTask>>(tasks);
        }

        public async Task<BaseResponse<ICollection<ToDoTask>>> GetUserToDoList(string username, Guid taskId = default)
        {
            var user = await GetUser(username);
            List<ToDoTask> tasks = new List<ToDoTask>();
            if (taskId == default)
            {
                tasks = user.ToDos;
            }
            else
            {
                tasks.Add(user.ToDos.FirstOrDefault(t => t.ID == taskId));
            }
            return new BaseResponse<ICollection<ToDoTask>>(tasks);
        }

        #endregion ITodoOperatable

    }

}
