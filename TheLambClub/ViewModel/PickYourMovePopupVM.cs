using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TheLambClub.ViewModel
{
    public class PickYourMovePopupVM
    {
        public ICommand ?Stay { get; }
        public ICommand? Fold { get; }
        public PickYourMovePopupVM()
        {

        }
    }
}
