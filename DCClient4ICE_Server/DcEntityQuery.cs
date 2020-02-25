using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;

namespace DCClient4ICE_Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single, IncludeExceptionDetailInFaults = true)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    class DcEntityQuery: IDcEntityQuery
    {

        public List<DcEntity> GetInfo()
        {
            return DcServer.dcEntities.ToList();
        }

        public List<DcEntity> GetInfoGET()
        {
            return DcServer.dcEntities.ToList();
        }
    }
}
