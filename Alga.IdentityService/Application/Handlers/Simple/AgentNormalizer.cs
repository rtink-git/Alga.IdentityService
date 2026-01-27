using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Alga.IdentityService.Application.Handlers.Simple;

internal static class AgentNormalizer
{
    public static (
        string IpNormalized,
        string? LangNormalized,
        string? SecChUaNormalized,
        string? SecCHUAPlatformNormalized,
        string? UserAgentNormalized)?
    Do(
        IPAddress ip,
        string? lang,
        string? secChUa,
        string? secCHUAPlatform,
        string? userAgent)
    {
        if (IpSubnetHelper.Normalize(ip) is not { } ipNormalized) return null;

        var langNormalazed = AcceptLanguageHelper.Normalize(lang);
        var secChUaNormalized = SecChUaHelper.Normalize(secChUa);
        var secCHUAPlatformNormalazed = SecChUaPlatformHelper.Normalize(secCHUAPlatform);
        var userAgentNormalized = UserAgentHelper.Normalize(userAgent);

        return (
            ipNormalized,
            langNormalazed,
            secChUaNormalized,
            secCHUAPlatformNormalazed,
            userAgentNormalized
        );
    }

    static class IpSubnetHelper
    {
        public static string? Normalize(IPAddress? ip)
        {
            if (ip is null) return null;

            if (ip.AddressFamily == AddressFamily.InterNetwork) // IPv4
                return NormalizeIPv4(ip);

            if (ip.AddressFamily == AddressFamily.InterNetworkV6) // IPv6
                return NormalizeIPv6(ip);

            return null;
        }

        private static string NormalizeIPv4(IPAddress ip)
        {
            var bytes = ip.GetAddressBytes();
            // /24 → первые 3 октета
            return $"{bytes[0]}.{bytes[1]}.{bytes[2]}.0/24";
        }

        private static string NormalizeIPv6(IPAddress ip)
        {
            var bytes = ip.GetAddressBytes();

            // /64 → первые 8 байт (64 бита)
            return
                $"{bytes[0]:x2}{bytes[1]:x2}:" +
                $"{bytes[2]:x2}{bytes[3]:x2}:" +
                $"{bytes[4]:x2}{bytes[5]:x2}:" +
                $"{bytes[6]:x2}{bytes[7]:x2}::/64";
        }
    }

    static class AcceptLanguageHelper
    {
        private static readonly Regex LangRegex =
            new(@"^[a-z]{2,3}(-[a-z]{2,4})?$", RegexOptions.Compiled);

        public static string? Normalize(string? acceptLanguage, int maxParts = 3)
        {
            if (string.IsNullOrWhiteSpace(acceptLanguage)) return null;

            var parts = acceptLanguage
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Split(';')[0].Trim().ToLowerInvariant())
                .Where(p => LangRegex.IsMatch(p))
                .Take(maxParts)
                .ToArray();

            if (parts.Length == 0) return null;

            return string.Join('|', parts);
        }
    }

    static class SecChUaHelper
    {
        private static readonly Regex BrandRegex = new(@"""(?<brand>[^""]+)"";\s*v=""(?<version>\d+)""", RegexOptions.Compiled);

        public static string? Normalize(string? secChUa)
        {
            if (string.IsNullOrWhiteSpace(secChUa)) return null;

            var brands = BrandRegex.Matches(secChUa)
                .Select(m => new
                {
                    Brand = m.Groups["brand"].Value.ToLowerInvariant(),
                    Version = int.Parse(m.Groups["version"].Value)
                })
                .Where(b => !b.Brand.Contains("not.a"))
                .ToList();

            if (brands.Count == 0)
                return null;

            // приоритет Chrome > Edge > Chromium
            var main = brands
                .OrderBy(b =>
                    b.Brand.Contains("chrome") ? 0 :
                    b.Brand.Contains("edge") ? 1 :
                    b.Brand.Contains("chromium") ? 2 : 3)
                .First();

            return $"{main.Brand}:{main.Version}";
        }
    }

    static class SecChUaPlatformHelper
    {
        public static string? Normalize(string? platform)
        {
            if (string.IsNullOrWhiteSpace(platform))
                return null;

            var value = platform.Trim().Trim('"').ToLowerInvariant();

            return value switch
            {
                "windows" => "windows",
                "macos" => "macos",
                "linux" => "linux",
                "android" => "android",
                "ios" => "ios",
                _ => null
            };
        }
    }

    static class UserAgentHelper
    {
        public static string? Normalize(string? userAgent)
        {
            if (string.IsNullOrWhiteSpace(userAgent))
                return null;

            var ua = userAgent.ToLowerInvariant();

            var browser = DetectBrowser(ua);
            if (browser == null) return null;

            var version = DetectBrowserMajorVersion(ua, browser);
            if (version == null) return null;

            var os = DetectOS(ua);
            if (os == null) return null;

            var device = DetectDevice(ua);
            if (device == null) return null;

            return $"{browser}:{version}:{os}:{device}";
        }

        private static string? DetectBrowser(string ua)
        {
            if (ua.Contains("edg/"))
                return "edge";
            if (ua.Contains("chrome/"))
                return "chrome";
            if (ua.Contains("firefox/"))
                return "firefox";
            if (ua.Contains("safari/") && ua.Contains("version/"))
                return "safari";

            return null;
        }

        private static string? DetectBrowserMajorVersion(string ua, string browser)
        {
            var pattern = browser switch
            {
                "edge" => @"edg/(\d+)",
                "chrome" => @"chrome/(\d+)",
                "firefox" => @"firefox/(\d+)",
                "safari" => @"version/(\d+)",
                _ => null
            };

            if (pattern == null)
                return null;

            var match = Regex.Match(ua, pattern);
            return match.Success ? match.Groups[1].Value : null;
        }

        private static string? DetectOS(string ua)
        {
            if (ua.Contains("windows"))
                return "windows";
            if (ua.Contains("mac os"))
                return "macos";
            if (ua.Contains("android"))
                return "android";
            if (ua.Contains("iphone") || ua.Contains("ipad"))
                return "ios";
            if (ua.Contains("linux"))
                return "linux";

            return null;
        }

        private static string? DetectDevice(string ua)
        {
            if (ua.Contains("mobile") || ua.Contains("android") || ua.Contains("iphone"))
                return "mobile";
            if (ua.Contains("windows") || ua.Contains("mac os") || ua.Contains("linux"))
                return "desktop";

            return null;
        }
    }
}
