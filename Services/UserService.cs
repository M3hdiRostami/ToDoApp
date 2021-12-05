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

namespace ToDoAPI.Services
{
    public interface IUserService
    {
        Task<bool> UserExists(string username);
        Task<User> CreateUser(ProfileCreateRequestCommand profileCreateRequestCommand);
        Task<User> GetUser(string username);
        Task UpdateUser(User user);
    }

    public class UserService : IUserService
    {
        protected readonly ICouchbaseService _couchbaseService;
        protected readonly ILogger<CouchDatabaseInintService> _logger;
        protected readonly CouchbaseConfig _couchbaseConfig;
        protected ICouchbaseCollection userCollection {
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


    }
}
