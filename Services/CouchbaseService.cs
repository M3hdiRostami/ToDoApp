using Couchbase;
using Couchbase.Extensions.DependencyInjection;
using Couchbase.KeyValue;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using ToDoAPI.Uilities.config;

namespace ToDoAPI.Services
{
    public interface ICouchbaseService
    {

        public Task<ICouchbaseCollection> GetScopeCollection(string collectionName,string ScopeName="");
        public Task<ICluster> GetClusterAsync();
    }


    public class CouchbaseService : ICouchbaseService
    {
        public readonly IClusterProvider _clusterProvider;
        public readonly IBucketProvider _bucketProvider;
        private readonly ILogger<CouchDatabaseInintService> _logger;
        private readonly CouchbaseConfig _couchbaseConfig;

        public CouchbaseService(
            IClusterProvider clusterProvider,
            IBucketProvider bucketProvider,
            IOptions<CouchbaseConfig> options,
            ILogger<CouchDatabaseInintService> logger)
        {
            _clusterProvider = clusterProvider;
            _bucketProvider = bucketProvider;
            _couchbaseConfig = options.Value;
            _logger = logger;
           
        }

        public async Task<ICluster> GetClusterAsync()
        {
           return await _clusterProvider.GetClusterAsync();
        }
        public async Task<ICouchbaseCollection> GetScopeCollection(string collectionName,string ScopeName="")
        {
            if (string.IsNullOrEmpty(ScopeName))
                ScopeName = _couchbaseConfig.ScopeName;

            var bucket = await _bucketProvider.GetBucketAsync(_couchbaseConfig.BucketName);
            var Scope =await bucket.ScopeAsync(ScopeName);
            var Collection = await Scope.CollectionAsync(collectionName);

            return Collection;
        }
    }
}
