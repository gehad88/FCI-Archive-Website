using System;

namespace Archive2.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; } // Represents the ID of the current request

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId); // Determines whether to show the request ID or not
    }
}
