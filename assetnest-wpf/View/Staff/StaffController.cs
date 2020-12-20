using System;
using System.Diagnostics;
using System.Net.Http;

using Velacro.Api;
using Velacro.Basic;

using Newtonsoft.Json.Linq;

using assetnest_wpf.Model;
using assetnest_wpf.Utils;

namespace assetnest_wpf.View.Staff
{
    public class StaffController : MyController
    {
        public StaffController(IMyView _myView) : base(_myView)
        {
        }

        public async void getStaff(int staffId)
        {
            ApiClient client = ApiUtil.Instance.vClient;
            ApiRequestBuilder requestBuilder;
            ApiRequestBundle requestBundle;
            HttpResponseBundle response = null;

            getView().callMethod("startLoading");
            requestBuilder = new ApiRequestBuilder()
                .buildHttpRequest()
                .setRequestMethod(HttpMethod.Get)
                .setEndpoint("users/" + staffId.ToString());
            requestBundle = requestBuilder.getApiRequestBundle();

            client.setAuthorizationToken(StorageUtil.Instance.token);
            client.setOnSuccessRequest(onSuccessGetStaff);
            client.setOnFailedRequest(onFailedGetStaff);

            try
            {
                response = await client.sendRequest(requestBundle);
                if (response.getHttpResponseMessage().Content != null)
                {
                    Trace.WriteLine("getStaff Response: \n" +
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
                getView().callMethod("changeToShowStaffPage");
            }
            else
            {
                getView().callMethod("showErrorMessage", "Error initializing staff data.");
                getView().callMethod("navigateToStaffListPage");
            }
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
            getView().callMethod("navigateToStaffListPage");
        }

        public async void updateStaff(int staffId, string name, string email, 
                                      string role, string image)
        {
            JObject userValue = new JObject();
            JObject user = new JObject();
            ApiClient client = ApiUtil.Instance.vClient;
            ApiRequestBuilder requestBuilder;
            ApiRequestBundle requestBundle;
            HttpResponseBundle response = null;

            getView().callMethod("startLoading");

            userValue.Add("name", name);
            userValue.Add("email", email);
            userValue.Add("role", role);
            userValue.Add("image", image);
            user.Add("user", userValue);

            requestBuilder = new ApiRequestBuilder()
                .buildHttpRequest()
                .setRequestMethod(HttpMethod.Put)
                .setEndpoint("users/" + staffId.ToString())
                .addJSON<JObject>(user);
            requestBundle = requestBuilder.getApiRequestBundle();
            
            client.setAuthorizationToken(StorageUtil.Instance.token);
            client.setOnSuccessRequest(onSuccessUpdateStaff);
            client.setOnFailedRequest(onFailedUpdateStaff);

            try
            {
                response = await client.sendRequest(requestBundle);
                if (response.getHttpResponseMessage().IsSuccessStatusCode &&
                    response.getHttpResponseMessage().Content != null)
                {
                    Trace.WriteLine("updateStaff Response : \n" +
                            await response.getHttpResponseMessage().Content.ReadAsStringAsync());
                    getStaff(staffId);
                }
            }
            catch (Exception e)
            {
                getView().callMethod("endLoading");
                getView().callMethod("showErrorMessage", "Failed updating staff. " + e.Message);
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
            getView().callMethod("showSuccessMessage", 
                                 "Staff updated successfully. " + reasonPhrase);
        }

        private void onFailedUpdateStaff(HttpResponseBundle _response)
        {
            string reasonPhrase = "";

            if (_response.getHttpResponseMessage().Content != null)
            {
                reasonPhrase = "Reason Phrase: " + _response.getHttpResponseMessage().ReasonPhrase;
            }
            getView().callMethod("endLoading");
            getView().callMethod("showErrorMessage", "Failed to update staff. " + reasonPhrase);
        }

        public async void deleteStaff(int staffId)
        {
            ApiClient client = ApiUtil.Instance.vClient;
            ApiRequestBuilder requestBuilder = new ApiRequestBuilder()
                .buildHttpRequest()
                .setRequestMethod(HttpMethod.Delete)
                .setEndpoint("users/" + staffId.ToString());
            ApiRequestBundle requestBundle = requestBuilder.getApiRequestBundle();
            HttpResponseBundle response = null;

            getView().callMethod("startLoading");
            client.setAuthorizationToken(StorageUtil.Instance.token);
            client.setOnSuccessRequest(onSuccessDeleteStaff);
            client.setOnFailedRequest(onFailedDeleteStaff);

            try
            {
                response = await client.sendRequest(requestBundle);
                if (response.getHttpResponseMessage().Content != null)
                {
                    Trace.WriteLine("deleteStaff Response: \n" +
                        await response.getHttpResponseMessage().Content.ReadAsStringAsync());
                }
            }
            catch (Exception e)
            {
                getView().callMethod("endLoading");
                getView().callMethod("showErrorMessage",
                                     "Failed to delete staff. " + e.Message);
            }
        }

        private void onSuccessDeleteStaff(HttpResponseBundle _response)
        {
            string reasonPhrase = "";

            if (_response.getHttpResponseMessage().Content != null)
            {
                reasonPhrase = "Reason Phrase: " + _response.getHttpResponseMessage().ReasonPhrase;
            }
            getView().callMethod("endLoading");
            getView().callMethod("showSuccessMessage", 
                                 "Staff deleted successfully. " + reasonPhrase);
            getView().callMethod("navigateToStaffListPage");
        }

        private void onFailedDeleteStaff(HttpResponseBundle _response)
        {
            string reasonPhrase = "";

            if (_response.getHttpResponseMessage().Content != null)
            {
                reasonPhrase = "Reason Phrase: " + _response.getHttpResponseMessage().ReasonPhrase;
            }
            getView().callMethod("endLoading");
            getView().callMethod("showErrorMessage", "Failed to delete staff. " + reasonPhrase);
        }
    }
}
