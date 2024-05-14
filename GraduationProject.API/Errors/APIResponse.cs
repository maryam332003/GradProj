namespace GraduationProject.APIs.Errors
{
    public class APIResponse
    {
        public int? StatusCode { get; set; }
        public string statusMessage { get; set; }

        public APIResponse(int statusCode, string? message)
        {
            StatusCode = statusCode;
            statusMessage = message ?? GetDefaultstatusCodeMessage(statusCode);
        }

        private string? GetDefaultstatusCodeMessage(int statusCode)
        {
            //400===>client side errors
            //500===>server side errors
            return statusCode switch
            {
                400 => "Bad Request ):",
                401 => "Unauthorized,You are not authorized ):",
                404 => "Resources not found ):",
                500 => "Server Error ):",
                _ => "Null"
            };
        }
    }
}
