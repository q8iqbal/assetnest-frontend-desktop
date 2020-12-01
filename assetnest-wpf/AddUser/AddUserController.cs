using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Velacro.Api;
using Velacro.Basic;

namespace assetnest_wpf.AddUser
{
    public class AddUserController : MyController
    {
        private AddUserPage addUserPage;

        public AddUserController(IMyView _myView) : base(_myView) { }

        public AddUserController(AddUserPage addUserPage)
        {
            this.addUserPage = addUserPage;
        }

        public async void addUser(
            string _staffName,
            string _email,
            string _role)
        {
            var client = new ApiClient("http://api.assetnest.me/");
            var request = new ApiRequestBuilder();

            string token = "";
            var req = request
                .buildHttpRequest()
                .addParameters("name", _staffName)
                .addParameters("email", _email)
                .addParameters("email", _role)
                .setEndpoint("api/register/")
                .setRequestMethod(HttpMethod.Post);
            client.setOnSuccessRequest(setViewAddUserStatus);
            var response = await client.sendRequest(request.getApiRequestBundle());

        }

        private void setViewAddUserStatus(HttpResponseBundle _response)
        {
            if (_response.getHttpResponseMessage().Content != null)
            {
                string status = _response.getHttpResponseMessage().ReasonPhrase;
                getView().callMethod("setAddUserStatus", status);
            }
        }
    }
}
