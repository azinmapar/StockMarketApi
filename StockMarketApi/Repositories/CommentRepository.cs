using Microsoft.EntityFrameworkCore;
using StockMarketApi.Data;
using StockMarketApi.Interfaces;
using StockMarketApi.Models;

namespace StockMarketApi.Repositories
{
    public class CommentRepository(ApplicationDbContext context) : ICommentRepository
    {

        private readonly ApplicationDbContext _context = context;

        
        public async Task<List<Comment>> GetAllAsync()
        {

            return await _context.Comments.ToListAsync();

        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }


    }
}
