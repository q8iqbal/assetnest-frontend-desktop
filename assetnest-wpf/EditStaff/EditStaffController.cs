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

namespace assetnest_wpf.EditStaff
{
    public class EditStaffController : MyController
    {
        public EditStaffController(IMyView _myView) : base(_myView)
        {
        }

        public async void updateStaff(int staffId, User newStaffData)
        {
            JObject userValue = new JObject();
            JObject user = new JObject();

            userValue.Add("name", newStaffData.name);
            userValue.Add("email", newStaffData.email);
            userValue.Add("role", newStaffData.role);
            userValue.Add("image", newStaffData.image);
            user.Add("user", userValue);

            var client = new ApiClient("http://api.assetnest.me/");
            var requestBuilder = new ApiRequestBuilder();
            var request = requestBuilder
                .buildHttpRequest()
                .setRequestMethod(HttpMethod.Put)
                .setEndpoint("users/" + staffId.ToString())
                .addJSON<JObject>(user);
            var requestBundle = request.getApiRequestBundle();
            string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOlwvXC9hcGkuYXNzZXRuZXN0Lm1lXC9sb2dpblwvbW9iaWxlIiwiaWF0IjoxNjA3MTM4MDA1LCJuYmYiOjE2MDcxMzgwMDUsImp0aSI6Im5pdWtZaW1iRnFKa2I5OEgiLCJzdWIiOjEsInBydiI6IjIzYmQ1Yzg5NDlmNjAwYWRiMzllNzAxYzQwMDg3MmRiN2E1OTc2ZjcifQ.k91eXqRRdmihSbq3k-93BJZUeJmqS3Dmpun3sw2efbg";

            Trace.WriteLine("Request : " + requestBundle.getJSON());
            client.setAuthorizationToken(token);

            var response = await client.sendRequest(request.getApiRequestBundle());

            Trace.WriteLine(await response.getHttpResponseMessage().Content.ReadAsStringAsync());
        }
    }
}
