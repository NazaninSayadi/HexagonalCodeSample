using ApiGateway.Models.Connector;
using AutoMapper;
using Application.Models;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConnectorController : ControllerBase
    {
        public readonly IConnectorService _connectorService;

        public ConnectorController(IConnectorService connectorService)
        {
            _connectorService = connectorService;
        }

        [HttpGet]
        public async Task<IEnumerable<ConnectorDTO>> Get()
        {
            return await _connectorService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ConnectorDTO>> GetById(int id, Guid statitionId)
        {
            if (statitionId == Guid.Empty)
                return BadRequest("ConnectorId can not be empty");

            return await _connectorService.Get(id, statitionId);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Add(CreateConnectorInputModel Connector)
        {
            if (Connector == null)
                return BadRequest("Connector can not be null");

            if (Connector.ChargeStationId == Guid.Empty)
                return BadRequest("Please send valid Id");

            return await _connectorService.Add(Connector.MaxCurrent, Connector.ChargeStationId);
        }

        [HttpPut("{id}/{stationId}")]
        public async Task<ActionResult> Update(UpdateConnectorInputModel Connector,int id,Guid stationId)
        {
            if (Connector == null)
                return BadRequest("Connector can not be null");

            if (stationId == Guid.Empty)
                return BadRequest("Please send valid ChargeStationId");


            await _connectorService.Update(id, stationId, Connector.MaxCurrent);

            return Ok();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Remove(int id, Guid statitionId)
        {
            if (statitionId == Guid.Empty)
                return BadRequest("Please send valid Id");

            await _connectorService.Remove(id,statitionId);

            return Ok();

        }
    }
}