using assetnest_wpf.ApiResponse;
using assetnest_wpf.Model;
using assetnest_wpf.Utils;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
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

        public async void sendLoginRequest(String email, String password)
        {
            view.startLoading();
            //JObject json=  JObject.FromObject(new { email=email, password=password });
            //var req = new ApiRequestBuilder();
            //req
            //    .buildHttpRequest()
            //    .addParameters("user", json.ToString())
            //    .setEndpoint("login/mobile")
            //    .setRequestMethod(HttpMethod.Post);

            //var response = await ApiUtil.Instance.vClient.sendRequest(req.getApiRequestBundle());
            //Console.WriteLine(response.getJObject()["token"]);

            var client = ApiUtil.Instance.client;
            var request = new RestRequest("login/mobile", Method.POST);
            request.AddJsonBody(new { user = new { email = email, password = password } });
            var response = await client.ExecuteAsync<LoginResponse>(request);
            view.endLoading();
            if (response.IsSuccessful)
            {
                ApiUtil.Instance.setToken(response.Data.token);
                StorageUtil.Instance.user = response.Data.user;
                StorageUtil.Instance.token = response.Data.token;
                var req = new RestRequest("companies", Method.GET);
                var respons = await client.ExecuteAsync<GetCompanyResponse>(req);
                if (respons.IsSuccessful)
                {
                    StorageUtil.Instance.company = respons.Data.data;
                }
                view.onSuccessLogin();
            }
            else
            {
                view.showMessage("Invalid Credential");
            }
        }

        public async void sendRegisterRequest(object user, object company)
        {
            view.startLoading();
            var request = new RestRequest("register/", Method.POST);
            request.AddJsonBody(new { user = user, company = company });
            Console.WriteLine(new { user = user, company = company });
            var client = ApiUtil.Instance.client;
            var response = await client.ExecuteAsync(request);   
            view.endLoading();

            if (response.IsSuccessful)
            {
                view.onSuccessRegister();
            }
            else
            {
                view.showMessage("ga masok");
            }
        }
    }
}
