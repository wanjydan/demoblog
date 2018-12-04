using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DAL;
using DAL.Models;
using DemoBlog.Authorization;
using DemoBlog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private const string GetTagActionName = "GetTag";
        private readonly IUnitOfWork _unitOfWork;

        public TagsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Tags
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TagViewModel>))]
        public async Task<IActionResult> GetTags()
        {
            var tags = await _unitOfWork.Tags.GetTags();
            var tagVM = Mapper.Map<IEnumerable<TagViewModel>>(tags);
            return Ok(tagVM);
        }

        // GET: api/Tags/5
        [HttpGet("{id}", Name = GetTagActionName)]
        [ProducesResponseType(200, Type = typeof(TagViewModel))]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTag([FromRoute] Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var tag = await _unitOfWork.Tags.GetTag(id);

            if (tag == null) return NotFound();

            var tagVM = Mapper.Map<TagViewModel>(tag);
            return Ok(tagVM);
        }

        // PUT: api/Tags/5
        [HttpPut("{id}")]
        [Authorize(Policies.ManageAllArticlesPolicy)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateTag([FromRoute] Guid id, [FromBody] TagViewModel tag)
        {
            if (ModelState.IsValid)
            {
                if (tag == null)
                    return BadRequest($"{nameof(tag)} cannot be null");

                if (id != tag.Id)
                    return BadRequest("Conflicting tag id in parameter and model data");

                var appTag = await _unitOfWork.Tags.GetTag(id);

                if (appTag == null)
                    return NotFound(id);

                Mapper.Map(tag, appTag);

                var result = await _unitOfWork.Tags.UpdateTag(appTag);

                if (result.Item1)
                    return NoContent();

                ModelState.AddModelError(string.Empty, result.Item2);
            }

            return BadRequest(ModelState);
        }

        // POST: api/Tags
        [HttpPost]
        [Authorize(Policies.ManageAllArticlesPolicy)]
        [ProducesResponseType(201, Type = typeof(TagViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> CreateTag([FromBody] TagViewModel tag)
        {
            if (ModelState.IsValid)
            {
                if (tag == null)
                    return BadRequest($"{nameof(tag)} cannot be null");


                var appTag = Mapper.Map<Tag>(tag);

                var result = await _unitOfWork.Tags.CreateTag(appTag);

                if (result.Item1)
                {
                    var tagVM = await GetTagViewModelHelper(appTag.Id);
                    return CreatedAtAction(GetTagActionName, new {id = tagVM.Id}, tagVM);
                }

                ModelState.AddModelError(string.Empty, result.Item2);
            }

            return BadRequest(ModelState);
        }

        // DELETE: api/Tags/5
        [HttpDelete("{id}")]
        [Authorize(Policies.ManageAllArticlesPolicy)]
        [ProducesResponseType(200, Type = typeof(TagViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteTag([FromRoute] Guid id)
        {
            TagViewModel tagVM = null;
            var appTag = await _unitOfWork.Tags.GetTag(id);

            if (appTag != null)
                tagVM = await GetTagViewModelHelper(id);


            if (tagVM == null)
                return NotFound(id);

            var result = await _unitOfWork.Tags.DeleteTag(appTag);
            if (!result.Item1)
                throw new Exception("The following errors occurred whilst deleting role: " +
                                    string.Join(", ", result.Item2));


            return Ok(tagVM);
        }

        private async Task<TagViewModel> GetTagViewModelHelper(Guid id)
        {
            var tag = await _unitOfWork.Tags.GetTag(id);
            if (tag != null)
                return Mapper.Map<TagViewModel>(tag);


            return null;
        }
    }
}