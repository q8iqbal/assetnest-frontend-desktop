using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Velacro.Api;
using Velacro.Basic;

namespace assetnest_wpf.View.Profile
{
    class ProfileController : MyController
    {
        public ProfileController(IMyView _myView) : base(_myView)
        {

        }

        public async void profile(string role, string name, string email)
        {
            var client = new ApiClient("http://api.assetnest.me/");
            var request = new ApiRequestBuilder();

            var req = request
                .buildHttpRequest()
                .addParameters("role", role)
                .addParameters("name", name)
                .addParameters("email", email)
                .setEndpoint("api/users/")
                .setRequestMethod(HttpMethod.Post);
            //client.setOnSuccessRequest(setViewLoginStatus);
            var response = await client.sendRequest(request.getApiRequestBundle());
            Console.WriteLine(response.getJObject()["access_token"]);
            client.setAuthorizationToken(response.getJObject()["access_token"].ToString());
        }
    }
}