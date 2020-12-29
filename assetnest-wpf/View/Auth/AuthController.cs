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
using System.Windows.Forms;
using Velacro.Api;
using Velacro.Basic;

namespace assetnest_wpf.View.Auth
{
    class AuthController
    {
        private AuthPage view;
        private RestClient client = ApiUtil.Instance.client;
        public AuthController(AuthPage view)
        {
            this.view = view;
        }

        public async void sendLoginRequest(String email, String password)
        {
            view.startLoading();
            var request = new RestRequest("login/mobile", Method.POST);
            request.AddJsonBody(new { user = new { email = email, password = password } });
            var response = await this.client.ExecuteAsync<LoginResponse>(request);
            view.endLoading();
            if (response.IsSuccessful)
            {
                ApiUtil.Instance.setToken(response.Data.token);
                StorageUtil.Instance.user = response.Data.user;
                StorageUtil.Instance.token = response.Data.token;
                var req = new RestRequest("companies", Method.GET);
                var respons = await this.client.ExecuteAsync<GetCompanyResponse>(req);
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

        public async void sendRegisterRequest(User user, Company company, string uImage , string cImage)
        {
            view.startLoading();
            var request = new RestRequest("register/", Method.POST);

            if(!string.IsNullOrWhiteSpace(uImage))
            {
                var req = new RestRequest("users/image", Method.POST);
                req.AddFile("image",uImage);
                var resp = await this.client.ExecuteAsync(req);
                JObject obj = JObject.Parse(resp.Content);
                user.image = obj["data"]["path"].ToString();
            }

            if (!string.IsNullOrWhiteSpace(cImage))
            {
                var req = new RestRequest("companies/image", Method.POST);
                req.AddFile("image", cImage);
                var resp = await this.client.ExecuteAsync(req);
                JObject obj = JObject.Parse(resp.Content);
                company.image = obj["data"]["path"].ToString();
                
            }

            object body = (new {
                user = new {
                    name = user.name,
                    email = user.email,
                    password = user.password,
                    image = user.image != null ? user.image : null
                }, 
                company = new{
                    name = company.name,
                    address = company.address,
                    phone = company.phone,
                    description = company.description,
                    image = company.image != null ? company.image : null
                } 
            });

            request.AddJsonBody(body);
            var client = ApiUtil.Instance.client;
            var response = await client.ExecuteAsync(request);   
            view.endLoading();

            if (response.IsSuccessful)
            {
                view.onSuccessRegister();
            }
            else
            {
                view.showMessage("Failed Register");
            }
        }
    }
}
