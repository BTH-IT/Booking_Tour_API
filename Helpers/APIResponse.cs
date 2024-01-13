namespace BookingApi.Helpers
{
    public class APIResponse<T>
    {
        public int StatusCode { get; set; }
        public T Result { get; set; }
        public string Message { get; set; }
    }
}