using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velacro.Api;
using Velacro.Basic;
using System.Net.Http;
using assetnest_wpf.Utils;

namespace assetnest_wpf.View.ListUser
{
    class ListUserController : MyController
    {

        public ListUserController(IMyView _myView) : base(_myView)
        {
        }
        public async void getUser(String filter)
        {
            var client = ApiUtil.Instance.vClient;
            var request = new ApiRequestBuilder();

            var req = request
                .buildHttpRequest()
                .setEndpoint("/users" + filter)
                .setRequestMethod(HttpMethod.Get);
            String token = StorageUtil.Instance.token;
            //Console.WriteLine(req.getApiRequestBundle().ToString());
            client.setAuthorizationToken(token);
            client.setOnSuccessRequest(setUser);
            var response = await client.sendRequest(request.getApiRequestBundle());
        }

        private void setUser(HttpResponseBundle _response)
        {
            if (_response.getHttpResponseMessage().Content != null)
            {
                //String status = _response.getHttpResponseMessage().ReasonPhrase;
                String totalData = _response.getJObject()["data"]["total"].ToString();
                Console.WriteLine("inilo = " + totalData);
                Data data = new Data();
                if (!totalData.Equals("0")) {
                    data = _response.getParsedObject<Root>().data;
                }
                getView().callMethod("showList", data);
            }
        }
    }
}