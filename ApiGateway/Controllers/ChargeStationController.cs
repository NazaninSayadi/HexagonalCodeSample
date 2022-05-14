using ApiGateway.Models.ChargeStation;
using AutoMapper;
using Application.Models;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChargeStationController : ControllerBase
    {
        public readonly IChargeStationService _chargeStationService;

        public ChargeStationController(IChargeStationService chargeStationService)
        {
            _chargeStationService = chargeStationService;
        }

        [HttpGet]
        public async Task<IEnumerable<ChargeStationDTO>> Get()
        {
            return await _chargeStationService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ChargeStationDTO>> GetById(Guid Id)
        {
            if (Id == Guid.Empty)
                return BadRequest("ChargeStationId can not be empty");

            return await _chargeStationService.Get(Id);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Add(CreateChargeStationInputModel chargeStation)
        {
            if (chargeStation == null)
                return BadRequest("ChargeStation can not be null");

            if (chargeStation.GroupId == Guid.Empty)
                return BadRequest("Please send valid GroupId");

            return await _chargeStationService.Add(chargeStation.Name, chargeStation.GroupId);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(UpdateChargeStationInputModel chargeStation,Guid id)
        {
            if (chargeStation == null)
                return BadRequest("ChargeStation can not be null");

            if (id == Guid.Empty)
                return BadRequest("Please send valid ChargeStationId");

            await _chargeStationService.Update(id, chargeStation.Name);

            return Ok();

        }

       [HttpDelete("{id}")]
        public async Task<ActionResult> Remove(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Please send valid Id");

            await _chargeStationService.Remove(id);

            return Ok();

        }
    }
}