using Couchbase;
using Couchbase.KeyValue;
using System.Threading.Tasks;

namespace ToDoAPI.Services
{
    public interface ICouchbaseService
    {

        public Task<ICouchbaseCollection> GetScopeCollection(string collectionName,string ScopeName="");
        public Task<ICluster> GetClusterAsync();
    }
}
