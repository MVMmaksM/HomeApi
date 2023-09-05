using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeApi.Contracts.Models.Rooms
{
    /// <summary>
    /// Запрос для обновления свойств комнаты
    /// </summary>
    public class EditRoomRequest
    {
        public string NewName { get; set; }
        public int NewArea { get; set; }
    }
}
