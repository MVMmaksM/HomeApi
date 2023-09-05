using System.Threading.Tasks;
using AutoMapper;
using HomeApi.Contracts.Models.Rooms;
using HomeApi.Data.Models;
using HomeApi.Data.Repos;
using Microsoft.AspNetCore.Mvc;

namespace HomeApi.Controllers
{
    /// <summary>
    /// Контроллер комнат
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RoomsController : ControllerBase
    {
        private IRoomRepository _repository;
        private IMapper _mapper;

        public RoomsController(IRoomRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //TODO: Задание - добавить метод на получение всех существующих комнат

        /// <summary>
        /// Добавление комнаты
        /// </summary>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Add([FromBody] AddRoomRequest request)
        {
            var existingRoom = await _repository.GetRoomByName(request.Name);
            if (existingRoom == null)
            {
                var newRoom = _mapper.Map<AddRoomRequest, Room>(request);
                await _repository.AddRoom(newRoom);
                return StatusCode(201, $"Комната {request.Name} добавлена!");
            }

            return StatusCode(409, $"Ошибка: Комната {request.Name} уже существует.");
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Edit([FromBody] EditRoomRequest request, [FromRoute] Guid id)
        {
            var editRoom = await _repository.GetRoomById(id);
            if (editRoom is null)
                return StatusCode(400, $"Ошибка: Комнаты с идентификатором {id} не существует!");

            var withSameName = await _repository.GetRoomByName(request.NewName);
            if (withSameName is not null)
                return StatusCode(400, $"Ошибка: комната с именем {request.NewName} уже существует! Выберите другое имя!");

            await _repository.UpdateRoom(editRoom, new Data.Queries.UpdateRoomQuery(request.NewName, request.NewArea));
            return StatusCode(200, $"Комната обновлена! Имя - {editRoom.Name}, Площадь - {editRoom.Area}");
        }
    }
}