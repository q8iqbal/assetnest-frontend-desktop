using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
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

        private async Task<bool> validatePassword(string currentPassword)
        {
            bool passwordValid = false;
            string currentEmail = StorageUtil.Instance.user.email;
            ApiClient client = ApiUtil.Instance.vClient;
            JObject userDataJson = new JObject();
            JObject userJson = new JObject();
            ApiRequestBuilder requestBuilder;
            ApiRequestBundle requestBundle;
            HttpResponseBundle responseBundle = null;

            userDataJson.Add("email", currentEmail);
            userDataJson.Add("password", currentPassword);
            userJson.Add("user", userDataJson);

            requestBuilder = new ApiRequestBuilder()
                .buildHttpRequest()
                .setRequestMethod(HttpMethod.Post)
                .addJSON<JObject>(userJson)
                .setEndpoint("login");
            requestBundle = requestBuilder.getApiRequestBundle();
            client.setOnSuccessRequest(null);
            client.setOnFailedRequest(null);

            try
            {
                responseBundle = await client.sendRequest(requestBundle);
            }
            catch (Exception e)
            {
                getView().callMethod("showErrorMessage",
                                     "Failed to validate current password. " + e.Message);
            }

            if (responseBundle != null)
            {
                HttpResponseMessage responseMessage = responseBundle.getHttpResponseMessage();

                if (responseMessage.IsSuccessStatusCode)
                {
                    passwordValid = true;
                }
                else if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    getView().callMethod("showErrorMessage", "Current password is invalid.");
                }
                else
                {
                    string reasonPhrase = "Reason Phrase: " + responseMessage.ReasonPhrase;

                    getView().callMethod("showErrorMessage",
                                         "Failed to validate current password. " + reasonPhrase);
                }
            }

            return passwordValid;
        }

        private async Task<string> postUserImage(MyFile imageFile)
        {
            MyList<string> fileKey;
            MyList<MyFile> files;
            ApiClient client = ApiUtil.Instance.vClient;
            ApiRequestBuilder requestBuilder;
            ApiRequestBundle requestBundle;
            HttpResponseBundle responseBundle = null;

            if (imageFile == null)
            {
                return null;
            }

            fileKey = new MyList<string>() { "image" };
            files = new MyList<MyFile>() { imageFile };
            requestBuilder = new ApiRequestBuilder()
                .buildMultipartRequest(new MultiPartContent(files, fileKey))
                .setRequestMethod(HttpMethod.Post)
                .setEndpoint("users/image");
            requestBundle = requestBuilder.getApiRequestBundle();
            client.setAuthorizationToken(StorageUtil.Instance.token);
            client.setOnSuccessRequest(null);
            client.setOnFailedRequest(null);
            try
            {
                responseBundle = await client.sendRequest(requestBundle);
            }
            catch(Exception e)
            {
                getView().callMethod("showErrorMessage", 
                                     "Failed uploading image. " + e.Message);
            }

            if (responseBundle != null)
            {
                Trace.WriteLine("Response: \n" + responseBundle.getJObject().ToString());
                if (responseBundle.getHttpResponseMessage().IsSuccessStatusCode)
                {
                    if (responseBundle.getHttpResponseMessage().Content != null)
                    {
                        JObject responseJSON = responseBundle.getJObject();
                        JObject dataJSON = (JObject)responseJSON["data"];
                        string imagePath = (string)dataJSON["path"];

                        return imagePath;
                    }
                } 
                else
                {
                    string reasonPhrase = responseBundle.getHttpResponseMessage().ReasonPhrase;

                    getView().callMethod("showErrorMessage", 
                                         "Failed uploading image. Reason Phrase: " + reasonPhrase);
                }
            }

            return null;
        }

        public async void updateUser(int userId, string name, string email, string currentPassword,
                                     string newPassword, string currentImagePath, 
                                     MyFile newImageFile)
        {
            string newImagePath;
            JObject userDataJson = new JObject();
            JObject userJson = new JObject();
            ApiClient client = ApiUtil.Instance.vClient;
            ApiRequestBuilder requestBuilder;
            ApiRequestBundle requestBundle;
            HttpResponseBundle response = null;

            getView().callMethod("startLoading");
            if (currentPassword != null && newPassword != null)
            {
                bool validatePasswordSuccess = await validatePassword(currentPassword);

                if (!validatePasswordSuccess)
                {
                    getView().callMethod("endLoading");
                    return;
                }
                userDataJson.Add("password", newPassword);
            }

            newImagePath = await postUserImage(newImageFile);
            userDataJson.Add("name", name);
            userDataJson.Add("email", email);
            userDataJson.Add("role", "owner");
            userDataJson.Add("image", newImagePath == null ? currentImagePath : newImagePath);
            userJson.Add("user", userDataJson);

            requestBuilder = new ApiRequestBuilder()
                .buildHttpRequest()
                .setRequestMethod(HttpMethod.Put)
                .setEndpoint("users/" + userId.ToString())
                .addJSON<JObject>(userJson);
            requestBundle = requestBuilder.getApiRequestBundle();

            client.setAuthorizationToken(StorageUtil.Instance.token);
            client.setOnSuccessRequest(onSuccessUpdateUser);
            client.setOnFailedRequest(onFailedUpdateUser);

            try
            {
                response = await client.sendRequest(requestBundle);
                if (response.getHttpResponseMessage().IsSuccessStatusCode)
                {
                    setUserInLocalStorage(userId);
                }
            } 
            catch(Exception e)
            {
                getView().callMethod("endLoading");
                getView().callMethod("showErrorMessage", "Failed updating profile. " + e.Message);
            }
        }

        private async void onSuccessUpdateUser(HttpResponseBundle _response)
        {
            string reasonPhrase = "";
            HttpResponseMessage responseMessage = _response.getHttpResponseMessage();

            if (responseMessage.Content != null)
            {
                Trace.WriteLine("onSuccessUpdateUser Response: ");
                Trace.WriteLine(await responseMessage.Content.ReadAsStringAsync());
                reasonPhrase = "Reason Phrase: " + responseMessage.ReasonPhrase;
            }
            getView().callMethod("endLoading");
            getView().callMethod("showSuccessMessage", 
                                 "Profile updated successfully. " + reasonPhrase);
        }

        private async void onFailedUpdateUser(HttpResponseBundle _response)
        {
            string reasonPhrase = "";
            HttpResponseMessage responseMessage = _response.getHttpResponseMessage();

            if (responseMessage.Content != null)
            {
                Trace.WriteLine("onFailedUpdateUser Response: ");
                Trace.WriteLine(await responseMessage.Content.ReadAsStringAsync());
                reasonPhrase = "Reason Phrase: " + responseMessage.ReasonPhrase;
            }
            getView().callMethod("endLoading");
            getView().callMethod("showErrorMessage", "Failed updating profile. " + reasonPhrase);
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
                getView().callMethod("showErrorMessage", "Failed getting updated user data.");
            }

            getView().callMethod("navigateToProfilePage");
        }
        
        public async Task<HttpResponseBundle> getUser(int userId)
        {
            ApiClient client = ApiUtil.Instance.vClient;
            ApiRequestBuilder requestBuilder = new ApiRequestBuilder()
                .buildHttpRequest()
                .setRequestMethod(HttpMethod.Get)
                .setEndpoint("users/" + userId.ToString());
            ApiRequestBundle requestBundle = requestBuilder.getApiRequestBundle();
            HttpResponseBundle response = null;

            client.setAuthorizationToken(StorageUtil.Instance.token);
            client.setOnSuccessRequest(null);
            client.setOnFailedRequest(null);
            try
            {
                response = await client.sendRequest(requestBundle);
            } 
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }

            return response;
        }
    }
}
