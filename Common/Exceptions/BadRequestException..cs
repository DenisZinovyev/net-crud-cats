﻿namespace DSR.CrudCats.Common
{
    using System;

    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }
    }
}
