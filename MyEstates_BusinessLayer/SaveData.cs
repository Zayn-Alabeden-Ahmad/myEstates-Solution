using MyEstates_DataAaccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEstates_BusinessLayer
{
    public interface SaveData
    {
        bool Save();
        bool IfExsist();
       
    }
}
