using LanguageExt.Common;

namespace Shared.Domain;

public static partial class DomainErrors
{
    public static Error NotFound(string objectName)
    {
        return Error.New(Common.NotFound, $"{objectName} not found.");
    }

    public class Common
    {
        public const int NotFound = 101;
        public const int InvalidOperation = 102;
        public const int InternalError = 103;
    }

    public class Auth
    {
        public static class Users
        {
            public const int DuplicatedUserName = 1101;
            public const int DuplicatedEmail = 1102;
            public const int PasswordError = 1103;
        }
    }
}
