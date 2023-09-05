using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeApi.Data.Queries
{
    /// <summary>
    ///  Класс для передачи дополнительных параметров при обновлении комнаты
    /// </summary>
    public class UpdateRoomQuery
    {
        public string NewName { get; }
        public int? NewArea { get; }
        public UpdateRoomQuery(string newName = null, int? newArea = null)
        {
            NewName = newName;
            NewArea = newArea;
        }
    }
}
