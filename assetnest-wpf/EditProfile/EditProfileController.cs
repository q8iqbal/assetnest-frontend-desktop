using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Velacro.Api;
using Velacro.Basic;
//using assetnest_wpf.Model;
using Newtonsoft.Json.Linq;

namespace assetnest_wpf.EditProfile
{
    public class EditProfileController : MyController
    {
        public EditProfileController(IMyView _myView) : base(_myView)
        {

        }


        private async Task<string> postUserImage(MyFile imageFile)
        {
            if (imageFile == null)
            {
                return null;
            }

            MyList<string> fileKey = new MyList<string>() { "image" };
            MyList<MyFile> files = new MyList<MyFile>() { imageFile };
            var client = new ApiClient("http://api.assetnest.me/");
            var request = new ApiRequestBuilder()
                .buildMultipartRequest(new MultiPartContent(files, fileKey))
                .setRequestMethod(HttpMethod.Post)
                .setEndpoint("users/image");
            string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOlwvXC9hcGkuYXNzZXRuZXN0Lm1lXC9sb2dpblwvbW9iaWxlIiwiaWF0IjoxNjA3MTM4MDA1LCJuYmYiOjE2MDcxMzgwMDUsImp0aSI6Im5pdWtZaW1iRnFKa2I5OEgiLCJzdWIiOjEsInBydiI6IjIzYmQ1Yzg5NDlmNjAwYWRiMzllNzAxYzQwMDg3MmRiN2E1OTc2ZjcifQ.k91eXqRRdmihSbq3k-93BJZUeJmqS3Dmpun3sw2efbg";

            client.setAuthorizationToken(token);

            var response = await client.sendRequest(request.getApiRequestBundle());

            Trace.WriteLine(response.getJObject().ToString());

            if (response.getHttpResponseMessage().IsSuccessStatusCode)
            {
                if (response.getHttpResponseMessage().Content != null)
                {
                    JObject responseJSON = response.getJObject();
                    JObject dataJSON = (JObject)responseJSON["data"];
                    string imagePath = (string)dataJSON["path"];

                    return imagePath;
                }
            }

            return null;
        }

        public async void putUser(int user_id, string name, string email, 
                                  string password, MyFile imageFile)
        {
            string imagePath = await postUserImage(imageFile);
            JObject userValue = new JObject();
            JObject user = new JObject();

            userValue.Add("name", name);
            userValue.Add("email", email);
            userValue.Add("role", "owner");
            userValue.Add("image", imagePath);
            userValue.Add("password", password);
            user.Add("user", userValue);

            var client = new ApiClient("http://api.assetnest.me/");
            var requestBuilder = new ApiRequestBuilder();
            var request = requestBuilder
                .buildHttpRequest()
                .setRequestMethod(HttpMethod.Put)
                .setEndpoint("users/" + user_id.ToString())
                .addJSON<JObject>(user);
            var requestBundle = request.getApiRequestBundle();
            string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOlwvXC9hcGkuYXNzZXRuZXN0Lm1lXC9sb2dpblwvbW9iaWxlIiwiaWF0IjoxNjA3MTM4MDA1LCJuYmYiOjE2MDcxMzgwMDUsImp0aSI6Im5pdWtZaW1iRnFKa2I5OEgiLCJzdWIiOjEsInBydiI6IjIzYmQ1Yzg5NDlmNjAwYWRiMzllNzAxYzQwMDg3MmRiN2E1OTc2ZjcifQ.k91eXqRRdmihSbq3k-93BJZUeJmqS3Dmpun3sw2efbg";

            Trace.WriteLine("Request : " + requestBundle.getJSON());
            client.setAuthorizationToken(token);
            
            var response = await client.sendRequest(request.getApiRequestBundle());
        }

        public async Task<JObject> getUser(int user_id)
        {
            var client = new ApiClient("http://api.assetnest.me/");
            var requestBuilder = new ApiRequestBuilder();
            var request = requestBuilder
                .buildHttpRequest()
                .setRequestMethod(HttpMethod.Put)
                .setEndpoint("users/" + user_id.ToString());
            var requestBundle = request.getApiRequestBundle();
            string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOlwvXC9hcGkuYXNzZXRuZXN0Lm1lXC9sb2dpblwvbW9iaWxlIiwiaWF0IjoxNjA3MTM4MDA1LCJuYmYiOjE2MDcxMzgwMDUsImp0aSI6Im5pdWtZaW1iRnFKa2I5OEgiLCJzdWIiOjEsInBydiI6IjIzYmQ1Yzg5NDlmNjAwYWRiMzllNzAxYzQwMDg3MmRiN2E1OTc2ZjcifQ.k91eXqRRdmihSbq3k-93BJZUeJmqS3Dmpun3sw2efbg";

            Trace.WriteLine("Request : " + requestBundle.getJSON());
            client.setAuthorizationToken(token);

            var response = await client.sendRequest(request.getApiRequestBundle());

            return response.getJObject();
        }

        private async void setViewUser(HttpResponseBundle _response)
        {
            HttpResponseMessage responseMessage = _response.getHttpResponseMessage();
            HttpContent responseContent = responseMessage.Content;

            if (_response.getHttpResponseMessage().Content != null)
            {
                string status = _response.getHttpResponseMessage().ReasonPhrase;
                string content = await responseContent.ReadAsStringAsync();

                Trace.WriteLine("Reason Phrase :" + status);
                Trace.WriteLine("Response Body :" + content);
            }
        }
    }
}
