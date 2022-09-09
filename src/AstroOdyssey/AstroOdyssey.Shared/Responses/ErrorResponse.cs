using System.Linq;

namespace AstroOdysseyCore
{
    public class ErrorResponse
    {
        public string[] errors { get; set; } = new string[] { };

        public ErrorResponse BuildExternalError(params string[] error)
        {
            return new ErrorResponse() { errors = error?.Where(x => x is not null)?.ToArray() };
        }
    }
}
