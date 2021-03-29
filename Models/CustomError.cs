namespace PersonalCrm
{
    public record CustomError()
    {
        public string Error;
        public int StatusCode;
        public string RequestId;
    };
}