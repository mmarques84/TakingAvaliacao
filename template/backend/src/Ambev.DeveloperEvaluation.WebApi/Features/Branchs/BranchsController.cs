using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Ports.Interfaces;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Branchs.CreateBranch;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branchs
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;
        private readonly IMapper _mapper;
        // Construtor com injeção de dependência do IBranchService
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
                return NotFound("Filial não encontrada.");

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
        public async Task<IActionResult> CreateBranchAsync([FromBody] CreateBranchRequest createBranchRequest)
        {
            if (createBranchRequest == null)
                return BadRequest("Dados da filial não fornecidos.");

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
                return BadRequest("Dados da filial não fornecidos.");

            try
            {
                
                var updatedBranch =  _branchService.UpdateBranchAsync(id, name);
                if (updatedBranch == null)
                    return NotFound("Filial não encontrada.");

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
                    return NotFound("Filial não encontrada.");

                return NoContent();  // Status 204 para deletar com sucesso sem retornar conteúdo
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
