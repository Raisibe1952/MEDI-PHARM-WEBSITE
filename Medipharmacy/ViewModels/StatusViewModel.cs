using static Medipharmacy.Modules.Module;

namespace Medipharmacy.ViewModels
{
    public class StatusViewModel
    {
        public Status Code { get; set; }
        public string Message { get; set; }

        public StatusViewModel()
        {
        }

        public StatusViewModel(Status code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}