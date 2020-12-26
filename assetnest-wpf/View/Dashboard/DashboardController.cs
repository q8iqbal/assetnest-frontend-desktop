using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Velacro.Api;
using Velacro.Basic;
using Newtonsoft.Json.Linq;
using assetnest_wpf.Utils;
using assetnest_wpf.Model;

namespace assetnest_wpf.View.Dashboard
{
    class DashboardController : MyController
    {
        public DashboardController(IMyView _myView) : base(_myView)
        {

        }

        public void getCompany()
        {
            Company company = StorageUtil.Instance.company;

            getView().callMethod("setCompany", company);
        }

        public async void getUserTotal(string role)
        {
            var client = new ApiClient(Constants.BASE_URL);
            var requestBuilder = new ApiRequestBuilder();
            var request = requestBuilder
                .buildHttpRequest()
                .setEndpoint("users?filter[role]=" + role)
                .setRequestMethod(HttpMethod.Get);
            string token = StorageUtil.Instance.token;
            client.setAuthorizationToken(token);
            if (role.Equals("admin"))
            {
                client.setOnSuccessRequest(setViewAdminTotal);
                client.setOnFailedRequest(setViewAdminTotal);
            }
            else
            {
                client.setOnSuccessRequest(setViewUserTotal);
                client.setOnFailedRequest(setViewUserTotal);
            }
            var response = await client.sendRequest(request.getApiRequestBundle());
        }

        public async void getTotal()
        {
            var client = new ApiClient(Constants.BASE_URL);
            var requestBuilder = new ApiRequestBuilder();
            var request = requestBuilder
                .buildHttpRequest()
                .setEndpoint("users")
                .setRequestMethod(HttpMethod.Get);
            string token = StorageUtil.Instance.token;
            client.setAuthorizationToken(token);
            client.setOnSuccessRequest(setViewTotal);
            var response = await client.sendRequest(request.getApiRequestBundle());
        }

        private void setViewUserTotal(HttpResponseBundle _response)
        {
            if (_response.getHttpResponseMessage().Content != null)
            {
                int userTotal = (int)((JObject)_response.getJObject()["data"])["total"];
                getView().callMethod("setUserTotal", userTotal);
            }
        }

        private void setViewAdminTotal(HttpResponseBundle _response)
        {
            if (_response.getHttpResponseMessage().Content != null)
            {
                int adminTotal = (int)((JObject)_response.getJObject()["data"])["total"];
                getView().callMethod("setAdminTotal", adminTotal);
            }
        }
        private void setViewTotal(HttpResponseBundle _response)
        {
            if (_response.getHttpResponseMessage().Content != null)
            {
                int total = (int)((JObject)_response.getJObject()["data"])["total"];
                getView().callMethod("setTotal", total);
            }
        }
    }
}