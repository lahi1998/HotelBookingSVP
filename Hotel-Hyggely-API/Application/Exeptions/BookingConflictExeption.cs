using System;

namespace Application.Exeptions
{
    public class BookingConflictExeption : Exception
    {
        public BookingConflictExeption(string message): base(message){ }
    }
}
