﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockMarketApi.DTOs.Comment;
using StockMarketApi.Extensions;
using StockMarketApi.Interfaces;
using StockMarketApi.Mappers;
using StockMarketApi.Models;

namespace StockMarketApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class CommentController(ICommentRepository commentRepo, IStockRepository stockRepo, UserManager<AppUser> userManager) : ControllerBase
    {

        private readonly ICommentRepository _commentRepo = commentRepo;
        private readonly IStockRepository _stockRepo = stockRepo;
        private readonly UserManager<AppUser> _userManager = userManager;


        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var comments = await _commentRepo.GetAllAsync();
            var commentsDto = comments.Select(x => x.ToCommentDto());

            return Ok(commentsDto);

        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var comment = await _commentRepo.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound("No Comment found with this id");
            }

            return Ok(comment.ToCommentDto());
        } 

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDto commentDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (! await _stockRepo.StockExistsAsync(stockId))
            {
                return BadRequest("No stock with this id");
            }

            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);



            var commentModel = commentDto.FromCreateCommentDto(stockId);
            commentModel.AppUserId = appUser.Id;
            commentModel.AppUser = appUser;
            await _commentRepo.CreateAsync(commentModel);
            return CreatedAtAction(nameof(GetById), new {id = commentModel.Id}, commentModel.ToCommentDto());

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentDto commentDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var comment = await _commentRepo.UpdateAsync(id, commentDto);
            if (comment == null)
            {
                return NotFound("No comment with this id");
            }

            return Ok(comment.ToCommentDto());


        }


        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var comment = await _commentRepo.DeleteAsync(id);
            if (comment == null)
            {
                return NotFound("no comment with this id");
            }
            return Ok(comment);
        }

    }
}
