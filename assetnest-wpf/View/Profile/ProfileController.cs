using System;
using System.Net.Http;
using Velacro.Api;
using Velacro.Basic;
using System.Diagnostics;
using assetnest_wpf.Utils;

namespace assetnest_wpf.View.Profile
{
    class ProfileController : MyController
    {
        String token = StorageUtil.Instance.token;
        int id = StorageUtil.Instance.user.id;
        public ProfileController(IMyView _myView) : base(_myView)
        {
            Console.WriteLine(StorageUtil.Instance.company.name);
        }

        public async void profile()
        {
            getView().callMethod("setProfile", StorageUtil.Instance.user);
        }

        public async void logout()
        {
            var client = ApiUtil.Instance.vClient;
            var request = new ApiRequestBuilder();

            var req = request
                .buildHttpRequest()
                .setEndpoint("/logout?Authorization=Bearer" + token)
                .setRequestMethod(HttpMethod.Get);
            client.setAuthorizationToken(token);
            client.setOnSuccessRequest(navigateViewLogout);
            var response = await client.sendRequest(request.getApiRequestBundle());
            Trace.WriteLine(await response.getHttpResponseMessage().Content.ReadAsStringAsync());
        }

        public void navigateViewLogout(HttpResponseBundle _response)
        {
            if (_response.getHttpResponseMessage().Content != null)
            {
                String status = _response.getHttpResponseMessage().ReasonPhrase;
                StorageUtil.Instance.user = null;
                StorageUtil.Instance.company = null;
                StorageUtil.Instance.token = null;
                getView().callMethod("navigateToLogin");
            }
        }
    }
}