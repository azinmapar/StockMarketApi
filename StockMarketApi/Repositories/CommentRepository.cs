using Microsoft.EntityFrameworkCore;
using StockMarketApi.Data;
using StockMarketApi.DTOs.Comment;
using StockMarketApi.Interfaces;
using StockMarketApi.Models;

namespace StockMarketApi.Repositories
{
    public class CommentRepository(ApplicationDbContext context) : ICommentRepository
    {

        private readonly ApplicationDbContext _context = context;

        
        public async Task<List<Comment>> GetAllAsync()
        {

            return await _context.Comments.Include(a => a.AppUser).ToListAsync();

        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.Include(a => a.AppUser).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if (comment == null)
            {
                return null;
            }

             _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> UpdateAsync(int id, UpdateCommentDto updateComment)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if (comment == null)
            {
                return null;
            }

            comment.Content = updateComment.Content;
            comment.Title = updateComment.Title;
            await _context.SaveChangesAsync();

            return comment;
        }
    }
}
