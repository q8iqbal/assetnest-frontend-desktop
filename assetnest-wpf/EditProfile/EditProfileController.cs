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
using assetnest_wpf.Utils;
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
            var client = ApiUtil.Instance.vClient;
            var request = new ApiRequestBuilder()
                .buildMultipartRequest(new MultiPartContent(files, fileKey))
                .setRequestMethod(HttpMethod.Post)
                .setEndpoint("users/image");
            HttpResponseBundle response = null;

            client.setAuthorizationToken(StorageUtil.Instance.token);

            try
            {
                response = await client.sendRequest(request.getApiRequestBundle());
            }
            catch(Exception e)
            {
                getView().callMethod("showErrorMessage", "Error uploading image. " + e.Message);
            }

            if (response != null)
            {
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
                else
                {
                    string reasonPhrase = response.getHttpResponseMessage().ReasonPhrase;
                    getView().callMethod("showErrorMessage", 
                                         "Error uploading image. Reason Phrase: " + reasonPhrase);
                }
            }

            return null;
        }

        public async void updateUser(int user_id, string name, string email, 
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

            var client = ApiUtil.Instance.vClient;
            var requestBuilder = new ApiRequestBuilder();
            var request = requestBuilder
                .buildHttpRequest()
                .setRequestMethod(HttpMethod.Put)
                .setEndpoint("users/" + user_id.ToString())
                .addJSON<JObject>(user);
            var requestBundle = request.getApiRequestBundle();
            HttpResponseBundle response = null;

            Trace.WriteLine("Request : " + requestBundle.getJSON());
            client.setAuthorizationToken(StorageUtil.Instance.token);
            client.setOnSuccessRequest(onSuccessPutUser);
            client.setOnFailedRequest(onFailedPutUser);

            try
            {
                response = await client.sendRequest(request.getApiRequestBundle());
            } 
            catch(Exception e)
            {
                getView().callMethod("showErrorMessage", "Error updating profile. " + e.Message);
            }
        }

        private async void onSuccessPutUser(HttpResponseBundle _response)
        {
            string reasonPhrase = "";

            if (_response.getHttpResponseMessage().Content != null)
            {
                Trace.WriteLine(await _response.getHttpResponseMessage().Content.ReadAsStringAsync());
                reasonPhrase = "Reason Phrase: " + _response.getHttpResponseMessage().ReasonPhrase;
            }
            getView().callMethod("showSuccessMessage", "Profile updated successfully. " + reasonPhrase);
        }

        private async void onFailedPutUser(HttpResponseBundle _response)
        {
            string reasonPhrase = "";

            if (_response.getHttpResponseMessage().Content != null)
            {
                Trace.WriteLine(await _response.getHttpResponseMessage().Content.ReadAsStringAsync());
                reasonPhrase = "Reason Phrase: " + _response.getHttpResponseMessage().ReasonPhrase;
            }
            getView().callMethod("showErrorMessage", "Error updating profile. " + reasonPhrase);
        }

        public async Task<JObject> getUser(int user_id)
        {
            var client = ApiUtil.Instance.vClient;
            var requestBuilder = new ApiRequestBuilder();
            var request = requestBuilder
                .buildHttpRequest()
                .setRequestMethod(HttpMethod.Put)
                .setEndpoint("users/" + user_id.ToString());
            var requestBundle = request.getApiRequestBundle();
            
            Trace.WriteLine("Request : " + requestBundle.getJSON());
            client.setAuthorizationToken(StorageUtil.Instance.token);

            var response = await client.sendRequest(request.getApiRequestBundle());

            return response.getJObject();
        }

        private async void setUser(HttpResponseBundle _response)
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
