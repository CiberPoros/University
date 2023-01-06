using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphsGenerator.DataAccess
{
    public interface IDataAccessService
    {
        public Task UpsertGraphWithVector(IEnumerable<GraphWithVectorModel> graphWithVectorModels);
    }
}
