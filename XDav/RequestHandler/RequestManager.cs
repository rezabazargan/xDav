using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace XDav.RequestHandler
{
    public class RequestManager
    {

        public RequestManager()
        {
        }

        private List<RequestBase> _handles;
        internal List<RequestBase> Handlers
        {
            get
            {
                if(_handles == null)
                    _handles = RegisterHandles();
                return _handles;
            }
        }

        private List<RequestBase> RegisterHandles()
        {
            return new List<RequestBase>() { 
                new PutRequest(),
                new GetRequest(),
                new LockRequest(),
                new OptionRequest(),
                new UnlockRequest()
            };
        }


        public void HandleRequest(HttpContext context)
        {
            Handlers.ForEach(h => {
                if (h.Condition(context))
                    h.HandelRequest(context);
            });
        }

    }
}
