using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseService
{
    public interface IWareHouseService
    {
        event Action<string> ServiceInfoEvent;

        WareHouseResult GetPositionInfo(WareHousePara para);

        WareHouseResult MoveOutTray(WareHousePara para);

        WareHouseResult MoveInTray(WareHousePara para);

        WareHouseResult WriteBackData(WareHousePara para);

        WareHouseResult ResetTray(WareHousePara para);

        WareHouseResult ReleaseWriterLock(WareHousePara para);
    }
}
