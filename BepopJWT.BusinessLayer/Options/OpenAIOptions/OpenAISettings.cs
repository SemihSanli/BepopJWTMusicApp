using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.BusinessLayer.Options.OpenAIOptions
{
    public class OpenAISettings
    {
        public const string OpenAI = "OpenAI"; 
        public string ApiKey { get; set; } = string.Empty;
        public string Model { get; set; } = "gpt-4o";
    }
}
