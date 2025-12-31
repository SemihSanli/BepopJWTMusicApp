using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopJWT.DTOLayer.OpenAIMusicSuggestionDTO
{
    public class OpenAIResponse
    {
        public List<Choice> Choices { get; set; }
    }
}
