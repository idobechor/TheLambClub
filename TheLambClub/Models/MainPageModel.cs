using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheLambClub.Models
{
    abstract class MainPageModel
    {
        public abstract void ShowNumericPromptCasting(object obj);
        public abstract void ShowInstructionsPrompt(object obj);
    }
}
