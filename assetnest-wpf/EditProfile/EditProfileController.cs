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

namespace assetnest_wpf.EditProfile
{
    public class EditProfileController : MyController
    {
        public EditProfileController(IMyView _myView) : base(_myView)
        {

        }
        /*
        public async void uploadImage(int user_id, MyFile image)
        {
            MyList<string> fileKey = new MyList<string>() { "image" };
            MyList<MyFile> files = new MyList<MyFile>() { image };
            var request = new ApiRequestBuilder();
            var multiPartContent = new MultiPartContent(files, fileKey);
            var multiPartRequest = request
                .buildMultipartRequest(multiPartContent)
                .setRequestMethod(HttpMethod.Post)
                .setEndpoint("/users/");

            
        }
        */
        public async void updateUser(int user_id, string name, string email, string image)
        { /*
            var client = new ApiClient("http://api.assetnest.me");
            var requestBuilder = new ApiRequestBuilder();
            var request = requestBuilder
                .buildHttpRequest()
                .setRequestMethod(HttpMethod.Put)
                .setEndpoint("/users/" + user_id.ToString())
                .addParameters("name", name)
                .addParameters("email", email)
                .addParameters("role", "owner")
                .addParameters("image", image);
            request.getApiRequestBundle().getParameters();
            String token = "Bearer " + "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOlwvXC9hcGkuYXNzZXRuZXN0Lm1lXC9sb2dpblwvbW9iaWxlIiwiaWF0IjoxNjA2ODk4NTk2LCJuYmYiOjE2MDY4OTg1OTYsImp0aSI6IkZ0RjZhMXg5WGZiZ2prbHMiLCJzdWIiOjEsInBydiI6IjIzYmQ1Yzg5NDlmNjAwYWRiMzllNzAxYzQwMDg3MmRiN2E1OTc2ZjcifQ._wRv5ibsdISmlkmXfCMRR3oNdTJNfSSzQKZ80qeZ2qo";

            client.setAuthorizationToken(token);
            var response = await client.sendRequest(request.getApiRequestBundle());
            Trace.WriteLine("Put User Response : " + response.getJObject().ToString());*/
        }

        public async void updateUserPassword(int user_id, String password)
        {/*
            var client = new ApiClient("http://api.assetnest.me");
            var requestBuilder = new ApiRequestBuilder();
            var request = requestBuilder
                .buildHttpRequest()
                .setRequestMethod(HttpMethod.Put)
                .setEndpoint("/users/" + user_id.ToString() + "/password")
                .addParameters("password", password);
            String token = "Bearer " + "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOlwvXC9hcGkuYXNzZXRuZXN0Lm1lXC9sb2dpblwvbW9iaWxlIiwiaWF0IjoxNjA2ODk4NTk2LCJuYmYiOjE2MDY4OTg1OTYsImp0aSI6IkZ0RjZhMXg5WGZiZ2prbHMiLCJzdWIiOjEsInBydiI6IjIzYmQ1Yzg5NDlmNjAwYWRiMzllNzAxYzQwMDg3MmRiN2E1OTc2ZjcifQ._wRv5ibsdISmlkmXfCMRR3oNdTJNfSSzQKZ80qeZ2qo";

            client.setAuthorizationToken(token);
            var response = await client.sendRequest(request.getApiRequestBundle());
            Trace.WriteLine("Put User Password Response " + response.getJObject().ToString());*/
        }
    }
}
