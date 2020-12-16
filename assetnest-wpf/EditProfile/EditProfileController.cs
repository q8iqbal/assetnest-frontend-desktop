using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using Velacro.Api;
using Velacro.Basic;

using assetnest_wpf.Model;
using assetnest_wpf.Utils;

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
                Trace.WriteLine("Response: \n" + response.getJObject().ToString());
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

        public async void updateUser(int userId, string name, string email, string password, 
                                     string currentImagePath, MyFile newImageFile)
        {
            getView().callMethod("startLoading");
            string newImagePath = await postUserImage(newImageFile);
            JObject userValue = new JObject();
            JObject user = new JObject();

            userValue.Add("name", name);
            userValue.Add("email", email);
            userValue.Add("role", "owner");
            userValue.Add("image", newImagePath == null ? currentImagePath : newImagePath);
            userValue.Add("password", password);
            user.Add("user", userValue);

            var client = ApiUtil.Instance.vClient;
            var requestBuilder = new ApiRequestBuilder();
            var request = requestBuilder
                .buildHttpRequest()
                .setRequestMethod(HttpMethod.Put)
                .setEndpoint("users/" + userId.ToString())
                .addJSON<JObject>(user);
            var requestBundle = request.getApiRequestBundle();
            HttpResponseBundle response = null;

            Trace.WriteLine("Request : \n" + requestBundle.getJSON());
            client.setAuthorizationToken(StorageUtil.Instance.token);
            client.setOnSuccessRequest(onSuccessPutUser);
            client.setOnFailedRequest(onFailedPutUser);

            try
            {
                response = await client.sendRequest(request.getApiRequestBundle());

                if (response.getHttpResponseMessage().IsSuccessStatusCode)
                {
                    setUserInLocalStorage(userId);
                }
            } 
            catch(Exception e)
            {
                getView().callMethod("endLoading");
                getView().callMethod("showErrorMessage", "Error updating profile. " + e.Message);
            }
        }

        private async void onSuccessPutUser(HttpResponseBundle _response)
        {
            string reasonPhrase = "";

            if (_response.getHttpResponseMessage().Content != null)
            {
                Trace.WriteLine("onSuccessPutUser Response: ");
                Trace.WriteLine(await _response.getHttpResponseMessage().Content.ReadAsStringAsync());
                reasonPhrase = "Reason Phrase: " + _response.getHttpResponseMessage().ReasonPhrase;
            }
            getView().callMethod("endLoading");
            getView().callMethod("showSuccessMessage", "Profile updated successfully. " + reasonPhrase);
        }

        private async void onFailedPutUser(HttpResponseBundle _response)
        {
            string reasonPhrase = "";

            if (_response.getHttpResponseMessage().Content != null)
            {
                Trace.WriteLine("Response: ");
                Trace.WriteLine(await _response.getHttpResponseMessage().Content.ReadAsStringAsync());
                reasonPhrase = "Reason Phrase: " + _response.getHttpResponseMessage().ReasonPhrase;
            }
            getView().callMethod("endLoading");
            getView().callMethod("showErrorMessage", "Error updating profile. " + reasonPhrase);
        }

        private async void setUserInLocalStorage(int userId)
        {
            HttpResponseBundle response = await getUser(userId);

            if (response != null && response.getHttpResponseMessage().IsSuccessStatusCode)
            {
                JObject userDataJson = (JObject)response.getJObject()["data"];

                StorageUtil.Instance.user = new User()
                {
                    id = (int)userDataJson["id"],
                    company_id = (int)userDataJson["company_id"],
                    name = (string)userDataJson["name"],
                    email = (string)userDataJson["email"],
                    role = (string)userDataJson["role"],
                    image = (string)userDataJson["image"]
                };
            }
            else
            {
                getView().callMethod("showErrorMessage", "Error getting updated user data.");
            }

            getView().callMethod("navigateToProfilePage");
        }
        
        public async Task<HttpResponseBundle> getUser(int userId)
        {
            var client = ApiUtil.Instance.vClient;
            var requestBuilder = new ApiRequestBuilder();
            var request = requestBuilder
                .buildHttpRequest()
                .setRequestMethod(HttpMethod.Get)
                .setEndpoint("users/" + userId.ToString());
            HttpResponseBundle response = null;

            client.setAuthorizationToken(StorageUtil.Instance.token);

            try
            {
                response = await client.sendRequest(request.getApiRequestBundle());
            } catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }

            return response;
        }
    }
}
