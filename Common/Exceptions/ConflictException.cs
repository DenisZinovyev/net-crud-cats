namespace DSR.CrudCats.Common
{
    using System;

    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }
}
