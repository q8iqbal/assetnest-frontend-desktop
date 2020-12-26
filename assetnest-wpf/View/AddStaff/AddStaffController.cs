using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Velacro.Api;
using Velacro.Basic;
using Newtonsoft.Json.Linq;
using assetnest_wpf.Utils;

namespace assetnest_wpf.View.AddStaff
{
    public class AddStaffController : MyController
    {
        public AddStaffController(IMyView _myView) : base(_myView) { }

        public async void save(string _name, string _email, string _role)
        {
            getView().callMethod("startLoading");

            var client = ApiUtil.Instance.vClient;
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
            string token = StorageUtil.Instance.token;
            client.setAuthorizationToken(token);
            client.setOnSuccessRequest(setAddUserStatus);
            client.setOnFailedRequest(setAddUserStatus);
            
            try 
            { 
                var response = await client.sendRequest(request.getApiRequestBundle());
            }
            catch (Exception e)
            {
                getView().callMethod("endLoading");
                getView().callMethod("showErrorMessage", "Failed to add new staff. " + e.Message);
            }
        }

        private async void setAddUserStatus(HttpResponseBundle _response)
        {
            getView().callMethod("endLoading");
            if (_response.getHttpResponseMessage().Content != null)
            {
                string status = _response.getHttpResponseMessage().ReasonPhrase;
                string response = await _response.getHttpResponseMessage().Content.ReadAsStringAsync();
                getView().callMethod("setAddUserStatus", status);
                Trace.WriteLine(response);
                if (_response.getHttpResponseMessage().IsSuccessStatusCode)
                {
                    JObject userDataJson = null;
                    if (_response.getJObject() != null)
                    {
                        userDataJson = (JObject)_response.getJObject()["data"];
                    }
                    if (userDataJson != null)
                    {
                        int staffId = (int)userDataJson["id"];

                        getView().callMethod("navigateToStaffPage", staffId);
                    }

                }
            }
        }
    }
}

