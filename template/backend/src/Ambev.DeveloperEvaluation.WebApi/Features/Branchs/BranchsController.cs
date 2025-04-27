using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Ports.Interfaces;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Branchs.CreateBranch;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branchs
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;
        private readonly IMapper _mapper;
  
        public BranchController(IBranchService branchService, IMapper mapper)
        {
            _branchService = branchService;
            _mapper = mapper;
        }

        // GET api/branch/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBranchByIdAsync(Guid id)
        {
            var branch = await _branchService.GetBranchByIdAsync(id);
            if (branch == null)
            {
                var errorResponse = new ErrorResponse(404, "Filial não encontrada.");
                return NotFound(errorResponse);
            }          

            return Ok(branch);
        }

        // GET api/branch
        [HttpGet]
        public async Task<IActionResult> GetAllBranchesAsync()
        {
            var branches = await _branchService.GetAllBranchesAsync();
            return Ok(branches);
        }

        // POST api/branch
        [HttpPost]
        public async Task<IActionResult> CreateBranchAsync([FromBody] CreateBranchRequest createBranchRequest,CancellationToken cancellationToken)
        {

            var validator = new CreateBranchRequestValidator();
            var validationResult = await validator.ValidateAsync(createBranchRequest, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            if (createBranchRequest == null)
            {
                var errorResponse = new ErrorResponse(400, "Dados da filial não fornecidos.");
                return BadRequest(errorResponse);
            }       

            try
            {
                var branchmapper = _mapper.Map<Branch>(createBranchRequest);
                var branch = await _branchService.CreateBranchAsync(branchmapper);
                return CreatedAtAction(nameof(GetBranchByIdAsync), new { id = branch.Id }, branch);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/branch/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBranchAsync(Guid id, string name)
        {
            if (name == null)
            {
                var errorResponse = new ErrorResponse(400, "Dados da filial não fornecidos.");
                return BadRequest(errorResponse);
            }     

            try
            {
                
                var updatedBranch =  _branchService.UpdateBranchAsync(id, name);
                if (updatedBranch == null)
                {
                    var errorResponse = new ErrorResponse(404, "Filial não encontrada.");
                    return NotFound(errorResponse);
                }

                return Ok(updatedBranch);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/branch/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBranchAsync(Guid id)
        {
            try
            {
                var deleted =  _branchService.DeleteBranchAsync(id);
                if (deleted !=null)
                {
                    var errorResponse = new ErrorResponse(404, "Filial não encontrada.");
                    return NotFound(errorResponse);
                }
           

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
