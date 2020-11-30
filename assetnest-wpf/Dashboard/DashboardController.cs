using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Velacro.Api;
using Velacro.Basic;
using static assetnest_wpf.Dashboard.ModelDashboard;

namespace assetnest_wpf.Dashboard
{
    class DashboardController : MyController
    {
        public DashboardController(IMyView _myView) : base(_myView) 
        {

        }

        public object ApiConstant { get; private set; }

        public async void dashboard(String token)
        {
            var client = new ApiClient("http://api.assetnest.me/");
            var request = new ApiRequestBuilder();

            var req = request
                .buildHttpRequest()
                .setEndpoint("api.assetnest.me/dashboard")
                .setRequestMethod(HttpMethod.Get);
            client.setAuthorizationToken(token);
            client.setOnSuccessRequest(setDashboard);
            var response = await client.sendRequest(request.getApiRequestBundle());
        }

        private void setDashboard(HttpResponseBundle _response)
        {
            if (_response.getHttpResponseMessage().Content != null)
            {
                String status = _response.getHttpResponseMessage().ReasonPhrase;
                Console.WriteLine(_response.getParsedObject<Root>().dashboard);
                getView().callMethod("setDashboard", _response.getParsedObject<Root>().dashboard);
            }
        }
    }
}
