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
    public class CategoriesController : ControllerBase
    {
        private const string GetCategoryActionName = "GetCategory";
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Categories
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<CategoryViewModel>))]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _unitOfWork.Categories.GetCategories();
            var categoryVM = Mapper.Map<IEnumerable<CategoryViewModel>>(categories);
            return Ok(categoryVM);
        }

        // GET: api/Categories/5
        [HttpGet("{id}", Name = GetCategoryActionName)]
        [ProducesResponseType(200, Type = typeof(CategoryViewModel))]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCategory([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _unitOfWork.Categories.GetCategory(id);

            if (category == null)
                return NotFound();

            var categoryVM = Mapper.Map<CategoryViewModel>(category);
            return Ok(categoryVM);
        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        [Authorize(Policies.ManageAllArticlesPolicy)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] CategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                if (category == null)
                    return BadRequest($"{nameof(category)} cannot be null");

                if (id != category.Id)
                    return BadRequest("Conflicting category id in parameter and model data");

                var appCategory = await _unitOfWork.Categories.GetCategory(id);

                if (appCategory == null)
                    return NotFound(id);

                Mapper.Map(category, appCategory);

                var result = await _unitOfWork.Categories.UpdateCategory(appCategory);

                if (result.Item1)
                    return NoContent();

                ModelState.AddModelError(string.Empty, result.Item2);
            }

            return BadRequest(ModelState);
        }

        // POST: api/Categories
        [HttpPost]
        [Authorize(Policies.ManageAllArticlesPolicy)]
        [ProducesResponseType(201, Type = typeof(CategoryViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                if (category == null)
                    return BadRequest($"{nameof(category)} cannot be null");


                var appCategory = Mapper.Map<Category>(category);

                var result = await _unitOfWork.Categories.CreateCategory(appCategory);

                if (result.Item1)
                {
                    var categoryVM = await GetCategoryViewModelHelper(appCategory.Id);
                    return CreatedAtAction(GetCategoryActionName, new {id = categoryVM.Id}, categoryVM);
                }

                ModelState.AddModelError(string.Empty, result.Item2);
            }

            return BadRequest(ModelState);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        [Authorize(Policies.ManageAllArticlesPolicy)]
        [ProducesResponseType(200, Type = typeof(CategoryViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            CategoryViewModel categoryVM = null;
            var appCategory = await _unitOfWork.Categories.GetCategory(id);

            if (appCategory != null)
                categoryVM = await GetCategoryViewModelHelper(id);


            if (categoryVM == null)
                return NotFound(id);

            var result = await _unitOfWork.Categories.DeleteCategory(appCategory);
            if (!result.Item1)
                throw new Exception("The following errors occurred whilst deleting role: " +
                                    string.Join(", ", result.Item2));


            return Ok(categoryVM);
        }

        private async Task<CategoryViewModel> GetCategoryViewModelHelper(Guid id)
        {
            var category = await _unitOfWork.Categories.GetCategory(id);
            if (category != null)
                return Mapper.Map<CategoryViewModel>(category);


            return null;
        }
    }
}