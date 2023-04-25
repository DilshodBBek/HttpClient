using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace LessonHttp
{
    internal class Program
    {

        static async Task Main(string[] args)
        {
            var service = new ServiceCollection();
            service.AddHttpClient();
            var provider = service.BuildServiceProvider();
            var factory = provider.GetService<IHttpClientFactory>();
            var _httpClient = factory?.CreateClient();

            //HttpClientSample(_httpClien);
            //HttpCrud(_httpClient);
            //await FileShareHttp(_httpClient);
            // await MiltipartFileShareHttp(_httpClient);
            await CookieExample(_httpClient);


            Console.ReadKey();


        }

        private static async Task CookieExample(HttpClient? httpClient)
        {

            Uri uri = new Uri("https://localhost:7154/");

            using var response = await httpClient.GetAsync(uri);

            CookieContainer cookies = new();

            foreach (string cookieHeader1 in response.Headers.GetValues("Set-Cookie"))
                cookies.SetCookies(uri, cookieHeader1);

            foreach (Cookie cookie in cookies.GetCookies(uri))
                Console.WriteLine($"{cookie.Name}: {cookie.Value}");

            Cookie? email = cookies.GetCookies(uri).FirstOrDefault(c => c.Value == "Elyor_bek");
            Console.WriteLine($"Электронный адрес: {email?.Name}");

            using var response1 = await httpClient.GetAsync(uri+ "test/");

            foreach (string cookieHeader1 in response.Headers.GetValues("Set-Cookie"))
                await Console.Out.WriteLineAsync(cookieHeader1);

            // Uri uri = new Uri("http://google.com");

            //Cookie nameCookie = new Cookie("name", "Tom");
            //Cookie emailCookie = new Cookie("email", "tom@localhost.com");

            //CookieContainer cookieContainer = new();

            //cookieContainer.SetCookies(uri, "name=Tom");
            //cookieContainer.Add(uri, nameCookie);

            //cookieContainer.Add(uri, emailCookie);

            //var allCookies = cookieContainer.GetAllCookies();
            //await Console.Out.WriteLineAsync("GetAllCookies");
            //foreach (Cookie cookie in allCookies)
            //{
            //    Console.WriteLine($"{cookie.Name} : {cookie.Value}");
            //}

            //var uriCookies = cookieContainer.GetCookies(uri);
            //Console.WriteLine("GetCookies(uri);");
            //foreach (Cookie cookie in uriCookies)
            //{
            //    Console.WriteLine($"{cookie.Name} : {cookie.Value}");
            //}
            //await Console.Out.WriteLineAsync("GetCookieHeader");
            //var cookieHeader = cookieContainer.GetCookieHeader(uri);
            //Console.WriteLine(cookieHeader);
        }

        private static async Task MiltipartFileShareHttp(HttpClient? _httpClient)
        {
            string[] files = { @"C:\Users\User\Desktop\nature.jpg" };//, @"C:\Users\User\Desktop\nature1.jpg" };

            using MultipartFormDataContent multipartFormDataContent = new();
            multipartFormDataContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            foreach (var file in files)
            {
                StreamContent streamContent = new StreamContent(File.OpenRead(file));
                multipartFormDataContent.Add(streamContent, "file", Path.GetFileName(file));
            }

            var response = await _httpClient.PostAsync("http://10.10.1.145:5000/downloads", multipartFormDataContent);

            await Console.Out.WriteLineAsync(await response.Content.ReadAsStringAsync());
        }

        private static async Task FileShareHttp(HttpClient _httpClient)
        {
            string uriVideo = "https://www.instagram.com/reel/CrVfo-cri3P/?igshid=YmMyMTA2M2Y%3D";
            HttpResponseMessage response = await _httpClient.GetAsync(uriVideo);

            // await Console.Out.WriteLineAsync(await response.Content.ReadAsStringAsync());
            using FileStream fs = new FileStream(@"C:\Users\User\Desktop\nature2.mp4", FileMode.Create);

            await response.Content.CopyToAsync(fs);
            await Console.Out.WriteLineAsync("Finished");

            //StreamContent content = new(fileStream);

            //HttpRequestMessage httpRequest = new(HttpMethod.Post, "http://10.10.1.145:5000/upload");
            //httpRequest.Content = content;

            //var response = await _httpClient.SendAsync(httpRequest);
            //await Console.Out.WriteLineAsync(await response.Content.ReadAsStringAsync());
            //await Console.Out.WriteLineAsync(response.StatusCode.ToString());

        }

        private static async Task HttpCrud(HttpClient _httpClient)
        {
            HttpRequestMessage requestMessage = new(HttpMethod.Get, "https://localhost:7154/");
            requestMessage.Headers.Add("User-Agent", "1234567");
            HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);
            var bytes = await response.Content.ReadAsByteArrayAsync();
            await Console.Out.WriteLineAsync(Encoding.UTF8.GetString(bytes));


            StringContent content = new("My name is Dilshod");

            var responsePost = await _httpClient.PostAsync("https://localhost:7154/", content);
            await Console.Out.WriteLineAsync(responsePost.StatusCode.ToString());
            await Console.Out.WriteLineAsync(await responsePost.Content.ReadAsStringAsync());
            await Console.Out.WriteLineAsync(responsePost.Headers.ToString());
        }

        private static async Task<HttpResponseMessage> HttpClientStatic()
        {
            HttpClient httpClient;
            HttpRequestMessage requestMessage = new(HttpMethod.Get, "https://localhost:7154/2");
            var socketHandler = new SocketsHttpHandler()
            {
                PooledConnectionLifetime = TimeSpan.FromSeconds(2),
            };
            httpClient = new HttpClient(socketHandler);
            HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
            await Console.Out.WriteLineAsync(await response.Content.ReadAsStringAsync());
            await Console.Out.WriteLineAsync(response.StatusCode.ToString());
            return response;
        }

        private static async Task HttpClientSample(HttpClient _httpClient)
        {
            Dictionary<string, string> _students = new();
            _students.Add("1", "Ali");
            _students.Add("2", "Xurshid");
            _students.Add("3", "Javlon");

            FormUrlEncodedContent DictioanryContent = new FormUrlEncodedContent(_students);

            var fromResponse = await _httpClient.PostAsync("https://localhost:7154/data", DictioanryContent);
            await PrintHttpResponse(fromResponse);
        }

        private static async Task PrintHttpResponse(HttpResponseMessage fromResponse)
        {
            await Console.Out.WriteLineAsync(fromResponse.StatusCode.ToString()); ;
            await Console.Out.WriteLineAsync(await fromResponse.Content.ReadAsStringAsync());
        }

    }
}