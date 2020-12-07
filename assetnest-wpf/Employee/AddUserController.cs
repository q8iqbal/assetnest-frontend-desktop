using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Velacro.Api;
using Velacro.Basic;
using Newtonsoft.Json.Linq;

namespace assetnest_wpf.Employee
{
    public class AddUserController : MyController
    {
        public AddUserController(IMyView _myView) : base(_myView) { }

        public async void save(
            string _name,
            string _email,
            string _role)
        {
            var client = new ApiClient("http://api.assetnest.me/");
            var request = new ApiRequestBuilder();
            JObject userValue = new JObject();
            JObject user = new JObject();
            userValue.Add("name", _name);
            userValue.Add("email", _email);
            userValue.Add("password", _name);
            userValue.Add("role", _role);
            user.Add("user", userValue);

            var req = request
                .buildHttpRequest()
                .addJSON<JObject>(user)
                .setEndpoint("users")
                .setRequestMethod(HttpMethod.Post);
            string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOlwvXC9hcGkuYXNzZXRuZXN0Lm1lXC9sb2dpblwvbW9iaWxlIiwiaWF0IjoxNjA3MjMxMTkzLCJuYmYiOjE2MDcyMzExOTMsImp0aSI6InRLTXQ2TnRYOERzeWlVOG0iLCJzdWIiOjYsInBydiI6IjIzYmQ1Yzg5NDlmNjAwYWRiMzllNzAxYzQwMDg3MmRiN2E1OTc2ZjcifQ.i2H2RujIEtic9ibUZjI74sx6HQ-VqF-JBkxwOwPvsqI";
            client.setAuthorizationToken(token);
            client.setOnSuccessRequest(setAddUserStatus);
            var response = await client.sendRequest(request.getApiRequestBundle());

        }

        private async void setAddUserStatus(HttpResponseBundle _response)
        {
            if (_response.getHttpResponseMessage().Content != null)
            {
                string status = _response.getHttpResponseMessage().ReasonPhrase;
                string response = await _response.getHttpResponseMessage().Content.ReadAsStringAsync();
                getView().callMethod("setAddUserStatus", status);
                Trace.WriteLine(response);
            }
        }


    }
}

