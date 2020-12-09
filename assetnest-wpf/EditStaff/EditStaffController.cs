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

        public async void updateStaff(int staffId, string name, string role, 
                                      string email, string image)
        {
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
            var requestBundle = request.getApiRequestBundle();
            HttpResponseBundle response = null;
            //            string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOlwvXC9hcGkuYXNzZXRuZXN0Lm1lXC9sb2dpblwvbW9iaWxlIiwiaWF0IjoxNjA3MTM4MDA1LCJuYmYiOjE2MDcxMzgwMDUsImp0aSI6Im5pdWtZaW1iRnFKa2I5OEgiLCJzdWIiOjEsInBydiI6IjIzYmQ1Yzg5NDlmNjAwYWRiMzllNzAxYzQwMDg3MmRiN2E1OTc2ZjcifQ.k91eXqRRdmihSbq3k-93BJZUeJmqS3Dmpun3sw2efbg";

            Trace.WriteLine("Request : \n" + requestBundle.getJSON());
            client.setAuthorizationToken(StorageUtil.Instance.token);
            client.setOnSuccessRequest(onSuccessUpdateStaff);
            client.setOnFailedRequest(onFailedUpdateStaff);

            try
            {
                response = await client.sendRequest(request.getApiRequestBundle());
            }
            catch (Exception e)
            {
                getView().callMethod("showErrorMessage", "Error updating staff. " + e.Message);
            }

            Trace.WriteLine("Response : \n" + 
                    await response.getHttpResponseMessage().Content.ReadAsStringAsync());
        }

        private async void onSuccessUpdateStaff(HttpResponseBundle _response)
        {
            string reasonPhrase = "";

            if (_response.getHttpResponseMessage().Content != null)
            {
                Trace.WriteLine(await _response.getHttpResponseMessage().Content.ReadAsStringAsync());
                reasonPhrase = "Reason Phrase: " + _response.getHttpResponseMessage().ReasonPhrase;
            }
            getView().callMethod("showSuccessMessage", "Staff updated successfully. " + reasonPhrase);
        }

        private async void onFailedUpdateStaff(HttpResponseBundle _response)
        {
            string reasonPhrase = "";

            if (_response.getHttpResponseMessage().Content != null)
            {
                Trace.WriteLine(await _response.getHttpResponseMessage().Content.ReadAsStringAsync());
                reasonPhrase = "Reason Phrase: " + _response.getHttpResponseMessage().ReasonPhrase;
            }
            getView().callMethod("showErrorMessage", "Error updating staff. " + reasonPhrase);
        }

        public async void getStaff(string staffId)
        {
            var client = ApiUtil.Instance.vClient;
            var requestBuilder = new ApiRequestBuilder();
            var request = requestBuilder
                .buildHttpRequest()
                .setRequestMethod(HttpMethod.Get)
                .setEndpoint("users/" + staffId.ToString());
            var requestBundle = request.getApiRequestBundle();
            HttpResponseBundle response = null;
            //            string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOlwvXC9hcGkuYXNzZXRuZXN0Lm1lXC9sb2dpblwvbW9iaWxlIiwiaWF0IjoxNjA3MTM4MDA1LCJuYmYiOjE2MDcxMzgwMDUsImp0aSI6Im5pdWtZaW1iRnFKa2I5OEgiLCJzdWIiOjEsInBydiI6IjIzYmQ1Yzg5NDlmNjAwYWRiMzllNzAxYzQwMDg3MmRiN2E1OTc2ZjcifQ.k91eXqRRdmihSbq3k-93BJZUeJmqS3Dmpun3sw2efbg";

            client.setAuthorizationToken(StorageUtil.Instance.token);
            client.setOnSuccessRequest(onSuccessGetStaff);
            client.setOnFailedRequest(onFailedGetStaff);

            try
            {
                response = await client.sendRequest(request.getApiRequestBundle());
            }
            catch (Exception e)
            {
                getView().callMethod("showErrorMessage", 
                                     "Error initializing staff. " + e.Message);
            }

            Trace.WriteLine("Response : \n" +
                    await response.getHttpResponseMessage().Content.ReadAsStringAsync());
        }

        private async void onSuccessGetStaff(HttpResponseBundle _response)
        {
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

                Trace.WriteLine(await _response.getHttpResponseMessage().Content.ReadAsStringAsync());

                getView().callMethod("initStaff", staff);
            }
        }

        private async void onFailedGetStaff(HttpResponseBundle _response)
        {
            string reasonPhrase = "";

            if (_response.getHttpResponseMessage().Content != null)
            {
                Trace.WriteLine(await _response.getHttpResponseMessage().Content.ReadAsStringAsync());
                reasonPhrase = "Reason Phrase: " + _response.getHttpResponseMessage().ReasonPhrase;
            }
            getView().callMethod("showErrorMessage", "Error initializing staff. " + reasonPhrase);
            getView().callMethod("navigateToStaffPage");
        }
    }
}
