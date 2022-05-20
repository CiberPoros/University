using System.Collections.Generic;

namespace Generators.Programs.Analizer.Criteria
{
    internal interface ICriteria
    {
        (double val, bool isAccepted) CheckCriteria(IEnumerable<double> values);

        string Name { get; }
    }
}
