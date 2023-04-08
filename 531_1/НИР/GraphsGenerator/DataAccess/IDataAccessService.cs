using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphsGenerator.DataAccess
{
    public interface IDataAccessService
    {
        public Task UpsertGraphWithVector(IEnumerable<GraphWithVectorModel> graphWithVectorModels);
        public Task<(bool success, GraphWithVectorModel graphWithVectorModel)> TryGetGraphWithVectorModelByG6(string g6);
    }
}
