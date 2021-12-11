using Couchbase;
using Couchbase.Core.Exceptions;
using Couchbase.Extensions.DependencyInjection;
using Couchbase.Management.Buckets;
using Couchbase.Management.Collections;
using Couchbase.Query;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ToDoAPI.Uilities.config;

namespace ToDoAPI.Services
{
    public class CouchDatabaseInintService
    {
        private readonly IClusterProvider _clusterProvider;
        private readonly IBucketProvider _bucketProvider;
        private readonly ILogger<CouchDatabaseInintService> _logger;
        private readonly CouchbaseConfig _couchbaseConfig;

        public CouchDatabaseInintService(
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

        public async Task SetupDatabase()
        {
            ICluster cluster = null ;
            IBucket bucket;
            
            //try to create bucket, if exists will just fail which is fine
            try
            {
                cluster = await _clusterProvider.GetClusterAsync();
                if (cluster != null)
                {
                    var bucketSettings = new BucketSettings
                    {
                        Name = _couchbaseConfig.BucketName,
                        BucketType = BucketType.Couchbase,
                        RamQuotaMB = 256,
                        

                    };

                    await cluster.Buckets.CreateBucketAsync(bucketSettings);
                }
                else
                    throw new System.Exception("Can't create bucket - cluster is null, please check database configuration.");
            }

            catch (BucketExistsException)
            {
                _logger.LogWarning($"Bucket {_couchbaseConfig.BucketName} already exists");
            }
            catch(AuthenticationFailureException)
            {
                _logger.LogError($"Declared Credential for couchbase server is not configured or is invalid!");
            }
            catch (System.NullReferenceException)
            {
                _logger.LogError($"Can't access to Database server items,check if connection string is correct,declared user has FullAdmin role,cluster initial configuration is ok");
               
            }

            bucket = await _bucketProvider.GetBucketAsync(_couchbaseConfig.BucketName);
            if (bucket != null)
            {
                if (!_couchbaseConfig.ScopeName.StartsWith("_"))
                {
                    //try to create scope - if fails it's ok we are probably using default
                    try
                    {
                        await bucket.Collections.CreateScopeAsync(_couchbaseConfig.CollectionName);
                    }
                    catch (ScopeExistsException)
                    {
                        _logger.LogWarning($"Scope {_couchbaseConfig.ScopeName} already exists, probably default");
                    }
                    catch (HttpRequestException)
                    {
                        _logger.LogWarning($"HttpRequestExcecption when creating Scope {_couchbaseConfig.ScopeName}");
                    }
                }

                //try to create collection - if fails it's ok the collection probably exists
                try
                {
                    await bucket.Collections.CreateCollectionAsync(new CollectionSpec(_couchbaseConfig.ScopeName, _couchbaseConfig.CollectionName));
                }
                catch (CollectionExistsException)
                {
                    _logger.LogWarning($"Collection {_couchbaseConfig.CollectionName} already exists in {_couchbaseConfig.BucketName}.");
                }
                catch (HttpRequestException)
                {
                    _logger.LogWarning($"HttpRequestExcecption when creating collection  {_couchbaseConfig.CollectionName}");
                }

                //try to create index - if fails it probably already exists
                try
                {
                    await Task.Delay(5000);
                    var queries = new List<string>
                    {
                        
                        $"CREATE PRIMARY INDEX default_profile_index ON {_couchbaseConfig.BucketName}.{_couchbaseConfig.ScopeName}.{_couchbaseConfig.CollectionName}",
                        $"CREATE Primary INDEX on {_couchbaseConfig.BucketName}",
                        $"CREATE INDEX date_range ON {_couchbaseConfig.BucketName}.{_couchbaseConfig.ScopeName}.{_couchbaseConfig.CollectionName}(DISTINCT ARRAY v.date FOR v IN toDos END)"
                    };
                    foreach (var query in queries)
                    {
                        var result = await cluster.QueryAsync<dynamic>(query);
                        if (result.MetaData.Status != QueryStatus.Success)
                        {
                            throw new System.Exception($"Error create index didn't return proper results for index {query}");
                        }
                    }
                }
                catch (IndexExistsException)
                {
                    _logger.LogWarning($"Collection {_couchbaseConfig.CollectionName} already exists in {_couchbaseConfig.BucketName}.");
                }
            }
            else
                throw new System.Exception("Can't retreive bucket.");
        }
    }
}
