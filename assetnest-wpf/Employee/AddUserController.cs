using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Velacro.Api;
using Velacro.Basic;

namespace assetnest_wpf.Employee
{
    public class AddUserController : MyController
    {
        public AddUserController(IMyView _myView) : base(_myView) { }

        public async void save(
            string _name,
            string _email,
            string token)
        {
            var client = new ApiClient("http://api.assetnest.me/");
            var request = new ApiRequestBuilder();
           
            var req = request
                .buildHttpRequest()
                .addParameters("name", _name)
                .addParameters("email", _email)
                .setEndpoint("api/users/")
                .setRequestMethod(HttpMethod.Post);
            client.setAuthorizationToken(token);
            client.setOnSuccessRequest(setAddUserStatus);
            var response = await client.sendRequest(request.getApiRequestBundle());

        }

        private void setAddUserStatus(HttpResponseBundle _response)
        {
            if (_response.getHttpResponseMessage().Content != null)
            {
                string status = _response.getHttpResponseMessage().ReasonPhrase;
                getView().callMethod("setAddUserStatus", status);
            }
        }


    }
}

