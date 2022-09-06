namespace DSR.CrudCats.Common
{
    using System;

    public class AccessDeniedException : Exception
    {
        public AccessDeniedException(string message) : base(message) { }
    }
}
