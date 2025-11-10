using System;

namespace Application.Exeptions
{
    public class UserNameTakenExeption : Exception
    {
        public UserNameTakenExeption(string message) : base(message) { }
    }
}
