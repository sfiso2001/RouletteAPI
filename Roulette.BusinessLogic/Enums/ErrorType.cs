using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette.BusinessLogic.Enums
{
    public enum ErrorType
    {
        None = 0,
        PlayerIdNotFound = 1,
        PlayerBalanceLow = 2,
        GameReferenceDuplicated = 3,
        GameReferenceNotFound = 4
    }
}
