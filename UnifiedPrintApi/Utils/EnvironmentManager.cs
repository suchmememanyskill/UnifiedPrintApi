namespace Utils;

public static class EnvironmentManager 
{
    public static string BaseUrl {
        get {
            var baseUrl = Environment.GetEnvironmentVariable("BASE_URL") ?? throw new Exception("BASE_URL enviroment variable not set");

            if (baseUrl.EndsWith("/"))
            {
                baseUrl = baseUrl[..^1];
            }

            return baseUrl;
        }
    }
}