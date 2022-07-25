namespace rcendactgen.Common;

public static class CurrentUserInfo
{
    /// <summary>
    /// Will Return username with domain name (ie. CORPORATENETWORK\john)
    /// If the call to UserDomainName fails for whatever reason then just return username
    /// </summary>
    /// <returns></returns>
    public static string GetCurrentUserWithDomain()
    {
        string userNameWithDomain = "";
        try
        {
            userNameWithDomain += Environment.UserDomainName;
        }
        catch (Exception)
        {
            userNameWithDomain += Environment.UserName;
            return userNameWithDomain;
        }
        userNameWithDomain += $"\\{Environment.UserName}";
        return userNameWithDomain;
    }
}