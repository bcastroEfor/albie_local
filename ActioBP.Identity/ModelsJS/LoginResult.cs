using ActioBP.Identity.Models;

namespace ActioBP.Identity.ModelsJS
{
    public class LogInResult
    {
        public MyUser User { get; protected set; }
        public LoginStatusTypes Status { get; set; }
        public bool IsSuccessful
        {
            get
            {
                return this.Status == LoginStatusTypes.Success;
            }
        }

        public bool IsLocked {
        get
            {
                return this.Status == LoginStatusTypes.UserLocked_Permanent || this.Status == LoginStatusTypes.UserLocked_Temporal;
            }
        }

        public LogInResult(LoginStatusTypes status)
        {
            this.Status = status;
        }
        public LogInResult(LoginStatusTypes status, MyUser user)
        {
            this.Status = status;
            this.User = user;
        }
    }
}
