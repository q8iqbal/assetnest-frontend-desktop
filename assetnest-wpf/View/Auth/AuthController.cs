using assetnest_wpf.ApiResponse;
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
            var client = ApiUtil.Instance.client;
            var request = new RestRequest("login/mobile",Method.POST);
            request.AddJsonBody(new { user = new { email = email, password = password } });
            var response = await client.ExecuteAsync<LoginResponse>(request);
            view.endLoading();
            if (response.IsSuccessful)
            {
                ApiUtil.Instance.setToken(response.Data.token);
                StorageUtil.Instance.user = response.Data.user;
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
                view.onFailedLogin("Invalid Credential");
            }
        }
    }
}
