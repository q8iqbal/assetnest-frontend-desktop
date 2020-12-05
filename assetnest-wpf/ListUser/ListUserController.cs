using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velacro.Api;
using Velacro.Basic;
using System.Net.Http;
namespace assetnest_wpf.ListUser
{
    class ListUserController : MyController
    {
        public ListUserController(IMyView _myView) : base(_myView)
        {
        }
        public async void getUser(String token)
        {
            var client = new ApiClient("http://api.assetnest.me");
            var request = new ApiRequestBuilder();

            var req = request
                .buildHttpRequest()
                .setEndpoint("/users?page=2")
                .setRequestMethod(HttpMethod.Get);
            //Console.WriteLine(req.getApiRequestBundle().ToString());
            client.setAuthorizationToken(token);
            client.setOnSuccessRequest(setUser);
            var response = await client.sendRequest(request.getApiRequestBundle());
        }

        private void setUser(HttpResponseBundle _response)
        {
            if (_response.getHttpResponseMessage().Content != null)
            {
                String status = _response.getHttpResponseMessage().ReasonPhrase;
                Console.WriteLine(_response.getParsedObject<Root>().data.current_page);
                Console.WriteLine(_response.getJObject().ToString());
                //getView().callMethod("setProfile", _response.getParsedObject<RootModelProfile>().data);
            }
        }
    }
}
