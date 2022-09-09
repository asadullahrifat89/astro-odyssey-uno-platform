using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace AstroOdysseyCore
{
    public class ServiceResponse
    {
        public string RequestUri { get; set; }

        public string ExternalError { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.OK;

        public object Result { get; set; }
    }
}
