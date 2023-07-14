using MD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outgame
{
    public class EventHelper
    {
        /// <summary>
        /// 開催中のイベントを取得
        /// </summary>
        static public List<int> GetAllOpenedEvent()
        {
            List<int> ret = new List<int>();
            foreach (var evt in MasterData.Events)
            {
                if(IsEventOpen(evt.Id))
                {
                    ret.Add(evt.Id);
                }
            }
            return ret;
        }

        /// <summary>
        /// 開催中のイベントを表示するかどうか
        /// </summary>
        /// <param name="eventId">イベントID</param>
        static public bool IsEventOpen(int eventId)
        {
            var evt = MasterData.GetEvent(eventId);
            DateTime now = DateTime.Now;

            return evt.StartAt <= now && now <= evt.EndAt;
        }

        /// <summary>
        /// 開催中のイベントをプレイできるか
        /// </summary>
        /// <param name="eventId">イベントID</param>
        static public bool IsEventGamePlayable(int eventId)
        {
            var evt = MasterData.GetEvent(eventId);
            DateTime now = DateTime.Now;

            return evt.StartAt <= now && now <= evt.GameEndAt;
        }
    }
}
