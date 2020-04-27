using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Motopark.Core.Entities;
using Motopark.Core.IServices;

namespace Motopark.API.Controllers
{
    [Route("feature")]
    [ApiController]
    public class FeatureController : Controller
    {
        private IFeatureService<Feature> _featureService;

        public FeatureController(IFeatureService<Feature> featureService)
        {
            _featureService = featureService;
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetByID(Guid id)
        {
            var feature = await _featureService.GetByID(id);
            return Ok(feature);
        }

        [HttpGet("product/{id:Guid}")]
        public async Task<IActionResult> GetByProductID(Guid id)
        {
            var features = await _featureService.GetByProductID(id);
            return Ok(features);
        }
    }
}