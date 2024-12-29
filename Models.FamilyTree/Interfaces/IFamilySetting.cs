using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.FamilyTree.Interfaces
{
    public interface IFamilySetting
    {
        Guid SettingID { get; set; }
        Guid FamilyID { get; set; }
        string SettingName { get; set; }
        string SettingValue { get; set; }

        IFamily Family { get; set; }
    }

}
