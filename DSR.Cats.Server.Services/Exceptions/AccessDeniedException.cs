using System;

namespace DSR.Cats.Server.Services.Exceptions
{
    /// <summary>
    /// Thrown when user try to make changes in entity of other user. When user hasn't have permissions to change foreign entity.
    /// </summary>
    public class AccessDeniedException : Exception
    {
    }
}
