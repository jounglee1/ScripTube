using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScripTube.Enums
{
    public enum EVideoStatus
    {
        OK,
        UNPLAYABLE, // maybe copyright issues (ex. music video)
        ERROR // video is invalid or does not exist
    }
}
