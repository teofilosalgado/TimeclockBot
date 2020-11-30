using System;
using System.Linq;
using System.Net;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using TimeclockBot.Models;
using TimeclockBot.Serializer;

namespace TimeclockBot.Functions
{
    public static class Execute
    {
        private static readonly string BASE_URL = "https://platform.senior.com.br/";
        private static readonly string LOGIN_URL = "login/?redirectTo=https://platform.senior.com.br/senior-x/&tenant=kpmg.com.br";
        private static readonly string AUTHENTICATE_URL = "auth/LoginServlet";
        private static readonly string GET_EMPLOYEE_URL = "t/senior.com.br/bridge/1.0/rest/hcm/pontomobile/queries/employeeByUserQuery";
        private static readonly string CLOCKING_URL = "t/senior.com.br/bridge/1.0/rest/hcm/pontomobile/actions/clockingEventImportByBrowser";
        private static readonly string LOGOUT_URL = "auth/LogoutServlet?redirectTo=https://platform.senior.com.br/login?redirectTo=https://platform.senior.com.br/senior-x/&tenant=kpmg.com.br";

        private static CookieContainer cookies;
        private static HttpClientHandler handler;
        private static HttpClient client;

        [FunctionName("Execute")]
        public static async Task Run([TimerTrigger("0 11,15,17,21 * * 1-5")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"[TimeclockBot.Functions.Execute] Timer trigger function started at: {DateTime.Now}");

            string user = Environment.GetEnvironmentVariable("SENIOR_USER");
            string password = Environment.GetEnvironmentVariable("SENIOR_PASSWORD");
            string suffix = Environment.GetEnvironmentVariable("SENIOR_SUFFIX");
            string latitudeRaw = Environment.GetEnvironmentVariable("SENIOR_LAT");
            string longitudeRaw = Environment.GetEnvironmentVariable("SENIOR_LNG");

            double latitude = Convert.ToDouble(latitudeRaw, CultureInfo.InvariantCulture);
            double longitude = Convert.ToDouble(longitudeRaw, CultureInfo.InvariantCulture);

            cookies = new CookieContainer();
            handler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                CookieContainer = cookies,
                UseCookies = true
            };
            client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:84.0) Gecko/20100101 Firefox/84.0");
            client.BaseAddress = new Uri(BASE_URL);

            await Login(user, password, suffix);
            Employee employee = await GetEmployee();
            await Clocking(employee, latitude, longitude);
            await Logout();
            log.LogInformation($"[TimeclockBot.Functions.Execute] Timer trigger function finished at: {DateTime.Now}");
        }

        private static async Task Login(string user, string password, string suffix)
        {
            await client.GetAsync(LOGIN_URL);
            Dictionary<string, string> credentials = new Dictionary<string, string>
            {
                {"redirectTo", "https://platform.senior.com.br/senior-x/"},
                {"lng", ""},
                {"emailSuffix", suffix},
                {"expirationRememberMe", "-1"},
                {"scope", ""},
                {"user", user},
                {"password", password}
            };
            FormUrlEncodedContent content = new FormUrlEncodedContent(credentials);
            HttpResponseMessage result = await client.PostAsync(AUTHENTICATE_URL, content);

            string rawTokenCookie = cookies.GetCookies(new Uri(BASE_URL)).Where(cookie => cookie.Name == "com.senior.pau.token").First().Value;
            string parsedTokenCookie = rawTokenCookie.Replace(@"\", "").Trim('"');

            Token token = JsonSerializer.Deserialize<Token>(parsedTokenCookie);
            client.DefaultRequestHeaders.Add("Authorization", $"{token.Type} {token.Value}");
        }

        private static async Task<Employee> GetEmployee()
        {
            StringContent content = new StringContent("{}", Encoding.UTF8, "application/json");
            HttpResponseMessage result = await client.PostAsync(GET_EMPLOYEE_URL, content);
            string json = await result.Content.ReadAsStringAsync();
            EmployeeSerializer employeeSerializer = JsonSerializer.Deserialize<EmployeeSerializer>(json);
            Employee employee = employeeSerializer.Employee;
            return employee;
        }

        private static async Task Clocking(Employee employee, double latitude, double longitude)
        {
            Geolocation geolocation = new Geolocation
            {
                Latitude = latitude,
                Longitude = longitude,
                DateAndTime = DateTime.UtcNow.ToString("O")
            };
            Clocking entry = new Clocking
            {
                CNPJ = employee.Company.CNPJ,
                PIS = employee.PIS,
                AppVersion = "2.5.2",
                Employee = employee,
                Geolocation = geolocation
            };
            ClockingSerializer clocking = new ClockingSerializer
            {
                ClockingEvents = new List<Clocking> { entry }
            };
            string json = JsonSerializer.Serialize<ClockingSerializer>(clocking);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage result = await client.PostAsync(CLOCKING_URL, content);
        }

        private static async Task Logout()
        {
            HttpResponseMessage result = await client.GetAsync(LOGOUT_URL);
        }
    }
}
