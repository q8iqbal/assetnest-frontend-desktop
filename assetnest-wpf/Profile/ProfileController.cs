using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Velacro.Api;
using Velacro.Basic;

namespace assetnest_wpf.Profile
{
    class ProfileController : MyController
    {
        public ProfileController(IMyView _myView) : base(_myView)
        {

        }

        public object ApiConstant { get; private set; }

        public async void profile(String token)
        {
            var client = new ApiClient(ApiConstant.BASE_URL);
            var request = new ApiRequestBuilder();

            var req = request
                .buildHttpRequest()
                .setEndpoint("api/users/")
                .setRequestMethod(HttpMethod.Get);
            client.setAuthorizationToken(token);
            client.setOnSuccessRequest(setUser);
            var response = await client.sendRequest(request.getApiRequestBundle());
        }

        private void setUser(HttpResponseBundle _response)
        {
            if (_response.getHttpResponseMessage().Content != null)
            {
                String status = _response.getHttpResponseMessage().ReasonPhrase;
                Console.WriteLine("BAWAH");
                Console.WriteLine(_response.getHttpResponseMessage<Root>().profile);
                getView().callMethod("setUser", _response.getParsedObject<Root>().profile);
            }
        }
    }
}
