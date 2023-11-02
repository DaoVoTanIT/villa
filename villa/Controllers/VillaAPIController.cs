using System.Net;
using AutoMapper;
using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Villa_API.Dto;
using Villa_API.Model;
using Villa_API.Repository.IRepository;

namespace Villa_API.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IMapper _mapper;
        private readonly IVillaRepository _dbVilla;
        public VillaAPIController(IMapper mapper, IVillaRepository dbVilla)
        {
            _mapper = mapper;
            _dbVilla = dbVilla;
            this._response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                IEnumerable<Villa> villasList = await _dbVilla.GetAllAsync();
                _response.Result = _mapper.Map<List<VillaDto>>(villasList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>(){
                    ex.ToString()
                };
            }
            return _response;
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        // [ProducesResponseType(200)]
        // [ProducesResponseType(404)]
        // [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillasById(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villa = await _dbVilla.GetAsync(u => u.Id == id);
                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<VillaDto>(villa);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>(){
                    ex.ToString()
                };
            }
            return _response;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDto createDto)
        {
            try
            {
                if (await _dbVilla.GetAsync(t => t.Name.ToLower() == createDto.Name.ToLower()) != null)
                {//if name is exists
                    ModelState.AddModelError("Custom model", "Villa already exists");
                    return BadRequest(ModelState);
                }
                if (createDto == null)
                {
                    return BadRequest();
                }
                Villa villa = _mapper.Map<Villa>(createDto);
                await _dbVilla.CreateAsync(villa);
                _response.Result = _mapper.Map<VillaDto>(villa);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", new { id = villa.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>(){
                    ex.ToString()
                };
            }
            return _response;
        }
        [HttpDelete]
        public async Task<ActionResult<APIResponse>> DeleteVillaAsync(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var villa = await _dbVilla.GetAsync(u => u.Id == id);
                if (villa == null)
                {
                    return NotFound();
                }
                await _dbVilla.RemoveAsync(villa);
                _response.Result = _mapper.Map<VillaDto>(villa);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>(){
                    ex.ToString()
                };
            }
            return _response;
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<APIResponse>> UpdateVillaAsync(int id, [FromBody] VillaUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.Id)
                {
                    return BadRequest();
                }
                //    var villa = await _db.Villas.FirstOrDefaultAsync(t => t.Id == id);
                Villa villa = _mapper.Map<Villa>(updateDto);
                await _dbVilla.UpdateAsync(villa);
                _response.Result = _mapper.Map<VillaDto>(villa);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>(){
                    ex.ToString()
                };
            }
            return _response;
        }
        [HttpPatch]
        public async Task<IActionResult> UpdatePartialVillaAsync(int id, JsonPatchDocument<VillaUpdateDto> patchDTO)
        {
            if (id == 0 || patchDTO == null)
            {
                return BadRequest();
            }
            var villa = await _dbVilla.GetAsync(u => u.Id == id, tracked: false);
            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);
            if (villa == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villaDto, ModelState);
            Villa model = _mapper.Map<Villa>(villaDto);
            await _dbVilla.UpdateAsync(model);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            return NoContent();
        }
    }
}