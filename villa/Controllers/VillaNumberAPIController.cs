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
    [Route("api/VillaNumberAPI")]
    [ApiController]
    public class VillaNumberAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IMapper _mapper;
        private readonly IVillaNumberRepository _dbVillaNumber;
        private readonly IVillaRepository _dbVilla;
        public VillaNumberAPIController(IMapper mapper, IVillaNumberRepository dbVillaNumber, IVillaRepository dbVilla)
        {
            _mapper = mapper;
            _dbVillaNumber = dbVillaNumber;
            _dbVilla = dbVilla;
            this._response = new();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillasNumber()
        {
            try
            {
                IEnumerable<VillaNumber> villasNumberList = await _dbVillaNumber.GetAllAsync();
                _response.Result = _mapper.Map<List<VillaNumberDto>>(villasNumberList);
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

        [HttpGet("{id:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillasNumberById(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villaNumber = await _dbVillaNumber.GetAsync(u => u.VillaNo == id);
                if (villaNumber == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<VillaNumberDto>(villaNumber);
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
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDto createDto)
        {
            try
            {
                if (await _dbVillaNumber.GetAsync(t => t.VillaNo == createDto.VillaNo) != null)
                {//if name is exists
                    ModelState.AddModelError("Custom model", "Villa Number already exists");
                    return BadRequest(ModelState);
                }
                if (await _dbVilla.GetAsync(t => t.Id == createDto.VillaID) == null)
                {
                    ModelState.AddModelError("CustomError", "Villa ID is Invalid");
                    return BadRequest(ModelState);
                }
                if (createDto == null)
                {
                    return BadRequest();
                }
                VillaNumber villaNumber = _mapper.Map<VillaNumber>(createDto);
                await _dbVillaNumber.CreateAsync(villaNumber);
                _response.Result = _mapper.Map<VillaNumberDto>(villaNumber);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVillaNumber", new { id = villaNumber.VillaNo }, _response);
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumberAsync(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var villaNumber = await _dbVillaNumber.GetAsync(u => u.VillaNo == id);
                if (villaNumber == null)
                {
                    return NotFound();
                }
                await _dbVillaNumber.RemoveAsync(villaNumber);
                _response.Result = _mapper.Map<VillaNumberDto>(villaNumber);
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
        public async Task<ActionResult<APIResponse>> UpdateVillaAsync(int id, [FromBody] VillaNumberUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.VillaNo)
                {
                    return BadRequest();
                }
                //    var villa = await _db.Villas.FirstOrDefaultAsync(t => t.Id == id);
                VillaNumber villaNumber = _mapper.Map<VillaNumber>(updateDto);
                await _dbVillaNumber.UpdateAsync(villaNumber);
                _response.Result = _mapper.Map<VillaNumberDto>(villaNumber);
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
        public async Task<IActionResult> UpdatePartialVillaAsync(int id, JsonPatchDocument<VillaNumberUpdateDto> patchDTO)
        {
            if (id == 0 || patchDTO == null)
            {
                return BadRequest();
            }
            var villa = await _dbVillaNumber.GetAsync(u => u.VillaNo == id, tracked: false);
            VillaNumberUpdateDto villaNumberDto = _mapper.Map<VillaNumberUpdateDto>(villa);
            if (villa == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villaNumberDto, ModelState);
            VillaNumber model = _mapper.Map<VillaNumber>(villaNumberDto);
            await _dbVillaNumber.UpdateAsync(model);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            return NoContent();
        }
    }
}