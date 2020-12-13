using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Velacro.Api;
using Velacro.Basic;
using Newtonsoft.Json.Linq;
using assetnest_wpf.Model;
using assetnest_wpf.Utils;

namespace assetnest_wpf.EditStaff
{
    public class EditStaffController : MyController
    {
        public EditStaffController(IMyView _myView) : base(_myView)
        {
        }

        public async void getStaff(int staffId)
        {
            getView().callMethod("startLoading");
            var client = ApiUtil.Instance.vClient;
            var requestBuilder = new ApiRequestBuilder();
            var request = requestBuilder
                .buildHttpRequest()
                .setRequestMethod(HttpMethod.Get)
                .setEndpoint("users/" + staffId.ToString());
            HttpResponseBundle response = null;

            client.setAuthorizationToken(StorageUtil.Instance.token);
            client.setOnSuccessRequest(onSuccessGetStaff);
            client.setOnFailedRequest(onFailedGetStaff);

            try
            {
                response = await client.sendRequest(request.getApiRequestBundle());
                if (response.getHttpResponseMessage().Content != null)
                {
                    Trace.WriteLine("Response: \n" +
                        await response.getHttpResponseMessage().Content.ReadAsStringAsync());
                }
            }
            catch (Exception e)
            {
                getView().callMethod("endLoading");
                getView().callMethod("showErrorMessage",
                                     "Error initializing staff data. " + e.Message);
                getView().callMethod("navigateToStaffListPage");
            }
        }

        private void onSuccessGetStaff(HttpResponseBundle _response)
        {
            getView().callMethod("endLoading");
            if (_response.getHttpResponseMessage().Content != null)
            {
                JObject responseJSON = _response.getJObject();
                JObject userDataJSON = (JObject)responseJSON["data"];
                User staff = new User()
                {
                    id = (int)userDataJSON["id"],
                    company_id = (int)userDataJSON["company_id"],
                    name = (string)userDataJSON["name"],
                    email = (string)userDataJSON["email"],
                    image = (string)userDataJSON["image"],
                    role = (string)userDataJSON["role"]
                };
                getView().callMethod("initStaff", staff);
            }
            else
            {
                getView().callMethod("showErrorMessage", "Error initializing staff data.");
            }
            getView().callMethod("changeToShowStaffPage");
        }

        private void onFailedGetStaff(HttpResponseBundle _response)
        {
            string reasonPhrase = "";

            if (_response.getHttpResponseMessage().Content != null)
            {
                reasonPhrase = "Reason Phrase: " + _response.getHttpResponseMessage().ReasonPhrase;
            }
            getView().callMethod("endLoading");
            getView().callMethod("showErrorMessage", 
                                 "Error initializing staff data. " + reasonPhrase);
            getView().callMethod("changeToShowStaffPage");
        }

        public async void updateStaff(int staffId, string name, string email, 
                                      string role, string image)
        {
            getView().callMethod("startLoading");

            JObject userValue = new JObject();
            JObject user = new JObject();

            userValue.Add("name", name);
            userValue.Add("email", email);
            userValue.Add("role", role);
            userValue.Add("image", image);
            user.Add("user", userValue);

            var client = ApiUtil.Instance.vClient;
            var requestBuilder = new ApiRequestBuilder();
            var request = requestBuilder
                .buildHttpRequest()
                .setRequestMethod(HttpMethod.Put)
                .setEndpoint("users/" + staffId.ToString())
                .addJSON<JObject>(user);
            HttpResponseBundle response = null;

            Trace.WriteLine("Request : \n" + request.getApiRequestBundle().getJSON());
            client.setAuthorizationToken(StorageUtil.Instance.token);
            client.setOnSuccessRequest(onSuccessUpdateStaff);
            client.setOnFailedRequest(onFailedUpdateStaff);

            try
            {
                response = await client.sendRequest(request.getApiRequestBundle());
                if (response.getHttpResponseMessage().IsSuccessStatusCode &&
                    response.getHttpResponseMessage().Content != null)
                {
                    Trace.WriteLine("Response : \n" +
                            await response.getHttpResponseMessage().Content.ReadAsStringAsync());
                    getStaff(staffId);
                }
            }
            catch (Exception e)
            {
                getView().callMethod("endLoading");
                getView().callMethod("showErrorMessage", "Error updating staff. " + e.Message);
                getView().callMethod("resetFields");
                getView().callMethod("changeToShowStaffPage");
            }
        }

        private void onSuccessUpdateStaff(HttpResponseBundle _response)
        {
            string reasonPhrase = "";

            if (_response.getHttpResponseMessage().Content != null)
            {
                reasonPhrase = "Reason Phrase: " + _response.getHttpResponseMessage().ReasonPhrase;
            }
            getView().callMethod("endLoading");
            getView().callMethod("showSuccessMessage", "Staff updated successfully. " + reasonPhrase);
        }

        private void onFailedUpdateStaff(HttpResponseBundle _response)
        {
            string reasonPhrase = "";

            if (_response.getHttpResponseMessage().Content != null)
            {
                reasonPhrase = "Reason Phrase: " + _response.getHttpResponseMessage().ReasonPhrase;
            }
            getView().callMethod("endLoading");
            getView().callMethod("showErrorMessage", "Error updating staff. " + reasonPhrase);
        }

        public async void deleteStaff()
        {

        }
    }
}
