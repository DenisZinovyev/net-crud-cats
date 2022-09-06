namespace DSR.CrudCats.Common
{
    using System;

    public class InternalServerErrorException : Exception
    {
        public InternalServerErrorException(string message) : base(message) { }
    }
}
