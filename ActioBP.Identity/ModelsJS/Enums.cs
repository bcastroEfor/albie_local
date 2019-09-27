namespace ActioBP.Identity.ModelsJS
{
    public enum ApplicationTypes
    {
        JavaScript = 0,
        NativeConfidential = 1,
        AppMovil=2
    };
    /// <summary>
    /// 0 = Success.
    /// >= 1 = Error.
    /// >= 4 = Locked.
    /// </summary>
    public enum LoginStatusTypes
    {
        Success = 0,
        UnknownError = 1,
        DoesNotExist = 2,
        EmailNotConfirmed = 3,
        PasswordNotValid = 4,
        UserLocked_Permanent = 5,
        UserLocked_Temporal = 6,
    }
}