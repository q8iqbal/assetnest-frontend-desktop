using assetnest_wpf.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Velacro.Api;
using Velacro.Basic;

namespace assetnest_wpf.View.Auth
{
    class AuthController
    {
        private AuthPage view;
        public AuthController(AuthPage view)
        {
            ApiUtil apiUtil = ApiUtil.Instance;
            this.view = view;
        }

        public void sendLoginRequest(String email, String password)
        {
            // var client = new ApiClient("http://api.assetnest.me/");
            // var request = new ApiRequestBuilder();

            //request
            //     .buildHttpRequest()
            //     .addParameters("email", email)
            //     .addParameters("password", password)
            //     .setEndpoint("login")
            //     .setRequestMethod(HttpMethod.Post);
            // //client.setOnSuccessRequest(setViewLoginStatus);
            // var response = await client.sendRequest(request.getApiRequestBundle());
            // Console.WriteLine(response.getJObject()["token"]);

            ApiUtil util = ApiUtil.Instance;
            var request = new RestRequest("login/mobile",Method.POST);
            request.AddJsonBody(new { user = new { email = email , password = password } });
            IRestResponse response = util.client.Execute(request);
            Console.WriteLine(response.Content);
        }
    }
}
