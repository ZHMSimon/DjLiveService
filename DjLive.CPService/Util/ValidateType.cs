namespace DjLive.CPService.Util
{
    public enum ValidateType
    {
        None = 0,
        NotExist,
        PasswordError,
        Disabled,
        Freezeed,
        UnExcept,
        Verify,
        AuthCookieEmpty,
        AuthCookieError,
        AuthCookieExpired,
    }
}