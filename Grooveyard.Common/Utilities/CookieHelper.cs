using Microsoft.AspNetCore.Http;

namespace Grooveyard.Common.Utilities
{
    public static class CookieHelper
    {
        public static void SetCookie(HttpResponse response, string name, string value, int? expireTime, bool isHttpOnly, bool isSecure, SameSiteMode sameSite)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = isHttpOnly,
                Secure = isSecure,
                SameSite = sameSite,
            };

            if (expireTime.HasValue)
            {
                cookieOptions.Expires = DateTimeOffset.UtcNow.AddHours(expireTime.Value);
            }

            response.Cookies.Append(name, value, cookieOptions);
        }
    }

}
