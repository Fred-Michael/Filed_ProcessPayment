namespace ProcessPayment.Utilities
{
    public static class Message
    {
        public static object ResponseMessage(string title, object errors = null, object data = null)
        {
            return new { title, errors, data };
        }
    }
}